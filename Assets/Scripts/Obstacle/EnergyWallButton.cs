using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyWallButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_Shield;

    // ����
    [SerializeField] private AudioClip m_Clip;
    private AudioSource m_AudioSource;
    private Animator m_Anim;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ź�� �浹�� ���
        if (collision.collider.tag == "Bullet")
        {
            m_Anim.SetTrigger("Access");
            // ������ �溮�� ���ִٸ� ���ش�.
            if (m_Shield[0].activeSelf)
            {
                m_AudioSource.clip = m_Clip;
                m_AudioSource.Play();

                // ������ �溮�� �ΰ� �̻��� �� �ֱ� ������ ��� �溮�� ������ �ľ��ؼ� ����
                for (int i = 0; i < m_Shield.Count; i++)
                {
                    m_Shield[i].SetActive(false);
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

}
