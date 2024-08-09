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
    private List<bool> m_IsGimicClear = new List<bool>(); // �� ��� Ŭ���� ����
    private List<string> m_TextList = new List<string>(); // ��� ���

    private bool m_IsTyping = true;     // Ÿ������ ����ǰ� �ִ��� ����
    private int m_TypingIndex = 0;      // �󸶳� Ÿ���� �ߴ����� ���� �ε��� ��
    private float m_TypingSpeed = 0.015f;  // Ÿ���� �ӵ�

    public Text m_TalkText;             // ���� ���
    public Text m_NextClickText;        // �������� �Ѿ�� ��� �˷��ִ� �ؽ�Ʈ
    public Text m_TalkTextCount;        // ��� ī��Ʈ
    private int m_CurrentTextIndex = 0; // ���� ��� �ε���
    private bool m_IsNextText = true;   // ���� ��縦 ����� �� �ִ��� ����

    // ���� ��ũ��
    [SerializeField]
    private Image m_ArrowMark1; // ȭ��ǥ1
    [SerializeField]
    private Image m_ArrowMark2; // ȭ��ǥ2
    [SerializeField]
    private Image m_ArrowMark3; // ȭ��ǥ3
    [SerializeField]
    private Image m_RectangleMark1; // �簢�� �׵θ� ǥ��1
    [SerializeField]
    private Image m_RectangleMark2; // �簢�� �׵θ� ǥ��2

    // Ʃ�丮�� ����� ������Ʈ��
    [SerializeField]
    private Transform m_RotateWall1; // ȸ�� �庮1
    [SerializeField]
    private Transform m_RotateWall2; // ȸ�� �庮2
    [SerializeField]
    private Transform m_EnergyShield; // ������ �ǵ�
    [SerializeField]
    private Transform m_Turret1; // �ͷ�1
    [SerializeField]
    private Transform m_Humanoid1_1; // �޸ӳ��̵�1

    // ȸ�� �庮 ��� ���� Ŭ���� ����
    private bool m_IsClearRotateWall1 = false;
    private bool m_IsClearRotateWall2 = false;

    void Start()
    {
        // ��� ����
        m_TextList.Add("�⺻ ��� �׽�Ʈ�� ��� ����ϼ̱���, ���� Ư�Ӻ��� ����̶� �׷����� ���� ����ϱ���!");
        m_TextList.Add("���� �����ڲ��� ������ ���ο� �������� Ȱ���ϰ� �� ���Դϴ�. �� �ܰ�� ���� �ذ� �ɷ� �׽�Ʈ�Դϴ�.�׸��� ���⼭ �����ϴ� ��������" +
                        "\"������ �溮\", \"ȸ�� �庮\"�Դϴ�.");
        m_TextList.Add("����, ȸ�� �庮�� �Ұ��ϰڽ��ϴ�. �� �������� ���ݱ��� Ȱ���ߴ� �̵� �庮���� �������� �ֽ��ϴ�." +
                        "������ ù��°��, �̵� �庮ó�� �庮�� ��ġ�� �ű� �� �����ϴ�. ������ �̵� �庮�� �ٸ��� �����ڲ��� ���ϴ´�� ȸ����Ű�� ������ ������ �� �ֽ��ϴ�.");
        m_TextList.Add("���� ������Ʈ�� �Ұ��ϰڽ��ϴ�.");
        m_TextList.Add("�� ������Ʈ�� \"������ �溮\"�̶�� �������Դϴ�. �� �������� ��ư�� �ִµ���, �����ڲ��� ������� �� ��ư�� ���߸�, �溮�� ON �Ǵ� OFF �˴ϴ�.");
        m_TextList.Add("�ϴ�, ź�� ������ �溮�� ��ư�� ���ؼ� ���ư����� ȸ�� �庮�� 45�� ȸ�����Ѻ�����.\n�� ���� ���콺 ��Ŭ������ ��� �����⼼��.");
        m_TextList.Add("�� �̷��� ȸ���� ��Ű�� �˴ϴ�.");
        m_TextList.Add("���� ������ �溮�� ��ư�� ���� �߻��ϼ���.");
        m_TextList.Add("���ϼ̽��ϴ�. ���� �溮�� OFF �Ǿ�����, �ͷ��� ����� �� �ִ� ������ �ϼ��Ǿ����ϴ�. �ͷ��� �ı��ϼ���.");
        m_TextList.Add("�� �ͷ��� �ı��Ǹ鼭, �Ʒ��� �ִ� �޸ӳ��̵�1.0�� �ͷ��� ���� �̵��� ���Դϴ�." +
                        "�޸ӳ��̵�1.0�� ��ҿ� �� ������ ��ȯ�ؼ� �̵��ϴٰ�, �ٸ� Ÿ���� �����ϸ�, ������ ������ �̵��ϴ� ����� �ֽ��ϴ�." +
                        "�޸ӳ��̵� 1.0�� �ش� ��ġ�� �̵��� ������ ��ٸ�����.");
        m_TextList.Add("�޸ӳ��̵� 1.0�� �ش� ��ġ�� �����߽��ϴ�. ���� �޸ӳ��̵� 1.0�� �ı��ϼ���.");
        m_TextList.Add("���ϼ̽��ϴ�. ����, ��� Ÿ���� �ı��ϼ̽��ϴ�.");

        // ��� Ŭ���� ���� false�� ����
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);

        // ù ��� �ڵ� ���
        StartCoroutine(Typing());
        m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;

        // ��� ��ũ ��Ȱ��ȭ
        m_ArrowMark1.gameObject.SetActive(false);
        m_ArrowMark2.gameObject.SetActive(false);
        m_ArrowMark3.gameObject.SetActive(false);
        m_RectangleMark1.gameObject.SetActive(false);
        m_RectangleMark2.gameObject.SetActive(false);
    }

    void Update()
    {
        // ��� ��簡 ��µǾ��ٸ� ���â ���ֱ�
        if (m_CurrentTextIndex >= m_TextList.Count && Input.GetMouseButtonDown(0))
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            gameObject.SetActive(false);
        }
        // Ÿ������ ������ �� Ŭ���ϸ� ���� ���� �Ѿ��
        if (m_IsNextText && TutorialManager.Instance.IsActive && !m_IsTyping && Input.GetMouseButtonDown(0))
        {
            m_TypingIndex = 0;
            StartCoroutine(Typing());
            m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;
        }
        // Ÿ���� ���̶�� Ÿ���� �ǰ��ִ� ��� �ѹ��� ����ϱ�
        else if (m_IsNextText && TutorialManager.Instance.IsActive && m_IsTyping && Input.GetMouseButtonDown(0))
        {
            m_TypingIndex = 0;
            m_IsTyping = false;

            m_TalkText.text = m_TextList[m_CurrentTextIndex];
        }

        //================================================= ��Ͱ��� ====================================================
        // �������溮 ǥ���ϴ� UI ���
        if (5 == m_CurrentTextIndex)
        {
            m_ArrowMark1.gameObject.SetActive(true);
            m_RectangleMark1.gameObject.SetActive(true);
            m_RectangleMark2.gameObject.SetActive(true);
        }

        // ȸ�� �庮�� ȭ��ǥ ǥ��
        // ȸ�� �庮 45���� ���ߴ� ���
        if (!m_IsGimicClear[0] && 6 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            m_ArrowMark1.gameObject.SetActive(false);
            m_RectangleMark1.gameObject.SetActive(false);
            m_RectangleMark2.gameObject.SetActive(false);

            m_IsClearRotateWall1 = false;
            m_IsClearRotateWall2 = false;
            m_ArrowMark2.gameObject.SetActive(true);
            m_ArrowMark3.gameObject.SetActive(true);

            // �� ȸ�� �庮�� ��� 45���� ���߾� ������ üũ
            // ��Ȯ�� 45���� ���߱� ����� ����� ��ó ���� �ٴٸ��� �ڵ����� 45���� ����
            if (m_RotateWall1.eulerAngles.y >= 44 && m_RotateWall1.eulerAngles.y <= 46
                || m_RotateWall1.eulerAngles.y >= 224 && m_RotateWall1.eulerAngles.y <= 226)
            {
                int rotateY = m_RotateWall1.eulerAngles.y <= 46 ? 45 : 225;

                m_RotateWall1.rotation = Quaternion.Euler(0, rotateY, 0);
                m_IsClearRotateWall1 = true;
            }
            if (m_RotateWall2.eulerAngles.y >= 44 && m_RotateWall2.eulerAngles.y <= 46
                || m_RotateWall2.eulerAngles.y >= 224 && m_RotateWall2.eulerAngles.y <= 226)
            {
                int rotateY = m_RotateWall2.eulerAngles.y <= 46 ? 45 : 225;

                m_RotateWall2.rotation = Quaternion.Euler(0, rotateY, 0);
                m_IsClearRotateWall2 = true;
            }

            // ��� ȸ�� �庮�� 45���� ����ٸ� ���� ���� �Ѿ��
            if (m_IsClearRotateWall1 && m_IsClearRotateWall2)
            {
                m_ArrowMark2.gameObject.SetActive(false);
                m_ArrowMark3.gameObject.SetActive(false);

                TutorialManager.Instance.IsActive = true;
                SettingNextText(0);
            }
        }

        // ������ �溮 ��ư ���ߴ� ���
        if (!m_IsGimicClear[1] && 8 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // ���� �Ѿ� �� ���� �߻��ߴٸ�
            // ���̻� �ൿ����, ������ �溮�� ������� üũ�ϴ� �ڷ�ƾ ȣ��
            if (2 >= Player.Instance.BulletSum)
            {
                TutorialManager.Instance.IsActive = true;

                StartCoroutine(EnergyWallHitCheck());
            }
        }
        // �ͷ� �ı��ϴ� ���
        if (!m_IsGimicClear[2] && 9 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // ���� �Ѿ� �� ���� �߻��ߴٸ�
            // ���̻� �ൿ����, �ͷ��� ������� üũ�ϴ� �ڷ�ƾ ȣ��
            if (1 >= Player.Instance.BulletSum)
            {
                TutorialManager.Instance.IsActive = true;

                StartCoroutine(TurretHitCheck());
            }
        }

        // �޸ӳ��̵尡 ������ ��ġ�� ������ üũ
        if (!m_IsGimicClear[3] && 10 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            if (m_Humanoid1_1.GetComponent<Humanoid>().AgroNow)
            {
                TutorialManager.Instance.IsActive = true;
                SettingNextText(3);
            }
        }

        // �޸ӳ��̵� �ı��ϴ� ���
        if (!m_IsGimicClear[4] && 11 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // �޸ӳ��̵带 �ı��ߴٸ� ���� ���� �Ѿ��
            if (m_Humanoid1_1 == null)
            {
                TutorialManager.Instance.IsActive = true;
                SettingNextText(4);
            }
        }
    }

    // Ÿ���� ȿ���� �ֱ� ���� �ڷ�ƾ
    private IEnumerator Typing()
    {
        m_IsTyping = true;

        // �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
        while (m_TypingIndex < m_TextList[m_CurrentTextIndex].Length)
        {
            if (!m_IsTyping)
            {
                break;
            }

            m_TalkText.text = m_TextList[m_CurrentTextIndex].Substring(0, m_TypingIndex);

            m_TypingIndex++;

            yield return new WaitForSeconds(m_TypingSpeed);
        }

        m_IsTyping = false;
        m_CurrentTextIndex++;
    }

    private IEnumerator EnergyWallHitCheck()
    {
        yield return new WaitForSeconds(3.0f);

        if (m_IsGimicClear[1])
            yield break;

        // ������ ��ư�� ����ٸ� ���� ���� �Ѿ��
        if (!m_EnergyShield.gameObject.activeSelf)
        {
            SettingNextText(1);
        }
        else
        {
            GameManager.Instance.m_IsGameOver = true;
            GameManager.Instance.m_IsFailed = true;
        }
    }
    private IEnumerator TurretHitCheck()
    {
        yield return new WaitForSeconds(3.0f);

        if (m_IsGimicClear[2])
            yield break;

        // ������ ��ư�� ����ٸ� ���� ���� �Ѿ��
        if (m_Turret1 == null)
        {
            SettingNextText(2);
        }
        else
        {
            GameManager.Instance.m_IsGameOver = true;
            GameManager.Instance.m_IsFailed = true;
        }
    }

    private void SettingNextText(int p_gimicNumber)
    {
        m_IsNextText = true;
        m_IsGimicClear[p_gimicNumber] = true;
        m_NextClickText.gameObject.SetActive(true);
        m_TypingIndex = 0;
        StartCoroutine(Typing());
        m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;
    }
}
