using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] private List<Transform> m_WayPoint;

    [SerializeField] private int m_CurrentTarget = 0;

    [SerializeField] private float m_WaitBeforeMoving;
    [SerializeField] private float m_ImpactTime;
    [SerializeField] private float m_ImpactGauge;
    private float m_Distance;

    [SerializeField] readonly float m_AgroTime = 14;
    private float m_Time;
    private float m_TurnTime;
    [SerializeField] private bool m_AgroNow;
    private Vector3 m_OriginalLocation;

    [SerializeField] private GameObject m_Explosion;
    [SerializeField] private GameObject m_Spark;

    private bool m_TargetReached = false;
    private bool m_IsReturned = false;

    private bool m_TriggerExplosion = false; // ���� �ߴ��� ����

    public NavMeshAgent m_Agent;

    private bool m_IsHit;

    private bool m_IsTriggerAni = true;
    private Animator m_Anim;

    public bool m_ExplosionDetection = false;

    public Vector3 m_ExplosionedPos;

    public bool AgroNow { get => m_AgroNow; }

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
        m_Time = 0.0f;
        m_TurnTime = 0.0f;
        m_OriginalLocation = transform.position;
        m_IsHit = false;
    }

    void Update()
    {
        if (AgroNow)
            m_Time += Time.deltaTime;
        if (m_ExplosionDetection)
            m_TurnTime += Time.deltaTime;
        if (m_IsHit == false)//�Ѿ˿� ���� �ʾ��� ���
        {
            // ��������Ʈ�� �ִٸ�
            if (m_WayPoint.Count > 0 && m_ExplosionDetection == false && AgroNow == false)
            {
                // �ִϸ��̼� �۵�
                if (m_IsTriggerAni == true)
                {
                    m_Anim.SetBool("walk", true);
                }

                // ������ ��������Ʈ�� �̵�
                m_Agent.SetDestination(m_WayPoint[m_CurrentTarget].position);

                // �ڽ��� ��ġ�� ��������Ʈ ��ġ ������ �Ÿ��� ����
                m_Distance = Vector3.Distance(transform.position, m_WayPoint[m_CurrentTarget].position);

                // �Ÿ��� 1.0 �̸��̶��
                if (m_Distance < 1.0f && m_TargetReached == false)
                {
                    m_TargetReached = true;
                    m_Anim.SetBool("walk", false);
                    m_IsTriggerAni = false;
                    StartCoroutine(WaitBeforeMoving());
                }
            }
            else
            {
                if (m_ExplosionDetection == true)
                //���İ� ���� �Ǿ��ٸ�
                {
                    m_Anim.SetBool("walk", true);
                    float distance = Vector3.Distance(transform.position, m_ExplosionedPos);
                    //���İ� ������ ��ġ�� �̵�
                    m_Agent.SetDestination(m_ExplosionedPos);
                    if ((m_Agent.velocity.sqrMagnitude > 0.0f && m_Agent.remainingDistance + 1 < Vector3.Distance(transform.position, m_ExplosionedPos)) && m_TurnTime > 0.2f)
                    {
                        if (m_WayPoint.Count > 0)
                        {
                            m_ExplosionDetection = false;
                            m_Anim.SetBool("walk", false);
                            new WaitForSeconds(3.0f);
                            return;
                        }                            
                        else
                        {
                            m_Agent.SetDestination(transform.position);
                            m_Anim.SetBool("walk", false);
                            distance = 0;
                        }                        
                    }
                    // �������� �����ϸ� �̵� ����
                    if (distance < 3.3f)
                    {
                        m_Time = 0.0f;
                        //����Ϸ� �������� ������ �Ͼ�� �ֱ� ������ 0���� �׻� �ʱ�ȭ
                        m_TurnTime = 0.0f;

                        // �۵� ����
                        m_Distance = 0;
                        m_Agent.ResetPath();
                        m_Anim.SetBool("walk", false);
                        m_AgroNow = true;
                        m_ExplosionDetection = false;
                    }
                }
            }
            if (m_AgroTime < m_Time)
            //����ϰ� �ִ� �ð��� ������ �ð����� Ŭ ���
            {
                if (m_WayPoint.Count <= 0)
                {
                    m_Anim.SetBool("walk", true);
                    float distance = Vector3.Distance(transform.position, m_OriginalLocation);
                    m_Agent.SetDestination(m_OriginalLocation);
                    if (m_Agent.velocity.sqrMagnitude > 0 && m_Agent.remainingDistance + 1 < Vector3.Distance(transform.position, m_OriginalLocation))
                    {
                        if (m_WayPoint.Count > 0)
                        {
                            m_ExplosionDetection = false;
                            new WaitForSeconds(1.0f);
                            return;
                        }
                        else
                        {
                            m_Agent.SetDestination(transform.position);
                            m_Anim.SetBool("walk", false);
                            distance = 0;
                        }
                    }
                    if (distance < 3.3f)
                    {
                        m_Distance = 0;
                        m_Agent.ResetPath();
                        m_Anim.SetBool("walk", false);
                        m_Time = 0.0f;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && m_TriggerExplosion == false)
        {
            this.GetComponent<BoxCollider>().isTrigger = true;
            StopMoving();
            m_Agent.SetDestination(transform.position);
            Instantiate(m_Spark, transform.position, Quaternion.identity);
            m_IsHit = true;
            m_Anim.SetBool("walk", false);
            //�Ѿ˿� ������ true

            m_Anim.SetBool("die", true);
            // ���� ���� true�� ��ȯ
            m_TriggerExplosion = true;

            // ���� �Ŵ������� �� ���������� Ÿ�� �� ����
            GameManager.Instance.m_Targets--;

            // ź�� �浹�� ��� _hasBullet�� true ����
            StartCoroutine(ExplosionDelay(true));
        }
        if (collision.gameObject.CompareTag("EnergyWall"))
        {
            StartCoroutine(StopMoving());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // ź�� �浹�ϰų� �������߰� �浹�� ���
        if (other.tag == "GasExplosion" && m_TriggerExplosion == false)
        {
            StopMoving();
            Instantiate(m_Spark, transform.position, Quaternion.identity);
            m_Agent.SetDestination(transform.position);
            m_IsHit = true;
            //�Ѿ˿� ������ true
            m_Anim.SetBool("walk", false);
            m_Anim.SetBool("die", true);
            // ���� ���� true�� ��ȯ
            m_TriggerExplosion = true;

            // ���� �Ŵ������� �� ���������� Ÿ�� �� ����
            GameManager.Instance.m_Targets--;
            StartCoroutine(ExplosionDelay(false));
        }
    }


    // �̵� ����
    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.0f);

        // SetDestination �۵� ����
        m_Distance = 0;
        m_Agent.ResetPath();
        m_Anim.SetBool("walk", false);
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
        GameManager.Instance.HasExplosioned = true;

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
