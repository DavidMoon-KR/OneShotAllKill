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
    // ���� ��ũ��
    [SerializeField]
    private Image m_LeftMouseImage; // ���� Ŭ�� ���콺 �̹���
    [SerializeField]
    private GameObject m_LinkLineImage; // ���ἱ ������Ʈ

    void Start()
    {
        TutorialManager.Instance.IsActive = false;
    }

    public void OnClickEnergyWallButton()
    {
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        m_LeftMouseImage.gameObject.SetActive(!m_LeftMouseImage.gameObject.activeSelf);
        m_LinkLineImage.SetActive(!m_LinkLineImage.activeSelf);
    }
}
