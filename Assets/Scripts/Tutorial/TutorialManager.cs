using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // 튜토리얼매니저 스크립트를 인스턴스화 한 것
    private static TutorialManager m_Instance;
    public static TutorialManager Instance => m_Instance;

    private bool m_IsTutorial = true;

    public bool IsTutorial { get => m_IsTutorial; set => m_IsTutorial = value; }

    void Awake()
    {
        m_Instance = GetComponent<TutorialManager>();
    }
}
