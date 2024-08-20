using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.PostProcessing;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour
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

    public void OnClickEnergyWallButton()
    {
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        m_LeftMouseImage.gameObject.SetActive(!m_LeftMouseImage.gameObject.activeSelf);
        m_LinkLineImage.SetActive(!m_LinkLineImage.activeSelf);
    }
}
