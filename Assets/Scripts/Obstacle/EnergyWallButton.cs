using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWallButton : MonoBehaviour
{
    // ��ư�� �������� ���� ���� �Ǵ�
    private bool m_GetPress = false;

    [SerializeField] private List<GameObject> m_Shield;

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
        if(collision.collider.tag == "Bullet")
        {
            // ������ �溮�� ���ִٸ� ���ش�.
            if (m_GetPress == false)
            {
                m_AudioSource.clip = m_Clip;
                m_AudioSource.Play();
                
                // ������ �溮�� �ΰ� �̻��� �� �ֱ� ������ ��� �溮�� ������ �ľ��ؼ� ����
                for(int i = 0; i < m_Shield.Count; i++)
                {
                    m_Shield[i].SetActive(false);
                }
                m_GetPress = true;
            }
            // ������ �溮�� �����ִٸ� ���ش�.
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
