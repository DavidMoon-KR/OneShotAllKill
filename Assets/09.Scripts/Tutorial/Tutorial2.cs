using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Tutorial2 : MonoBehaviour
{
    // ���� ������Ʈ��
    [SerializeField] private GameObject m_GimicObject;         // ��� ������Ʈ
    [SerializeField] private GameObject m_TopRotateWall1;      // ȸ�� �� ������Ʈ 1
    [SerializeField] private GameObject m_BottomRotateWall2;   // ȸ�� �� ������Ʈ 2

    [SerializeField] private GameObject m_Laser;    // ������ �̹���
    [SerializeField] private GameObject m_Gimic2;   // ���2

    [SerializeField] private GameObject m_EnergyWall; // �������� ������Ʈ

    private bool m_IsTopWallClear = false;
    private bool m_IsBottomWallClear = false;

    // Ÿ���� ���� ������
    [SerializeField] private TMP_Text m_PopupDialogue;
    private string m_TableName = "Tutorial2"; // Localization ���̺� �̸�
    private string entryPrefix = "Tuto2_Tutorial_0"; // Entry Ű prefix
    private string m_Dialogue;
    private bool m_IsTyping = false;
    private float m_TypingSpeed = 0.05f;

    void Start()
    {
        Invoke("Gimic1", 1f);
        StartCoroutine(LoadLocalizedString());
    }

    IEnumerator LoadLocalizedString()
    {
        var table = LocalizationSettings.StringDatabase;
        var localizedString = table.GetLocalizedStringAsync(m_TableName, entryPrefix);
        yield return localizedString;
        m_Dialogue = localizedString.Result;
    }

    void Update()
    {
        if (!m_EnergyWall.activeSelf)
        {
            m_Laser.gameObject.SetActive(false);
            m_Gimic2.SetActive(true);

            m_BottomRotateWall2.GetComponent<IndicateClickAbleObject>().enabled = true;
            m_BottomRotateWall2.GetComponent<RotatingWall>().enabled = true;

            m_TopRotateWall1.GetComponent<IndicateClickAbleObject>().enabled = true;
            m_TopRotateWall1.GetComponent<RotatingWall>().enabled = true;

            return;
        }

        if (!m_IsTopWallClear &&
            m_TopRotateWall1.transform.eulerAngles.y >= 45 && m_TopRotateWall1.transform.eulerAngles.y <= 225)
        {
            m_IsTopWallClear = true;

            m_TopRotateWall1.GetComponent<IndicateClickAbleObject>().enabled = false;
            RotatingWall temp = m_TopRotateWall1.GetComponent<RotatingWall>();
            temp.MouseRButtonUp();
            temp.enabled = false;
            m_TopRotateWall1.transform.eulerAngles = new Vector3(0, 45, 0);
        }
        
        if (!m_IsBottomWallClear &&
            m_BottomRotateWall2.transform.eulerAngles.y >= 45 && m_BottomRotateWall2.transform.eulerAngles.y <= 225)
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
            m_Gimic2.SetActive(true);
        }
    }

    private void Gimic1()
    {
        m_GimicObject.SetActive(true);

        // �޽���â �ߴ� ���ȿ��� ���� �Ͻ�����
        GameManager.Instance.IsGamePause = true;
        TutorialManager.Instance.IsTutorial = true;

        StartCoroutine(Typing());
    }

    // ��ȭâ���� Ȯ�� ��ư�� Ŭ������ ���
    public void OnClickPopupButton()
    {
        // ���� Ÿ���� ���̶��
        // ������ ��簡 ��� �������� ����
        if (m_IsTyping)
        {
            m_PopupDialogue.text = m_Dialogue;
            m_IsTyping = false;
            return;
        }

        // �Ͻ����� ����
        GameManager.Instance.IsGamePause = false;
        TutorialManager.Instance.IsTutorial = false;

        m_GimicObject.SetActive(false);
    }

    // Ÿ���� ȿ��
    IEnumerator Typing()
    {
        m_PopupDialogue.text = null;
        m_IsTyping = true;

        for (int i = 0; i < m_Dialogue.Length; i++)
        {
            // Ÿ������ �����ٸ� ���̻� �������� ����
            if (!m_IsTyping)
                break;

            m_PopupDialogue.text += m_Dialogue[i];
            yield return new WaitForSeconds(m_TypingSpeed);
        }
        m_IsTyping = false;
    }
}
