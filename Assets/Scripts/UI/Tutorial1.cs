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

        m_TextList.Add("안녕하세요, 이번 로봇대응타격대 실기 시험을 보시는 김중사씨! 저는 경찰 특공대에서 로봇대응타격대 공채 시험 감독관입니다.");
        m_TextList.Add("응시자께서 육군 특수전사령부 특임보병으로 중사로 전역하셨군요, 이번 테스트가 매우 기대되네요. 일단 테스트를 시작하기 전에, 테스트에 대한 설명을 하겠습니다.");
        m_TextList.Add("테스트는 총 3단계로 기본 사격 테스트, 문제 해결 능력 테스트, 전략적 사고력 테스트로 구성되어 있습니다.");
        m_TextList.Add("첫째, 기본 사격 테스트 입니다. 이 단계는 응시자께서 기초적인 사격 실력을 검증하는 테스트입니다." +
                        "응시자께서 장애물 너머에 있는 타겟을 사격하기 위해 앞에 설치된 이동 장벽을 활용할 것입니다." +
                        "이 사물의 기능은 응시자께서 발사하신 탄을 반대 각도로 도탄 시킬 수 있는 장치입니다." +
                        "그리고 응시자께서 레일 방향을 따라서 상하 또는 좌우로 장벽의 위치를 옮길 수 있습니다.");
        m_TextList.Add("표시한 위치로 한 번 옮겨봅시다.");
        m_TextList.Add("네 이렇게 하시면 됩니다. 이제 응시자께서 앞에 있는 \"모래 주머니\"라는 장애물 너머에 존재하는 타겟을 사격할 수 있는 구도를 만들었습니다.");
        m_TextList.Add("그럼, 장벽의 위치를 옮겼으니 다음으로, 장애물 너머에 있는 터렛을 사격해보시겠어요?");
        m_TextList.Add("정말 잘하셨어요! 앞으로 각 단계의 테스트에서 이렇게 장벽을 적절한 위치로 옮기시면 됩니다.");

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

            // 벽을 제대로 된 위치에 옮겼다면 튜토리얼 계속 진행
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
