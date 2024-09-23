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
    // ���� ������Ʈ��
    [SerializeField] private Image m_RightMouseImage;    // ������ Ŭ�� ���콺 �̹���
    [SerializeField] private Image m_ArrowImage;        // ȭ��ǥ �̹���

    [SerializeField] private GameObject m_EnergyWall;       // �������� ������Ʈ

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
