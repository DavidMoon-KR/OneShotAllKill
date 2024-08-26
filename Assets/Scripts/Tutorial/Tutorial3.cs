using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.PostProcessing;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial3 : MonoBehaviour
{
    // 각종 마크들
    [SerializeField]
    private Image m_LeftMouseImage; // 왼쪽 클릭 마우스 이미지
    [SerializeField]
    private GameObject m_LinkLineImage; // 연결선 오브젝트

    void Start()
    {
        TutorialManager.Instance.IsActive = false;
    }
}
