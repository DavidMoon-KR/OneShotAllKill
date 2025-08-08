using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource m_BgmSound;


    // SoundManager 스크립트를 인스턴스화 한 것
    private static SoundManager m_Instance;
    public static SoundManager Instance => m_Instance;

    void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 자기 자신 제거
            return;
        }

        m_Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
    }

    void Start()
    {
        m_BgmSound = GetComponent<AudioSource>();
        m_BgmSound.volume = (float)GameDataManager.Instance.Data.BgmVolume;
    }

    // 오브젝트 삭제
    public void DestroySound()
    {
        Destroy(gameObject);
    }

    // 배경음 조절
    public void ChagneBgmSound(float p_Volume)
    {
        m_BgmSound.volume = p_Volume;
    }
}
