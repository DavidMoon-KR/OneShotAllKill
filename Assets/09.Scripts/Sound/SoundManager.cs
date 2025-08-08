using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource m_BgmSound;


    // SoundManager ��ũ��Ʈ�� �ν��Ͻ�ȭ �� ��
    private static SoundManager m_Instance;
    public static SoundManager Instance => m_Instance;

    void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� ������ �ڱ� �ڽ� ����
            return;
        }

        m_Instance = this;
        DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
    }

    void Start()
    {
        m_BgmSound = GetComponent<AudioSource>();
        m_BgmSound.volume = (float)GameDataManager.Instance.Data.BgmVolume;
    }

    // ������Ʈ ����
    public void DestroySound()
    {
        Destroy(gameObject);
    }

    // ����� ����
    public void ChagneBgmSound(float p_Volume)
    {
        m_BgmSound.volume = p_Volume;
    }
}
