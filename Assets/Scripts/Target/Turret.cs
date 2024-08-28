using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{    
    [SerializeField] private float m_ImpactTime;    // 폭발했을 때 쉐이킹 시간
    [SerializeField] private float m_ImpactGauge;   // 폭발 쉐이킹 강도
    [SerializeField] private float m_RotateSpeed;   // 회전 속도

    private bool m_ExploionTrigger = false; // 폭발 했는지 여부
    
    [SerializeField] GameObject m_Explosion;    // 폭발 VFX 프리팹
    [SerializeField] private GameObject m_Spark;
    private bool m_IsHit;

    // 사운드
    [SerializeField] private AudioClip m_ExplosionSound;
    private AudioSource m_Audio;

    private void Start()
    {
        m_IsHit = false;
    }

    void Update()
    {
        if(m_IsHit == false)
        {
            transform.Rotate(0, m_RotateSpeed * Time.deltaTime, 0);
            m_Audio = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 탄알에 충돌하거나 가스 폭발과 충돌했다면
        if ((other.tag == "Bullet" || other.tag == "GasExplosion") && m_ExploionTrigger == false)
        {
            m_IsHit = true;
            Instantiate(m_Spark, transform.position, Quaternion.identity);
            // 게임매니저에서 현재 타겟 수 -1
            GameManager.Instance.m_Targets--;
            m_ExploionTrigger = true;

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

    // 폭발하기 전 잠깐의 짧은 딜레이
    private IEnumerator ExplosionDelay(bool p_hasBullet)
    {
        // 탄과 충돌한 경우
        if (p_hasBullet == true)
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

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -14f, gameObject.transform.position.z);

        CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
        Destroy(gameObject);
    }
}