using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour
{
    // ���� ������Ʈ��
    [SerializeField] private GameObject m_TopRotateWall1; // ȸ�� �� ������Ʈ 1
    [SerializeField] private GameObject m_BottomRotateWall2; // ȸ�� �� ������Ʈ 2

    [SerializeField] private GameObject m_Laser; // ������ �̹���

    [SerializeField] private GameObject m_EnergyWall; // �������� ������Ʈ

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
