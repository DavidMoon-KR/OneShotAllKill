using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour
{
    private int m_NowGimicNumber = 1;

    // ���� ��ũ �� ǥ�õ�
    [SerializeField] private GameObject m_FirstGimicObjects;
    [SerializeField] private GameObject m_SecondGimicObjects;

    // Ʃ�丮�� ����� ������Ʈ��
    [SerializeField] private Transform m_RicochetWall1; // �̵� �庮1
    [SerializeField] private Transform m_Turret1;       // �ͷ�

    void Update()
    {
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        // 1�� ���
        // ���� ������ ��ġ�� �ű��
        if (1 == m_NowGimicNumber)
        {
            m_FirstGimicObjects.SetActive(true);

            // ���� ����� �� ��ġ�� �Ű�ٸ� ���� ������� �Ѿ��
            if (m_RicochetWall1.position.x < 1)
            {
                m_FirstGimicObjects.SetActive(false);

                m_NowGimicNumber++;
            }
        }

        // 2�� ���
        // �ͷ� �ı��ϱ�
        if (2 == m_NowGimicNumber)
        {
            m_SecondGimicObjects.SetActive(true);

            // �ͷ��� �ı��ߴٸ� ���� ���� �Ѿ��
            if (m_Turret1 == null)
            {
                m_SecondGimicObjects.SetActive(false);

                m_NowGimicNumber++;
            }
        }
    }
}
