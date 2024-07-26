using System.Collections;
using System.Collections.Generic;
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
    private GameObject m_Explosion;

    private bool m_TargetReached = false;
    private bool m_IsReturned = false;

    // 폭발 감지를 딱 한번만 실행하도록 하기 위한 장치
    private bool m_TriggerExplosion = false;

    public NavMeshAgent m_Agent;

    private bool m_IsTriggerAni = true;
    private Animator m_Anim;

    public bool m_ExplosionDetection = false;

    public Vector3 m_ExplosionedPos;

    void Start()
    {
        m_Instance = gameObject.GetComponent<Humanoid>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 웨이포인트가 있다면
        if (m_WayPoint.Count > 0)
        {
            // 애니메이션 작동
            if(m_IsTriggerAni == true)
            {
                m_Anim.SetBool("Walk", true);
            }

            // 설정한 웨이포인트로 이동
            m_Agent.SetDestination(m_WayPoint[m_CurrentTarget].position);
            
            // 자신의 위치와 웨이포인트 위치 사이의 거리를 구함
            m_Distance = Vector3.Distance(transform.position, m_WayPoint[m_CurrentTarget].position);
            
            // 거리가 1.0 미만이라면
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
            if(m_ExplosionDetection == true)
            {
                m_Anim.SetBool("Walk", true);
                float distance = Vector3.Distance(transform.position, m_ExplosionedPos);
                m_Agent.SetDestination(m_ExplosionedPos);
                
                // 목적지에 도착하면 이동 멈춤
                if (distance < 3.3f)
                {
                    // 작동 끄기
                    m_Distance = 0;
                    m_Agent.ResetPath();
                    m_Anim.SetBool("Walk", false);
                    m_ExplosionDetection = false;

                    // 오브젝트 흔들림 버그 방지를 위해 작성함
                    Rigidbody rb = GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.isKinematic = false;
                    
                }
            }
            
        }
    }

    // 이동 전에 기다리기
    private IEnumerator WaitBeforeMoving()
    {
        // 현 웨이포인트 차례가 마지막이라면?
        if(m_CurrentTarget == 0 || m_CurrentTarget == m_WayPoint.Count - 1)
        {
            // 대기
            yield return new WaitForSeconds(m_WaitBeforeMoving);

            // 방향 전환
            if (m_CurrentTarget == 0)
            {
                m_IsReturned = false;
            }
            else
            {
                m_IsReturned = true;
            }
        }

        // 방향 전환 연산
        if(m_IsReturned == true)
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
        // 탄과 충돌하거나 가스폭발과 충돌할 경우
        if((other.tag == "Bullet" || other.tag == "GasExplosion") && m_TriggerExplosion == false)
        {
            // 폭발 상태 true로 전환
            m_TriggerExplosion = true;
            
            // 게임 매니저에서 현 스테이지에 타겟 수 감소
            GameManager.Instance.m_Targets--;
            
            // 탄과 충돌할 경우 _hasBullet에 true 전달
            if(other.tag == "Bullet")
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
        // 에너지 방벽과 충돌한 경우
        if(collision.gameObject.CompareTag("EnergyWall"))
        {
            StartCoroutine(StopMoving());
        }
    }

    // 이동 멈춤
    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.5f);
        
        // SetDestination 작동 끄기
        m_Distance = 0;
        m_Agent.ResetPath();
        m_Anim.SetBool("Walk", false);
        m_ExplosionDetection = false;
    }

    // 폭발 딜레이
    private IEnumerator ExplosionDelay(bool p_hasBullet)
    {
        // 탄과 충돌한 경우
        if(p_hasBullet == true)
        {
            yield return new WaitForSeconds(GameManager.Instance.m_DelayExplosion);
        }
        // 가스 폭발과 충돌한 경우
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        Instantiate(m_Explosion, transform.position, Quaternion.identity);

        // 스테이지 내에서 타겟이 폭파되었다는 것을 알림
        GameManager.Instance.m_HasExplosioned = true;

        // 게임매니저에게 자신이 폭발한 위치를 전달하기
        GameManager.Instance.m_ExplosionedPos = transform.position;

        // 폭발하기 전 몸을 사라지게 한다.
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
        Destroy(gameObject);
    }
}
