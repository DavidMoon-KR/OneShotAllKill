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
    // 각종 오브젝트들
    [SerializeField] private Image m_LeftMouseImage;    // 왼쪽 클릭 마우스 이미지
    [SerializeField] private Image m_ArrowMark;         // 화살표 이미지

    [SerializeField] private Button EnergyWallClickButton;  // 에너지 벽 클릭 버튼
    [SerializeField] private GameObject m_EnergyWall;       // 에너지벽 오브젝트

    void Update()
    {
        if (0 >= m_ArrowMark.color.a)
        {
            m_ArrowMark.gameObject.SetActive(false);
        }

        if (!m_EnergyWall.activeSelf)
        {
            EnergyWallClickButton.gameObject.SetActive(false);
            m_LeftMouseImage.gameObject.SetActive(false);
            m_ArrowMark.gameObject.SetActive(false);
        }
    }

    public void OnClickEnergyWallButton()
    {
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        m_LeftMouseImage.gameObject.SetActive(false);

        Color color = m_ArrowMark.color;
        color.a = 1.0f;
        m_ArrowMark.color = color;
        m_ArrowMark.gameObject.SetActive(true);        
    }
}
