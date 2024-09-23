using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 각 스테이지별 아이디
public enum StageID
{
    NotActive = 0, // 아직 스테이지를 열지 않았음을 체크 // 기본값

    // 기본 사격 단계
    Level1_Stage1 = 1010,
    Level1_Stage2,
    Level1_Stage3,

    // 문제 해결 능력 단계
    Level2_Stage1 = 1020,
    Level2_Stage2,
    Level2_Stage3,

    // 전략적 사고력 단계
    Level3_Stage1 = 1030,
    Level3_Stage2,
    Level3_Stage3,
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_GameTitle;   // 게임 제목

    // 스테이지 관련
    [Header ("< Stage >")]
    [SerializeField] private Button m_StageButton;  // 스테이지 버튼
    [SerializeField] private GameObject m_Level;    // 레벨 그룹
    [SerializeField] private Image m_LevelWindowBackGround;  // 레벨 상세창 배경

    // 스테이지 리스트 관련
    [Header("- StageList")]
    [SerializeField] private GameObject m_StageListWindow;          // 스테이지 리스트 창
    [SerializeField] private List<GameObject> m_LevelWindowList;    // 레벨 리스트 창

    [SerializeField] private List<Image> m_LevelMark;   // 레벨 마크
    [SerializeField] private Sprite m_MarkOnSprite;     // 켜진 화살표 스프라이트
    [SerializeField] private Sprite m_MarkOffSprite;    // 꺼진 화살표 스프라이트

    [SerializeField] private Image m_PreviousArrowImage;    // 이전 페이지로 가는 화살표 UI
    [SerializeField] private Image m_NextArrowImage;        // 다음 페이지로 가는 화살표 UI
    [SerializeField] private Sprite m_ArrowOnSprite;        // 켜진 화살표 스프라이트
    [SerializeField] private Sprite m_ArrowOffSprite;       // 꺼진 화살표 스프라이트

    // 스테이지 정보 관련
    [Header("- StageInfo")]
    [SerializeField] private GameObject m_StageInfoWindow;          // 스테이지 정보 창
    [SerializeField] private List<GameObject> m_LevelWindowInfo;    // 레벨 정보 창
    
    [SerializeField] private List<GameObject> m_StageWindowInfo;        // 스테이지 정보 창
    [SerializeField] private List<GameObject> m_LevelWindowInfo_Title;  // 레벨 별 타이틀 정보

    private GameObject m_ClickLevel;        // 클릭한 레벨
    private int m_SelectLevelNumber = 0;    // 클릭한 레벨 숫자

    private StageID m_SelectStageID = StageID.NotActive; // 스테이지 아이디
    private int m_SelectStageIDValue = 0;                // 스테이지 아이디를 스테이지 단계에 맞게 정수로 변환한 값

    private const int m_Level1_StageCount = 3; // 기본 사격 단계 스테이지 수
    private const int m_Level2_StageCount = 3; // 문제 해결 능력 단계 스테이지 수
    private const int m_Level3_StageCount = 3; // 전략적 사고력 단계 스테이지 수


    // 스테이지 버튼 눌렀을 때
    public void OnClickStage()
    {
        m_Level.SetActive(true);            // 각 레벨 보여주기
        m_StageButton.interactable = false; // 더이상 스테이지 버튼 못누르게 스테이지 버튼 비활성화
    }

    // 레벨 그룹 버튼 눌렀을 때
    public void OnClickLevelGruop()
    {
        m_GameTitle.gameObject.SetActive(false);    // 게임 제목 없애기
        m_Level.SetActive(false);                   // 각 레벨 보여주던거 없애기

        m_LevelWindowBackGround.gameObject.SetActive(true); // 레벨 상세창 배경 활성화

        // 화살표 이미지 활성화 기본 설정
        m_PreviousArrowImage.sprite = m_ArrowOnSprite;
        m_NextArrowImage.sprite = m_ArrowOnSprite;

        // 어떤 레벨을 선택했는지 체크
        m_ClickLevel = EventSystem.current.currentSelectedGameObject;
        m_SelectLevelNumber = m_ClickLevel.name[0] - '0';

        // 선택한 레벨에 맞게 동그라미 표시 설정
        m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOnSprite;

        // 가장 왼쪽 단계(최저단계)라면 왼쪽으로 가는 화살표 끄기
        if (1 == m_SelectLevelNumber)
        {
            m_PreviousArrowImage.sprite = m_ArrowOffSprite;
        }
        // 가장 오른쪽 단계(최고단계)라면 오른쪽으로 가는 화살표 끄기
        else if (m_LevelWindowList.Count == m_SelectLevelNumber)
        {
            m_NextArrowImage.sprite = m_ArrowOffSprite;
        }
    }

