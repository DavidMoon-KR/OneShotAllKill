using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Ʃ�丮��Ŵ��� ��ũ��Ʈ�� �ν��Ͻ�ȭ �� ��
    private static TutorialManager m_Instance;
    public static TutorialManager Instance => m_Instance;

    private bool m_IsTutorial = true;

    public bool IsTutorial { get => m_IsTutorial; set => m_IsTutorial = value; }

    void Awake()
    {
        m_Instance = GetComponent<TutorialManager>();
    }
}
