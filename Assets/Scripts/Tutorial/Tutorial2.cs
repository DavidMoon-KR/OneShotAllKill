using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour
{
    // 각종 오브젝트들
    [SerializeField] private Image m_RightMouseImage;    // 오른쪽 클릭 마우스 이미지
    [SerializeField] private Image m_ArrowImage;        // 화살표 이미지

    [SerializeField] private GameObject m_EnergyWall;       // 에너지벽 오브젝트

    void Update()
    {
        if (!m_EnergyWall.activeSelf)
        {
            m_RightMouseImage.gameObject.SetActive(false);
            return;
        }

        if (m_ArrowImage.gameObject.activeSelf)
        {
            m_RightMouseImage.gameObject.SetActive(false);
        }
        else
        {
            m_RightMouseImage.gameObject.SetActive(true);
        }
    }
}
