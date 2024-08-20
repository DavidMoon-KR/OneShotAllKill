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
    private List<bool> m_IsGimicClear = new List<bool>(); // 각 기믹 클리어 여부
    private List<string> m_TextList = new List<string>(); // 모든 대사

    private bool m_IsTyping = true;     // 타이핑이 진행되고 있는지 여부
    private int m_TypingIndex = 0;      // 얼마나 타이핑 했는지에 대한 인덱스 값
    private float m_TypingSpeed = 0.015f;  // 타이핑 속도

    public Text m_TalkText;             // 현재 대사
    public Text m_NextClickText;        // 다음으로 넘어가는 방법 알려주는 텍스트
    //public Text m_TalkTextCount;        // 대사 카운트
    private int m_CurrentTextIndex = 0; // 현재 대사 인덱스
    private bool m_IsNextText = true;   // 다음 대사를 출력할 수 있는지 여부

    // 각종 마크들
    [SerializeField]
    private Image m_ArrowMark1; // 화살표1
    [SerializeField]
    private Image m_ArrowMark2; // 화살표2
    [SerializeField]
    private Image m_ArrowMark3; // 화살표3
    [SerializeField]
    private Image m_RectangleMark1; // 사각형 테두리 표시1
    [SerializeField]
    private Image m_RectangleMark2; // 사각형 테두리 표시2

    // 튜토리얼에 사용할 오브젝트들
    [SerializeField]
    private Transform m_RotateWall1; // 회전 장벽1
    [SerializeField]
    private Transform m_RotateWall2; // 회전 장벽2
    [SerializeField]
    private Transform m_EnergyShield; // 에너지 실드
    [SerializeField]
    private Transform m_Turret1; // 터렛1
    [SerializeField]
    private Transform m_Humanoid1_1; // 휴머노이드1

    // 회전 장벽 기믹 각각 클리어 여부
    private bool m_IsClearRotateWall1 = false;
    private bool m_IsClearRotateWall2 = false;

    void Start()
    {
        // 대사 저장
        m_TextList.Add("기본 사격 테스트를 모두 통과하셨군요, 역시 특임보병 출신이라 그러신지 정말 대단하군요!");
        m_TextList.Add("이제 응시자께서 앞으로 새로운 구조물을 활용하게 될 것입니다. 이 단계는 문제 해결 능력 테스트입니다.그리고 여기서 등장하는 구조물은" +
                        "\"에너지 방벽\", \"회전 장벽\"입니다.");
        m_TextList.Add("먼저, 회전 장벽을 소개하겠습니다. 이 구조물은 지금까지 활용했던 이동 장벽과는 차이점이 있습니다." +
                        "차이점 첫번째는, 이동 장벽처럼 장벽의 위치를 옮길 수 없습니다. 하지만 이동 장벽과 다르게 응시자께서 원하는대로 회전시키며 각도를 조절할 수 있습니다.");
        m_TextList.Add("다음 오브젝트를 소개하겠습니다.");
        m_TextList.Add("이 오브젝트는 \"에너지 방벽\"이라는 구조물입니다. 이 구조물에 버튼이 있는데요, 응시자께서 사격으로 이 버튼을 맞추면, 방벽이 ON 또는 OFF 됩니다.");
        m_TextList.Add("일단, 탄이 에너지 방벽의 버튼을 향해서 날아가도록 회전 장벽을 45도 회전시켜보세요.\n※ 벽을 마우스 좌클릭으로 잡고 끌어당기세요.");
        m_TextList.Add("네 이렇게 회전을 시키면 됩니다.");
        m_TextList.Add("이제 에너지 방벽의 버튼을 향해 발사하세요.");
        m_TextList.Add("잘하셨습니다. 이제 방벽이 OFF 되었으니, 터렛을 사격할 수 있는 구도가 완성되었습니다. 터렛을 파괴하세요.");
        m_TextList.Add("네 터렛이 파괴되면서, 아래에 있는 휴머노이드1.0이 터렛을 향해 이동할 것입니다." +
                        "휴머노이드1.0은 평소에 각 지점을 순환해서 이동하다가, 다른 타겟이 폭발하면, 폭발한 곳으로 이동하는 기능이 있습니다." +
                        "휴머노이드 1.0이 해당 위치로 이동할 때까지 기다리세요.");
        m_TextList.Add("휴머노이드 1.0이 해당 위치에 도착했습니다. 이제 휴머노이드 1.0을 파괴하세요.");
        m_TextList.Add("잘하셨습니다. 이제, 모든 타겟을 파괴하셨습니다.");

        // 기믹 클리어 여부 false로 설정
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);
        m_IsGimicClear.Add(false);

        // 첫 대사 자동 출력
        StartCoroutine(Typing());
        //m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;

        // 모든 마크 비활성화
        m_ArrowMark1.gameObject.SetActive(false);
        m_ArrowMark2.gameObject.SetActive(false);
        m_ArrowMark3.gameObject.SetActive(false);
        m_RectangleMark1.gameObject.SetActive(false);
        m_RectangleMark2.gameObject.SetActive(false);
    }

    void Update()
    {
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

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
            //m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;
        }
        // 타이핑 중이라면 타이핑 되고있는 대사 한번에 출력하기
        else if (m_IsNextText && TutorialManager.Instance.IsActive && m_IsTyping && Input.GetMouseButtonDown(0))
        {
            m_TypingIndex = 0;
            m_IsTyping = false;

            m_TalkText.text = m_TextList[m_CurrentTextIndex];
        }

        //================================================= 기믹관련 ====================================================
        // 에너지방벽 표시하는 UI 출력
        if (5 == m_CurrentTextIndex)
        {
            m_ArrowMark1.gameObject.SetActive(true);
            m_RectangleMark1.gameObject.SetActive(true);
            m_RectangleMark2.gameObject.SetActive(true);
        }

        // 회전 장벽에 화살표 표시
        // 회전 장벽 45도로 맞추는 기믹
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

            // 두 회전 장벽이 모두 45도로 맞추어 졌는지 체크
            // 정확히 45도를 맞추기 어려운 관계로 근처 값에 다다르면 자동으로 45도로 설정
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

            // 모든 회전 장벽을 45도로 맞췄다면 다음 대사로 넘어가기
            if (m_IsClearRotateWall1 && m_IsClearRotateWall2)
            {
                m_ArrowMark2.gameObject.SetActive(false);
                m_ArrowMark3.gameObject.SetActive(false);

                TutorialManager.Instance.IsActive = true;
                SettingNextText(0);
            }
        }

        // 에너지 방벽 버튼 맞추는 기믹
        if (!m_IsGimicClear[1] && 8 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // 만약 총알 한 발을 발사했다면
            // 더이상 행동금지, 에너지 방벽을 맞췄는지 체크하는 코루틴 호출
            if (2 >= Player.Instance.BulletSum)
            {
                TutorialManager.Instance.IsActive = true;

                StartCoroutine(EnergyWallHitCheck());
            }
        }
        // 터렛 파괴하는 기믹
        if (!m_IsGimicClear[2] && 9 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // 만약 총알 한 발을 발사했다면
            // 더이상 행동금지, 터렛을 맞췄는지 체크하는 코루틴 호출
            if (1 >= Player.Instance.BulletSum)
            {
                TutorialManager.Instance.IsActive = true;

                StartCoroutine(TurretHitCheck());
            }
        }

        // 휴머노이드가 폭발한 위치로 갔는지 체크
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

        // 휴머노이드 파괴하는 기믹
        if (!m_IsGimicClear[4] && 11 == m_CurrentTextIndex)
        {
            TutorialManager.Instance.IsActive = false;
            m_IsNextText = false;
            m_NextClickText.gameObject.SetActive(false);

            // 휴머노이드를 파괴했다면 다음 대사로 넘어가기
            if (m_Humanoid1_1 == null)
            {
                TutorialManager.Instance.IsActive = true;
                SettingNextText(4);
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

        // 에너지 버튼을 맞췄다면 다음 대사로 넘어가기
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

        // 에너지 버튼을 맞췄다면 다음 대사로 넘어가기
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
        //m_TalkTextCount.text = (m_CurrentTextIndex + 1) + " / " + m_TextList.Count;
    }
}
