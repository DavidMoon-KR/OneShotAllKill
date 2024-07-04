using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // 폭발했을 때 쉐이킹 시간
    [SerializeField]
    private float m_ImpactTime;

    // 폭발 쉐이킹 강도
    [SerializeField]
    private float m_ImpactGauge;

    // 회전 속도
    [SerializeField]
    private float m_RotateSpeed;

    // 한번만 폭발할 수 있도록 한다.
    private bool m_ExploionTrigger = false;

    // 폭발 VFX 프리팹
    [SerializeField]
    GameObject m_Explosion;

    [SerializeField]
    private AudioClip m_ExplosionSound;
    private AudioSource m_Audio;

    void Update()
    {
        transform.Rotate(0, m_RotateSpeed * Time.deltaTime, 0);
        m_Audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 탄알에 충돌하거나 가스 폭발과 충돌했다면
        if ((other.tag == "Bullet" || other.tag == "GasExplosion") && m_ExploionTrigger == false)
        {
            // 게임매니저에서 현재 타겟 수 -1
            GameManager.Instance.m_Targets--;
            StartCoroutine(ExplosionDelay());
            m_ExploionTrigger = true;

            // 탄알과 충돌한 경우 3초뒤에 파괴
            if(other.tag == "Bullet")
            {
                Destroy(gameObject, 3f);
            }
            // 아닐 경우 바로 파괴
            else
            {
                Instantiate(m_Explosion, transform.position, Quaternion.identity);
                CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
                Destroy(gameObject);
            }
        }
    }

    // 폭발하기 전 잠깐의 짧은 딜레이
    private IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(GameManager.Instance.m_DelayExplosion);
        m_Audio.clip = m_ExplosionSound;
        m_Audio.Play();

        // 폭발 프리팹 생성
        Instantiate(m_Explosion, transform.position, Quaternion.identity);
        
        // 스테이지 내에서 타겟이 폭파되었다는 것을 알림
        GameManager.Instance.m_HasExplosioned = true;
        
        // 게임매니저에게 자신이 폭발한 위치를 전달하기
        GameManager.Instance.m_ExplosionedPos = transform.position;
        
        // 폭발하기 직전 자신의 위치를 땅 밑으로 옮긴다.
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -14f, gameObject.transform.position.z);
        
        // 카메라 쉐이크 효과
        CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
    }
}