    // 스테이지 바꾸는 화살표 버튼 눌렀을 때
    public void OnClickArrow()
    {
        // 이전으로 가는 화살표 선택 시
        if (m_PreviousArrowImage.gameObject == EventSystem.current.currentSelectedGameObject)
        {
            // 더이상 이전으로 갈 수 없으면 실행 안함
            if (1 == m_SelectLevelNumber)
                return;

            // 이전 정보들 비활성화
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOffSprite;

            // 현재 선택된 레벨 숫자 증가
            // 이와 맞는 정보들 활성화
            m_SelectLevelNumber--;
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(true);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOnSprite;

            // 화살표 설정 관련
            // 이전으로 갔다면 다음으로 가는 화살표는 무조건 켜져있어야 함
            m_NextArrowImage.sprite = m_ArrowOnSprite;

            // 가장 왼쪽 단계(최저단계)라면 왼쪽으로 가는 화살표 끄기
            if (1 == m_SelectLevelNumber)
            {
                m_PreviousArrowImage.sprite = m_ArrowOffSprite;
            }
        }
        // 다음으로 가는 화살표 선택 시
        else
        {
            // 더이상 다음으로 갈 수 없으면 실행 안함
            if (m_LevelWindowList.Count == m_SelectLevelNumber)
                return;

            // 이전 정보들 비활성화
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOffSprite;

            // 현재 선택된 레벨 숫자 증가
            // 이와 맞는 정보들 활성화
            m_SelectLevelNumber++;
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(true);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOnSprite;

            // 화살표 설정 관련
            // 다음으로 갔다면 이전으로 가는 화살표는 무조건 켜져있어야 함
            m_PreviousArrowImage.sprite = m_ArrowOnSprite;

            // 가장 오른쪽 단계(최고단계)라면 오른쪽으로 가는 화살표 끄기
            if (m_LevelWindowList.Count == m_SelectLevelNumber)
            {
                m_NextArrowImage.sprite = m_ArrowOffSprite;
            }
        }
    }

    // 레벨 나가기 버튼 눌렀을 때
    public void OnClickLevelExit()
    {
        // 현재 스테이지 정보 창이 켜져있다면 (스테이지 아이디 값에 변화가 있다면)
        if (StageID.NotActive != m_SelectStageID)
        {
            // 스테이지 아이디 기본값으로 초기화
            m_SelectStageID = StageID.NotActive;

            // 스테이지 리스트 창 활성화
            // 스테이지 정보 창 비활성화
            m_StageListWindow.SetActive(true);
            m_StageInfoWindow.SetActive(false);
        }
        // 스테이지 정보 창이 켜져있지 않다면
        else
        {
            // 스테이지 버튼 활성화
            m_StageButton.interactable = true;

            // 레벨 상세창 배경 비활성화 및 게임 제목 활성화
            m_LevelWindowBackGround.gameObject.SetActive(false);
            m_GameTitle.gameObject.SetActive(true);

            // 스테이지 리스트 창 비활성화
            // 스테이지 정보 창 비활성화
            m_StageListWindow.SetActive(false);
            m_StageInfoWindow.SetActive(false);

            // 선택했던 레벨 리스트 창, 동그라미 마크 비활성화
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOffSprite;
        }

        // 공통 사항 처리
        // 선택했던 스테이지에 맞게 정보 창 관련된 것들 비활성화
        m_LevelWindowInfo[m_SelectLevelNumber - 1].SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].gameObject.SetActive(false);

        // 레벨에 따른 제목 활성화
        // 맵 정보 활성화, 타겟 정보 비활성화
        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(false);
    }

    // 각각의 스테이지를 눌렀을 때
    public void OnClickEachStage()
    {
        // 선택한 스테이지 아이디 저장
        int stageid = int.Parse(EventSystem.current.currentSelectedGameObject.name.Substring(0, 4));
        m_SelectStageID = (StageID)stageid;

        // 선택한 스테이지 아이디 값을 스테이지 단계에 맞게 정수로 변환한 값으로 보관
        switch (((stageid % 100) / 10))
        {
            case 1:
                m_SelectStageIDValue = stageid % 10;
                break;
            case 2:
                m_SelectStageIDValue = stageid % 10 + m_Level1_StageCount;
                break;
            case 3:
                m_SelectStageIDValue = stageid % 10 + m_Level1_StageCount + m_Level2_StageCount; ;
                break;
            default:
                break;
        }

        // 스테이지 리스트 창 비활성화
        // 스테이지 정보 창 활성화
        m_StageListWindow.SetActive(false);
        m_StageInfoWindow.SetActive(true);

        // 선택한 스테이지에 맞게 정보 창 관련된 것들 활성화
        m_LevelWindowInfo[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].gameObject.SetActive(true);
    }

    // 맵 버튼을 눌렀을 때
    public void OnClickMap()
    {
        // 타겟 관련 정보 비활성화 한 후
        // 맵 관련 정보 활성화
        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(false);
    }

    // 타겟 버튼을 눌렀을 때
    public void OnClickTarget()
    {
        // 맵 관련 정보 비활성화 한 후
        // 타겟 관련 정보 활성화
        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(true);
    }

    // 게임시작 버튼을 눌렀을 때
    public void OnClickGameStart()
    {
        // 선택한 스테이지에 맞게 게임 시작
        SceneManager.LoadScene(m_SelectStageIDValue + 1);
    }

    // 설정 버튼 눌렀을 때
    public void OnClickOption()
    {

    }

    // 게임 종료 버튼 눌렀을 때
    public void OnClickGameExit()
    {

    }
}
