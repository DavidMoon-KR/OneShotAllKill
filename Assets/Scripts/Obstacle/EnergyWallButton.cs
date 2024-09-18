using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyWallButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Shield;
    [SerializeField] private List<Image> m_ArrowMark;
    // 사운드
    [SerializeField] private AudioClip m_Clip;
    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 탄과 충돌할 경우
        if (collision.collider.tag == "Bullet")
        {
            // 에너지 방벽이 켜있다면 꺼준다.
            if (m_Shield[0].activeSelf)
            {
                m_AudioSource.clip = m_Clip;
                m_AudioSource.Play();

                // 에너지 방벽이 두개 이상일 수 있기 때문에 모든 방벽의 개수를 파악해서 끄기
                for (int i = 0; i < m_Shield.Count; i++)
                {
                    m_Shield[i].SetActive(false);
                    m_ArrowMark[i].gameObject.SetActive(false);
                }
            }
            // 에너지 방벽이 꺼져있다면 켜준다.
            else if (!m_Shield[0].activeSelf)
            {
                for (int i = 0; i < m_Shield.Count; i++)
                {
                    m_Shield[i].SetActive(true);
                }
            }
        }
    }

    private void OnMouseOver()
    {
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < m_ArrowMark.Count; i++)
            {
                if (!m_Shield[i].activeSelf)
                    continue;

                Color color = m_ArrowMark[i].color;
                color.a = 1.0f;
                m_ArrowMark[i].color = color;
                m_ArrowMark[i].gameObject.SetActive(true);
            }
        }
    }
}
