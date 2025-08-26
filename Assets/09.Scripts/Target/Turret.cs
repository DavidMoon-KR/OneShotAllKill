using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Turret : MonoBehaviour
{    
    [SerializeField] private float m_ImpactTime;    // 폭발했을 때 쉐이킹 시간
    [SerializeField] private float m_AddExpTime;    // 추가로 더해지는 폭발 딜레이 시간 25-01-15
    [SerializeField] private float m_ImpactGauge;   // 폭발 쉐이킹 강도
    [SerializeField] private float m_RotateSpeed;   // 회전 속도
    
    [SerializeField] GameObject m_Explosion;    // 폭발 VFX 프리팹
    [SerializeField] private GameObject m_Spark;
    private bool m_IsExplotion;
    private Animator m_Anim;
    private NavMeshSurface m_NavMeshSurface;// 네비메쉬
    // 사운드
    //[SerializeField] private AudioClip m_ExplosionSound;

    private void Start()
    {
        if (null != GameObject.Find("Navigation"))
        {
            m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();
        }
        m_Anim = GetComponent<Animator>();
        m_Anim.SetBool("turnright", true);
        m_IsExplotion = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 탄알에 충돌하거나 가스 폭발과 충돌했다면
        if (collision.gameObject.CompareTag("Bullet") && m_IsExplotion == false)
        {
            m_IsExplotion = true;
            m_Anim.SetBool("turnright", false);
            m_Anim.SetBool("die", true);
            Instantiate(m_Spark, transform.position, Quaternion.identity);
            // 게임매니저에서 현재 타겟 수 -1
            GameManager.Instance.m_Targets--;
            this.GetComponent<CapsuleCollider>().isTrigger = true;
            StartCoroutine(ExplosionDelay(true));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 탄알에 충돌하거나 가스 폭발과 충돌했다면
        if (other.gameObject.CompareTag("GasExplosion"))
        {
            m_IsExplotion = true;
            m_Anim.SetBool("turnright", false);
            m_Anim.SetBool("die", true);
            Instantiate(m_Spark, transform.position, Quaternion.identity);
            // 게임매니저에서 현재 타겟 수 -1
            if(!m_IsExplotion)
                GameManager.Instance.m_Targets--;
            StartCoroutine(ExplosionDelay(false));
        }
    }

    // 폭발하기 전 잠깐의 짧은 딜레이
    private IEnumerator ExplosionDelay(bool p_hasBullet)
    {
        // 탄과 충돌한 경우
        if (p_hasBullet == true)
        {
            //기존에 딜레이 시간에서 추가로 새 변수를 더함 25-01-15
            yield return new WaitForSeconds(GameManager.Instance.m_DelayExplosion + m_AddExpTime);
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);

            if (m_NavMeshSurface != null)
            {
                m_NavMeshSurface.BuildNavMesh();
            }
        }
        // 가스 폭발과 충돌한 경우
        else
        {
            yield return new WaitForSeconds(0.3f);
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);

            if (m_NavMeshSurface != null)
            {
                m_NavMeshSurface.BuildNavMesh();
            }
        }
        GameObject obj = Instantiate(m_Explosion, transform.position, Quaternion.identity);
        obj.GetComponent<AudioSource>().volume = (float)GameDataManager.Instance.Data.SfxVolume;

        // 스테이지 내에서 타겟이 폭파되었다는 것을 알림
        GameManager.Instance.HasExplosioned = true;

        // 게임매니저에게 자신이 폭발한 위치를 전달하기
        GameManager.Instance.m_ExplosionedPos = transform.position;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -14f, gameObject.transform.position.z);
        
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject);
    }
}