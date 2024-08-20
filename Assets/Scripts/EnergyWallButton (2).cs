using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWallButton : MonoBehaviour
{
    // 버튼을 눌렀는지 상태 여부 판단
    private bool m_GetPress = false;

    [SerializeField]
    private List<GameObject> m_Shield;

    [SerializeField]
    private AudioClip m_Clip;
    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 탄과 충돌할 경우
        if(collision.collider.tag == "Bullet")
        {
            // 에너지 방벽이 켜있다면 꺼준다.
            if (m_GetPress == false)
            {
                m_AudioSource.clip = m_Clip;
                m_AudioSource.Play();
                
                // 에너지 방벽이 두개 이상일 수 있기 때문에 모든 방벽의 개수를 파악해서 끄기
                for(int i = 0; i < m_Shield.Count; i++)
                {
                    m_Shield[i].SetActive(false);
                }
                m_GetPress = true;
            }
            // 에너지 방벽이 꺼져있다면 켜준다.
            else if (m_GetPress == true)
            {
                for (int i = 0; i < m_Shield.Count; i++)
                {
                    m_Shield[i].SetActive(true);
                }
                m_GetPress = false;
            }
        }
    }
}
