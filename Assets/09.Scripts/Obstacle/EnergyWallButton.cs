using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnergyWallButton : MonoBehaviour
{
    public List<GameObject> m_Shield;

    // ����
    [SerializeField] private AudioClip m_Clip;
    [SerializeField] private Material m_ChangeColor;
    private float m_Time = 0.0f;
    private bool m_ButtonPush = false;
    private MeshRenderer m_Button;
    private Material OriginMaterial;
    private AudioSource m_AudioSource;
    private Animator m_Anim;

    void Start()
    {        
        m_Button = transform.GetChild(0).GetComponent<MeshRenderer>();
        OriginMaterial = m_Button.material;
        m_Anim = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        m_AudioSource.volume = (float)GameDataManager.Instance.Data.SfxVolume;
    }

    private void Update()
    {
        if (m_ButtonPush)
            m_Time += Time.deltaTime;
        if(m_Time > 0.3f)
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ChangeButtonColor();
                m_ButtonPush = false;
                m_Time = 0.0f;
            }                
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ź�� �浹�� ���
        if (collision.collider.tag == "Bullet")
        {
            m_Anim.SetTrigger("Access");
            m_ButtonPush = true;
            ChangeButtonColor();
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
    private void ChangeButtonColor()
    {
        if (m_Button.material == OriginMaterial)
            m_Button.material = m_ChangeColor;
        else
            m_Button.material = OriginMaterial;
    }
}
