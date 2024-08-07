using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour
{
    private List<bool> m_IsGimicClear = new List<bool>(); // 각 기믹 클리어 여부
    private List<string> m_TextList = new List<string>(); // 모든 대사

    private bool m_IsTyping = true;     // 타이핑이 진행되고 있는지 여부
    private int m_TypingIndex = 0;      // 얼마나 타이핑 했는지에 대한 인덱스 값
    private float typingSpeed = 0.03f;  // 타이핑 속도

    public Text m_TalkText;             // 현재 대사
    public Text m_NextClickText;        // 다음으로 넘어가는 방법 알려주는 텍스트
    public Text m_TalkTextCount;        // 대사 카운트
    private int m_CurrentTextIndex = 0; // 현재 대사 인덱스
    private bool m_IsNextText = true;   // 다음 대사를 출력할 수 있는지 여부

    // 각종 마크들
    [SerializeField]
    private Image m_ArrowMark1; // 화살표1
    [SerializeField]
    private Image m_ArrowMark2; // 화살표2

    // 튜토리얼에 사용할 오브젝트들
    [SerializeField]
    private Transform m_RicochetWall1; // 이동 장벽1
    [SerializeField]
    private Transform m_Turret1; // 터렛

    void Start()
    {
        // 대사 저장
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

        // 기믹 클리어 여부 false로 설정
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);

        // 첫 대사 자동 출력
        StartCoroutine(Typing());
        m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;

        // 모든 마크 비활성화
        m_ArrowMark1.gameObject.SetActive(false);
        m_ArrowMark2.gameObject.SetActive(false);
    }

    void Update()
    {
        // 모든 대사가 출력되었다면 대사창 없애기
        if (m_CurrentTextIndex >= m_TextList.Count && Input.GetMouseButtonDown(0))
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            gameObject.SetActive(false);
        }

        // 타이핑이 끝났을 때 클릭하면 다음 대사로 넘어가기
        if (m_IsNextText && TutorialManager.Instance.IsActive && !m_IsTyping && Input.GetMouseButtonDown(0))
        {
            m_TypingIndex = 0;
            StartCoroutine(Typing());
            m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;
        }
        // 타이핑 중이라면 타이핑 되고있는 대사 한번에 출력하기
        else if (m_IsNextText && TutorialManager.Instance.IsActive && m_IsTyping && Input.GetMouseButtonDown(0))
        {
            m_TypingIndex = 0;
            m_IsTyping = false;

            m_TalkText.text = m_TextList[m_CurrentTextIndex];
        }

        //================================================= 기믹관련 ====================================================
        // 이동 장벽 표시하는 UI 출력
        if (4 == m_CurrentTextIndex)
        {
            m_ArrowMark1.gameObject.SetActive(true);
        }

        // 이동 장벽을 옮길 위치에 화살표 표시
        // 이동 장벽 옮기는 기믹
        if (!m_IsGimicClear[0] && 5 == m_CurrentTextIndex)
        {
            m_ArrowMark1.gameObject.SetActive(false);
            m_ArrowMark2.gameObject.SetActive(true);
            
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // 벽을 제대로 된 위치에 옮겼다면 다음 대사로 넘어가기
            if (m_RicochetWall1.position.x < 1)
            {
                m_ArrowMark2.gameObject.SetActive(false);

                TutorialManager.Instance.IsActive = true;
                SettingNextText(0);
            }
        }

        // 터렛 파괴하는 기믹
        if (!m_IsGimicClear[1] && 7 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // 터렛을 파괴했다면 다음 대사로 넘어가기
            if (m_Turret1 == null)
            {
                TutorialManager.Instance.IsActive = true;
                SettingNextText(1);
            }
        }
    }

    // 타이핑 효과를 주기 위한 코루틴
    private IEnumerator Typing()
    {
        m_IsTyping = true;

        // 텍스트를 한글자씩 타이핑치듯 재생
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
