using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private bool m_IsActive = true;

    private static TutorialManager m_Instance;
    public static TutorialManager Instance => m_Instance;

    public bool IsActive { get => m_IsActive; set => m_IsActive = value; }
    private void Start()
    {
        m_Instance = GetComponent<TutorialManager>();
    }
}
