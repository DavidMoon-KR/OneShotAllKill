using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour
{
    public bool m_IsActive = true;
    public Text m_TalkText;
    public Text m_TalkTextCount;

    private List<string> m_TextList = new List<string>();
    private int m_CurrentTextIndex = 0;

    private GameObject Mark1;
    private GameObject Mark2;

    public Transform Wall1;

    private static Tutorial1 m_Instance;
    public static Tutorial1 Instance => m_Instance;

    void Start()
    {
        m_Instance = GetComponent<Tutorial1>();
        Mark1 = GameObject.Find("Mark1");
        Mark2 = GameObject.Find("Mark2");

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

        m_TalkText.text = m_TextList[m_CurrentTextIndex];
        m_CurrentTextIndex++;
        m_TalkTextCount.text = m_CurrentTextIndex + " / " + m_TextList.Count;

        Mark1.SetActive(false);
        Mark2.SetActive(false);
    }

    void Update()
    {
        if (m_CurrentTextIndex >= m_TextList.Count && Input.GetMouseButtonDown(0))
        {
            m_IsActive = false;
            gameObject.SetActive(false);
        }

        if (m_IsActive && Input.GetMouseButtonDown(0))
        {
            m_TalkText.text = m_TextList[m_CurrentTextIndex];
            m_CurrentTextIndex++;
            m_TalkTextCount.text = m_CurrentTextIndex + " / " + m_TextList.Count;
        }

        if (4 == m_CurrentTextIndex)
        {
            Mark1.SetActive(true);
        }

        if (5 == m_CurrentTextIndex)
        {
            Mark1.SetActive(false);
            Mark2.SetActive(true);

            m_IsActive = false;

            // ���� ����� �� ��ġ�� �Ű�ٸ� Ʃ�丮�� ��� ����
            if (Wall1.position.x < 1)
            {
                Mark2.SetActive(false);
                m_IsActive = true;

                m_TalkText.text = m_TextList[m_CurrentTextIndex];
                m_CurrentTextIndex++;
                m_TalkTextCount.text = m_CurrentTextIndex + " / " + m_TextList.Count;
            }
        }

        if (7 == m_CurrentTextIndex)
        {
            m_IsActive = false;

            if ( 0 >= Player.Instance.m_BulletSum)
            {
                if (0 >= GameManager.Instance.m_Targets)
                {
                    m_TalkText.text = m_TextList[m_CurrentTextIndex];
                    m_CurrentTextIndex++;
                    m_TalkTextCount.text = m_CurrentTextIndex + " / " + m_TextList.Count;
                }
            }
        }
    }
}
