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
    [SerializeField] private Image m_LeftMouseImage;    // ���� Ŭ�� ���콺 �̹���
    [SerializeField] private Image m_ArrowMark;         // ȭ��ǥ �̹���

    [SerializeField] private Button EnergyWallClickButton;  // ������ �� Ŭ�� ��ư
    [SerializeField] private GameObject m_EnergyWall;       // �������� ������Ʈ

    void Update()
    {
        if (0 >= m_ArrowMark.color.a)
        {
            Color color = m_ArrowMark.color;
            color.a = 1.0f;
            m_ArrowMark.color = color;
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
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        m_LeftMouseImage.gameObject.SetActive(false);
        m_ArrowMark.gameObject.SetActive(true);
    }
}
