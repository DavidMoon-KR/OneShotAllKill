using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering.PostProcessing;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial3 : MonoBehaviour
{
    private int m_NowGimicNumber = 1;

    [SerializeField] private List<GameObject> m_GimicObjects;

    [SerializeField] private TMP_Text m_PopupDialogue;
    [SerializeField] private TMP_Text m_PopupBtnText;
    private List<string> m_DialogueList = new List<string>();
    private string m_ClosingText = "�ݱ�";
    private int m_NowDialogue = -1;
    private bool m_IsTyping = false;
    private float m_TypingSpeed = 0.05f;

    [SerializeField] private List<GameObject> m_EnergyWall;
    [SerializeField] private GameObject m_Turret;
    [SerializeField] private GameObject m_SecurityCameraCone;

    void Start()
    {
        m_DialogueList.Add("����ī�޶� ���ø��� �ֽ��ϴ�");
        m_DialogueList.Add("���ø��� ������ �������� �ʵ��� �����ϼ���");
        m_DialogueList.Add("����ī�޶�� EMPź(<sprite=0>)�� Ȱ���ؼ� ����ȭ�ϼ���");
    }

    void Update()
    {
        if (3 == m_NowGimicNumber)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_EnergyWall[0].SetActive(false);

                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);
                m_NowGimicNumber++;
            }            
        }

        if (4 == m_NowGimicNumber)
        {
            if(m_Turret == null && !m_SecurityCameraCone.activeSelf)
            {
                m_EnergyWall[0].SetActive(true);
                m_EnergyWall[1].SetActive(false);
                m_EnergyWall[2].SetActive(false);

                m_NowGimicNumber++;
            }
        }
    }

    // ����ī�޶� Ŭ������ ���
    public void OnClickSecurityCameraButton()
    {
        EventSystem.current.currentSelectedGameObject.gameObject.SetActive(false);

        m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);
        m_NowGimicNumber++;
        m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

        OnClickPopupButton();
    }

    // ��ȭâ���� ����, �ݱ� ��ư�� Ŭ������ ���
    public void OnClickPopupButton()
    {
        if (m_IsTyping)
        {
            m_PopupDialogue.text = m_DialogueList[m_NowDialogue];
            m_IsTyping = false;
            return;
        }

        m_NowDialogue++;

        // �ݱ� ��ư�� ������ ���
        if (m_NowDialogue > (m_DialogueList.Count - 1))
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);
            m_NowGimicNumber++;
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);
            return;
        }
        // ������ �ؽ�Ʈ�� ���
        if (m_NowDialogue >= (m_DialogueList.Count - 1))
        {
            m_PopupBtnText.text = m_ClosingText;
        }

        StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        m_PopupDialogue.text = null;
        m_IsTyping = true;

        for (int i = 0; i < m_DialogueList[m_NowDialogue].Length; i++)
        {
            // Ÿ������ �����ٸ� ���̻� �������� ����
            if (!m_IsTyping)
                break;

            // <sprite=0>�� ���� ������ �ּ������� ���� ���� �Ⱥ��̰� �����
            if (m_DialogueList[m_NowDialogue][i] == '<')
            {
                m_TypingSpeed = 0;
            }
            // <sprite=0>�� ������ �ٽ� ����ӵ��� ����
            else if (m_DialogueList[m_NowDialogue][i] == '>')
            {
                m_TypingSpeed = 0.05f;
            }

            m_PopupDialogue.text += m_DialogueList[m_NowDialogue][i];
            yield return new WaitForSeconds(m_TypingSpeed);
        }
        m_IsTyping = false;
    }
}
