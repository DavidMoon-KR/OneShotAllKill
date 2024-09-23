using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyWallButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Shield;
    [SerializeField] private List<Image> m_ArrowMark;
    // ����
    [SerializeField] private AudioClip m_Clip;
    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ź�� �浹�� ���
        if (collision.collider.tag == "Bullet")
        {
            // ������ �溮�� ���ִٸ� ���ش�.
            if (m_Shield[0].activeSelf)
            {
                m_AudioSource.clip = m_Clip;
                m_AudioSource.Play();

                // ������ �溮�� �ΰ� �̻��� �� �ֱ� ������ ��� �溮�� ������ �ľ��ؼ� ����
                for (int i = 0; i < m_Shield.Count; i++)
                {
                    m_Shield[i].SetActive(false);
                    m_ArrowMark[i].gameObject.SetActive(false);
                }
            }
            // ������ �溮�� �����ִٸ� ���ش�.
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
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
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
