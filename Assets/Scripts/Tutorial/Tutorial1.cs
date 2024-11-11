using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour
{
    private int m_NowGimicNumber = 1;

    // ���� ��ũ �� ǥ�õ�
    [SerializeField] private List<GameObject> m_GimicObjects;

    // Ʃ�丮�� ����� ������Ʈ��
    [SerializeField] private Transform m_RicochetWall1; // �̵� �庮1
    [SerializeField] private Transform m_Turret1;       // �ͷ�

    [SerializeField] private GameObject m_WallObjects;

    [SerializeField] private TMP_Text m_PopupDialogue;
    private string m_Dialogue = "���� ��Ŭ������ ��� �����⼼��";
    private bool m_IsTyping = false;
    private float m_TypingSpeed = 0.05f;

    // Ʃ�丮��1 ��ũ��Ʈ�� �ν��Ͻ�ȭ �� ��
    private static Tutorial1 m_Instance;
    public static Tutorial1 Instance => m_Instance;

    void Awake()
    {
        m_Instance = GetComponent<Tutorial1>();
    }

    void Update()
    {
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        // 1�� ���
        // �� Ŭ��
        // OnClickWall()

        // 2�� ���
        // �˾� ��ư Ŭ��
        // OnClickPopupButton()

        // 3�� ���
        // ���� ������ ��ġ�� �ű��
        if (3 == m_NowGimicNumber)
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

            // ���� ����� �� ��ġ�� �Ű�ٸ� ���� ������� �Ѿ��
            if (m_RicochetWall1.position.x < 1)
            {
                m_GimicObjects[0].SetActive(false);
                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);

                m_NowGimicNumber++;
            }
        }

        // 4�� ���
        // �ͷ� �ı��ϱ�
        if (4 == m_NowGimicNumber)
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

            // �ͷ��� �ı��ߴٸ� ���� ���� �Ѿ��
            if (m_Turret1 == null)
            {
                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);

                m_NowGimicNumber++;
            }
        }
    }

    // ���� ��Ŭ���ϸ� ����
    public void OnClickWall()
    {
        // ���� ���(���â) Ȱ��ȭ �� ���� ����Ű�� UI ��Ȱ��ȭ
        m_NowGimicNumber++;
        m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);
        m_WallObjects.gameObject.SetActive(false);

        // �޽���â �ߴ� ���ȿ��� ���� �Ͻ�����
        GameManager.Instance.IsGamePause = true;
        TutorialManager.Instance.IsTutorial = true;

        // Ÿ���� ġ���� ��� ���
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


        // �������(���â) ��Ȱ��ȭ �� ���� ���(�̵���� ���̱�) Ȱ��ȭ
        m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);

        m_NowGimicNumber++;
        m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);
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
