using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraillerController : MonoBehaviour
{
    public static TraillerController Instance = null;

    [SerializeField] private bool m_IsPlayTrailler = true;
    [SerializeField] private GameObject m_TraillerFiles;
    [SerializeField] private GameObject m_SoundManager;

    public bool IsPlayTrailler { get => m_IsPlayTrailler; }

    void Awake()
    {
        // SoundManager 인스턴스가 이미 있는지 확인, 이 상태로 설정
        if (Instance == null)
            Instance = this;

        // 인스턴스가 이미 있는 경우 오브젝트 제거
        else if (Instance != this)
            Destroy(gameObject);

        // 이렇게 하면 다음 scene으로 넘어가도 오브젝트가 사라지지 않습니다.
        DontDestroyOnLoad(gameObject);
    }
    
    void Update()
    {
        if (Input.anyKeyDown && m_IsPlayTrailler)
        {
            m_IsPlayTrailler = false;

            if (m_TraillerFiles != null)
            {
                m_TraillerFiles.SetActive(false);
                m_SoundManager.SetActive(true);
            }
        }
    }
}
