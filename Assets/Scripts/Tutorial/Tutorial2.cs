using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour
{
    // 각종 오브젝트들
    [SerializeField] private GameObject m_TopRotateWall1; // 회전 벽 오브젝트 1
    [SerializeField] private GameObject m_BottomRotateWall2; // 회전 벽 오브젝트 2

    [SerializeField] private GameObject m_Laser; // 레이저 이미지

    [SerializeField] private GameObject m_EnergyWall; // 에너지벽 오브젝트

    private bool m_IsTopWallClear = false;
    private bool m_IsBottomWallClear = false;

    void Update()
    {
        if (!m_EnergyWall.activeSelf)
        {
            m_Laser.gameObject.SetActive(false);

            m_BottomRotateWall2.GetComponent<IndicateClickAbleObject>().enabled = true;
            m_BottomRotateWall2.GetComponent<RotatingWall>().enabled = true;

            m_TopRotateWall1.GetComponent<IndicateClickAbleObject>().enabled = true;
            m_TopRotateWall1.GetComponent<RotatingWall>().enabled = true;

            return;
        }

        if (!m_IsTopWallClear && m_TopRotateWall1.transform.eulerAngles.y >= 45)
        {
            m_IsTopWallClear = true;

            m_TopRotateWall1.GetComponent<IndicateClickAbleObject>().enabled = false;
            RotatingWall temp = m_TopRotateWall1.GetComponent<RotatingWall>();
            temp.MouseRButtonUp();
            temp.enabled = false;
            m_TopRotateWall1.transform.eulerAngles = new Vector3(0, 45, 0);
        }

        if (!m_IsBottomWallClear && m_BottomRotateWall2.transform.eulerAngles.y >= 45)
        {
            m_IsBottomWallClear = true;

            m_BottomRotateWall2.GetComponent<IndicateClickAbleObject>().enabled = false;
            RotatingWall temp = m_BottomRotateWall2.GetComponent<RotatingWall>();
            temp.MouseRButtonUp();
            temp.enabled = false;
            m_BottomRotateWall2.transform.eulerAngles = new Vector3(0, 45, 0);
        }

        if (m_IsBottomWallClear && m_IsTopWallClear)
        {
            m_Laser.SetActive(true);
        }
    }
}
