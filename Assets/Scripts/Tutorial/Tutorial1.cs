using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour
{
    private List<bool> m_IsGimicClear = new List<bool>(); // �� ��� Ŭ���� ����
    private List<string> m_TextList = new List<string>(); // ��� ���

    private bool m_IsTyping = true;     // Ÿ������ ����ǰ� �ִ��� ����
    private int m_TypingIndex = 0;      // �󸶳� Ÿ���� �ߴ����� ���� �ε��� ��
    private float typingSpeed = 0.03f;  // Ÿ���� �ӵ�

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

    // Ʃ�丮�� ����� ������Ʈ��
    [SerializeField]
    private Transform m_RicochetWall1; // �̵� �庮1
    [SerializeField]
    private Transform m_Turret1; // �ͷ�

    void Start()
    {
        // ��� ����
        m_TextList.Add("�ȳ��ϼ���, �̹� �κ�����Ÿ�ݴ� �Ǳ� ������ ���ô� ���߻羾! ���� ���� Ư���뿡�� �κ�����Ÿ�ݴ� ��ä ���� �������Դϴ�.");
        m_TextList.Add("�����ڲ��� ���� Ư������ɺ� Ư�Ӻ������� �߻�� �����ϼ̱���, �̹� �׽�Ʈ�� �ſ� ���ǳ׿�. �ϴ� �׽�Ʈ�� �����ϱ� ����, �׽�Ʈ�� ���� ������ �ϰڽ��ϴ�.");
        m_TextList.Add("�׽�Ʈ�� �� 3�ܰ�� �⺻ ��� �׽�Ʈ, ���� �ذ� �ɷ� �׽�Ʈ, ������ ���� �׽�Ʈ�� �����Ǿ� �ֽ��ϴ�.");
        m_TextList.Add("ù°, �⺻ ��� �׽�Ʈ �Դϴ�. �� �ܰ�� �����ڲ��� �������� ��� �Ƿ��� �����ϴ� �׽�Ʈ�Դϴ�." +
                        "�����ڲ��� ��ֹ� �ʸӿ� �ִ� Ÿ���� ����ϱ� ���� �տ� ��ġ�� �̵� �庮�� Ȱ���� ���Դϴ�." +
                        "�� �繰�� ����� �����ڲ��� �߻��Ͻ� ź�� �ݴ� ������ ��ź ��ų �� �ִ� ��ġ�Դϴ�." +
                        "�׸��� �����ڲ��� ���� ������ ���� ���� �Ǵ� �¿�� �庮�� ��ġ�� �ű� �� �ֽ��ϴ�.");
        m_TextList.Add("ǥ���� ��ġ�� �� �� �Űܺ��ô�.");
        m_TextList.Add("�� �̷��� �Ͻø� �˴ϴ�. ���� �����ڲ��� �տ� �ִ� \"�� �ָӴ�\"��� ��ֹ� �ʸӿ� �����ϴ� Ÿ���� ����� �� �ִ� ������ ��������ϴ�.");
        m_TextList.Add("�׷�, �庮�� ��ġ�� �Ű����� ��������, ��ֹ� �ʸӿ� �ִ� �ͷ��� ����غ��ðھ��?");
        m_TextList.Add("���� ���ϼ̾��! ������ �� �ܰ��� �׽�Ʈ���� �̷��� �庮�� ������ ��ġ�� �ű�ø� �˴ϴ�.");

        // ��� Ŭ���� ���� false�� ����
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);

        // ù ��� �ڵ� ���
        StartCoroutine(Typing());
        m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;

        // ��� ��ũ ��Ȱ��ȭ
        m_ArrowMark1.gameObject.SetActive(false);
        m_ArrowMark2.gameObject.SetActive(false);
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
        // �̵� �庮 ǥ���ϴ� UI ���
        if (4 == m_CurrentTextIndex)
        {
            m_ArrowMark1.gameObject.SetActive(true);
        }

        // �̵� �庮�� �ű� ��ġ�� ȭ��ǥ ǥ��
        // �̵� �庮 �ű�� ���
        if (!m_IsGimicClear[0] && 5 == m_CurrentTextIndex)
        {
            m_ArrowMark1.gameObject.SetActive(false);
            m_ArrowMark2.gameObject.SetActive(true);
            
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // ���� ����� �� ��ġ�� �Ű�ٸ� ���� ���� �Ѿ��
            if (m_RicochetWall1.position.x < 1)
            {
                m_ArrowMark2.gameObject.SetActive(false);

                TutorialManager.Instance.IsActive = true;
                SettingNextText(0);
            }
        }

        // �ͷ� �ı��ϴ� ���
        if (!m_IsGimicClear[1] && 7 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // �ͷ��� �ı��ߴٸ� ���� ���� �Ѿ��
            if (m_Turret1 == null)
            {
                TutorialManager.Instance.IsActive = true;
                SettingNextText(1);
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

            yield return new WaitForSeconds(typingSpeed);
        }

        m_IsTyping = false;
        m_CurrentTextIndex++;
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
