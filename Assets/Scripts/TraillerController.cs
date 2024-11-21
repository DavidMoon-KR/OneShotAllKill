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
        // SoundManager �ν��Ͻ��� �̹� �ִ��� Ȯ��, �� ���·� ����
        if (Instance == null)
            Instance = this;

        // �ν��Ͻ��� �̹� �ִ� ��� ������Ʈ ����
        else if (Instance != this)
            Destroy(gameObject);

        // �̷��� �ϸ� ���� scene���� �Ѿ�� ������Ʈ�� ������� �ʽ��ϴ�.
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
