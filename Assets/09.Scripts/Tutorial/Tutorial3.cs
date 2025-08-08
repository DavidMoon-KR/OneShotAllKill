using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class Tutorial3 : MonoBehaviour
{
    private int m_NowGimicNumber = 1;

    [SerializeField] private List<GameObject> m_GimicObjects;

    [SerializeField] private TMP_Text m_PopupDialogue;
    [SerializeField] private TMP_Text m_PopupBtnText;
    private int m_DialogueCount = 3;
    private string m_TableName = "Tutorial3"; // Localization ���̺� �̸�
    private string entryPrefix = "Tuto3_Tutorial_"; // Entry Ű prefix
    private string entryPrefix_Close = "Tuto3_Close";
    private string m_Dialogue;
    private string m_ClosingText;
    private int m_NowDialogue = -1;
    private bool m_IsTyping = false;
    private float m_TypingSpeed = 0.05f;

    [SerializeField] private List<GameObject> m_EnergyWall;
    [SerializeField] private GameObject m_Turret;
    [SerializeField] private GameObject m_SecurityCameraCone;

    void Start()
    {
        TutoMeesage();
    }

    IEnumerator LoadLocalizedString()
    {
        var table = LocalizationSettings.StringDatabase;
        string entrystring = entryPrefix + m_NowDialogue;
        var localizedString = table.GetLocalizedStringAsync(m_TableName, entrystring);
        yield return localizedString;
        m_Dialogue = localizedString.Result;

        localizedString = table.GetLocalizedStringAsync(m_TableName, entryPrefix_Close);
        yield return localizedString;
        m_ClosingText = localizedString.Result;

        StartCoroutine(Typing());
    }

    void Update()
    {
        if (2 == m_NowGimicNumber)
        {
            TutorialManager.Instance.IsTutorial = false;
            GameManager.Instance.IsGamePause = false;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                TutorialManager.Instance.IsTutorial = true;

                m_EnergyWall[0].SetActive(false);

                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);
                m_NowGimicNumber++;
            }            
        }

        if (3 == m_NowGimicNumber)
        {
            if(m_Turret == null && !m_SecurityCameraCone.activeSelf)
            {
                TutorialManager.Instance.IsTutorial = false;

                m_EnergyWall[0].SetActive(true);
                m_EnergyWall[1].SetActive(false);
                m_EnergyWall[2].SetActive(false);

                m_NowGimicNumber++;
            }
        }
    }

    // ���� �������ڸ��� Ʃ�丮�� �޽��� ���
    public void TutoMeesage()
    {
        GameManager.Instance.IsGamePause = true;
        TutorialManager.Instance.IsTutorial = true;

        m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

        OnClickPopupButton();
    }

    // ��ȭâ���� ����, Ȯ�� ��ư�� Ŭ������ ���
    public void OnClickPopupButton()
    {
        if (m_IsTyping)
        {
            m_PopupDialogue.text = m_Dialogue;
            m_IsTyping = false;
            return;
        }

        m_NowDialogue++;
        StartCoroutine(LoadLocalizedString());

        // �ݱ� ��ư�� ������ ���
        if (m_NowDialogue > (m_DialogueCount - 1))
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);
            m_NowGimicNumber++;
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);
            return;
        }
        // ������ �ؽ�Ʈ�� ���
        if (m_NowDialogue >= (m_DialogueCount - 1))
        {
            m_PopupBtnText.text = m_ClosingText;
        }
    }

    IEnumerator Typing()
    {
        m_PopupDialogue.text = null;
        m_IsTyping = true;

        for (int i = 0; i < m_Dialogue.Length; i++)
        {
            // Ÿ������ �����ٸ� ���̻� �������� ����
            if (!m_IsTyping)
                break;

            // <sprite=0>�� ���� ������ �ּ������� ���� ���� �Ⱥ��̰� �����
            if (m_Dialogue[i] == '<')
            {
                m_TypingSpeed = 0;
            }
            // <sprite=0>�� ������ �ٽ� ����ӵ��� ����
            else if (m_Dialogue[i] == '>')
            {
                m_TypingSpeed = 0.05f;
            }

            m_PopupDialogue.text += m_Dialogue[i];
            yield return new WaitForSeconds(m_TypingSpeed);
        }
        m_IsTyping = false;
    }
}
