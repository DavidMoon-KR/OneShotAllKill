using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    private static Humanoid m_Instance;
    public static Humanoid Instance => m_Instance;

    [SerializeField]
    private List<Transform> m_WayPoint;

    [SerializeField]
    private int m_CurrentTarget = 0;

    [SerializeField]
    private float m_WaitBeforeMoving;
    [SerializeField]
    private float m_ImpactTime;
    [SerializeField]
    private float m_ImpactGauge;
    private float m_Distance;
    [SerializeField]
    private float m_AgroTime;
    private float m_Time;
    private bool m_AgroNow;
    private Vector3 m_OriginalLocation;

    [SerializeField]
    private GameObject m_Explosion;

    [SerializeField]
    private GameObject m_Spark;

    private bool m_TargetReached = false;
    private bool m_IsReturned = false;

    // ���� ������ �� �ѹ��� �����ϵ��� �ϱ� ���� ��ġ
    private bool m_TriggerExplosion = false;

    public NavMeshAgent m_Agent;

    private bool m_IsHit;

    private bool m_IsTriggerAni = true;
    private Animator m_Anim;

    public bool m_ExplosionDetection = false;

    public Vector3 m_ExplosionedPos;

    public bool AgroNow { get => m_AgroNow; }

    void Start()
    {
        m_Instance = gameObject.GetComponent<Humanoid>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        m_Time = 0.0f;
        m_OriginalLocation = transform.position;
        m_IsHit = false;
    }

    void Update()
    {
        if (m_AgroNow)
            m_Time += Time.deltaTime;
        // ��������Ʈ�� �ִٸ�
        if(m_IsHit == false)
        {
            if (m_WayPoint.Count > 0 && m_ExplosionDetection == false)
            {
                // �ִϸ��̼� �۵�
                if (m_IsTriggerAni == true)
                {
                    m_Anim.SetBool("Walk", true);
                }

                // ������ ��������Ʈ�� �̵�
                m_Agent.SetDestination(m_WayPoint[m_CurrentTarget].position);

                // �ڽ��� ��ġ�� ��������Ʈ ��ġ ������ �Ÿ��� ����
                m_Distance = Vector3.Distance(transform.position, m_WayPoint[m_CurrentTarget].position);

                // �Ÿ��� 1.0 �̸��̶��
                if (m_Distance < 1.0f && m_TargetReached == false)
                {
                    m_TargetReached = true;
                    m_Anim.SetBool("Walk", false);
                    m_IsTriggerAni = false;
                    StartCoroutine(WaitBeforeMoving());
                }
            }
            else
            {
                if (m_ExplosionDetection == true)
                {
                    m_Anim.SetBool("Walk", true);
                    float distance = Vector3.Distance(transform.position, m_ExplosionedPos);
                    m_Agent.SetDestination(m_ExplosionedPos);

                    // �������� �����ϸ� �̵� ����
                    if (distance < 3.3f)
                    {
                        // �۵� ����
                        m_Distance = 0;
                        m_Agent.ResetPath();
                        m_Anim.SetBool("Walk", false);

                        // ������Ʈ ��鸲 ���� ������ ���� �ۼ���
                        Rigidbody rb = GetComponent<Rigidbody>();
                        rb.isKinematic = true;
                        rb.isKinematic = false;
                        m_AgroNow = true;
                    }
                }
            }
            if (m_AgroTime < m_Time)
            {
                if (m_WayPoint.Count <= 0)
                {
                    m_Anim.SetBool("Walk", true);
                    float distance = Vector3.Distance(transform.position, m_OriginalLocation);
                    m_Agent.SetDestination(m_OriginalLocation);
                    if (distance < 3.3f)
                    {
                        m_Distance = 0;
                        m_Agent.ResetPath();
                        m_Anim.SetBool("Walk", false);
                        m_Time = 0.0f;

                        // ������Ʈ ��鸲 ���� ������ ���� �ۼ���
                        Rigidbody rb = GetComponent<Rigidbody>();
                        rb.isKinematic = true;
                        rb.isKinematic = false;
                    }
                }
                m_AgroNow = false;
                m_ExplosionDetection = false;
            }
        }
    }

    // �̵� ���� ��ٸ���
    private IEnumerator WaitBeforeMoving()
    {
        // �� ��������Ʈ ���ʰ� �������̶��?
        if (m_CurrentTarget == 0 || m_CurrentTarget == m_WayPoint.Count - 1)
        {
            // ���
            yield return new WaitForSeconds(m_WaitBeforeMoving);

            // ���� ��ȯ
            if (m_CurrentTarget == 0)
            {
                m_IsReturned = false;
            }
            else
            {
                m_IsReturned = true;
            }
        }

        // ���� ��ȯ ����
        if (m_IsReturned == true)
        {
            m_CurrentTarget--;
        }
        else
        {
            m_CurrentTarget++;
        }

        m_IsTriggerAni = true;
        m_TargetReached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ź�� �浹�ϰų� �������߰� �浹�� ���
        if ((other.tag == "Bullet" || other.tag == "GasExplosion") && m_TriggerExplosion == false)
        {
            StopMoving();
            Instantiate(m_Spark, transform.position, Quaternion.identity); 
            m_IsHit = true;
            m_Anim.SetBool("Hit", true);
            // ���� ���� true�� ��ȯ
            m_TriggerExplosion = true;

            // ���� �Ŵ������� �� ���������� Ÿ�� �� ����
            GameManager.Instance.m_Targets--;

            // ź�� �浹�� ��� _hasBullet�� true ����
            if (other.tag == "Bullet")
            {
                StartCoroutine(ExplosionDelay(true));
            }
            else
            {
                StartCoroutine(ExplosionDelay(false));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ������ �溮�� �浹�� ���
        if (collision.gameObject.CompareTag("EnergyWall"))
        {
            StartCoroutine(StopMoving());
        }
    }

    // �̵� ����
    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.0f);

        // SetDestination �۵� ����
        m_Distance = 0;
        m_Agent.ResetPath();
        m_Anim.SetBool("Walk", false);
        m_ExplosionDetection = false;
    }

    // ���� ������
    private IEnumerator ExplosionDelay(bool p_hasBullet)
    {

        // ź�� �浹�� ���
        if (p_hasBullet == true)
        {
            yield return new WaitForSeconds(GameManager.Instance.m_DelayExplosion);
        }
        // ���� ���߰� �浹�� ���
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        Instantiate(m_Explosion, transform.position, Quaternion.identity);

        // �������� ������ Ÿ���� ���ĵǾ��ٴ� ���� �˸�
        GameManager.Instance.m_HasExplosioned = true;

        // ���ӸŴ������� �ڽ��� ������ ��ġ�� �����ϱ�
        GameManager.Instance.m_ExplosionedPos = transform.position;

        // �����ϱ� �� ���� ������� �Ѵ�.
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
        Destroy(gameObject);
    }
}
