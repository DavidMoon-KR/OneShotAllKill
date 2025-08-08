using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 총 이미지 관련 정보
[Serializable]
public class RifleImageInfo
{
    // 총, 총알 이미지
    public Image RifleImage;
    public List<Image> BulletCount;
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_BeforeStartActiveFalseObjs;

    // 총, 총알 UI
    [SerializeField] private List<RifleImageInfo> m_RifleImageInfo;

    //[SerializeField] private Image m_RifleImage;
    //[SerializeField] private List<Image> m_BulletCount;

    // 총, 총알 스프라이트
    [SerializeField] private List<Sprite> m_RifleSprites;
    [SerializeField] private List<Sprite> m_BulletSprites;

    // 각종 오브젝트들
    [SerializeField] private GameObject m_RestartToMessage; // 재시작 메시지
    [SerializeField] private GameObject m_ClearBackGround;  // 클리어 화면 기본 배경
    [SerializeField] private GameObject m_BlackScreen;      // 검은 화면
    [SerializeField] private GameObject m_NotFinalStageUI;  // 마지막 스테이지가 아닐 경우 UI
    [SerializeField] private GameObject m_FinalStageUI;     // 마지막 스테이지일 경우 UI
    [SerializeField] private GameObject m_ClearText;        // Clear 글자
    [SerializeField] private GameObject m_VictoryText;      // Victory 글자
    [SerializeField] private GameObject m_EscMessage;       // Esc 눌렀을 �� 나오는 메시지

    [SerializeField] private Button m_HintBtn;          // 힌트 버튼
    [SerializeField] private GameObject m_HintNotification; // 힌트 알림
    [SerializeField] private GameObject m_HintMessage;              // 힌트 메시지
    [SerializeField] private TextMeshProUGUI m_HintRemaningCount;   // 남은 힌트 개수

    public bool m_MissionComplete = false;   // 스테이지 클리어 여부 판단
    private bool m_OneChecking = true;       // 결과 애니메이션이 한번만 나오게 하는 변수

    // UI매니저를 인스턴스화 한 것
    private static UIManager m_Instance;
    public static UIManager Instance => m_Instance;

    // 다른 것들보다 먼저 실행되도록 Awake로 실행
    void Awake()
    {
        m_Instance = GetComponent<UIManager>();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        // Esc키 누르면 메시지 뜨고 사라지게 하기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PressEscBtn();
        }
    }

    // Esc 버튼 눌렀을 때
    public void PressEscBtn()
    {
        EventSystem.current.SetSelectedGameObject(null);

        // Esc 메시지 활성화 및 게임 일시정지 상태를 반대로 설정
        m_EscMessage.SetActive(!m_EscMessage.activeSelf);
        GameManager.Instance.IsGamePause = m_EscMessage.activeSelf;

        // 현재 일시정지가 되어있지 않고, 튜토리얼이 진행중이라면
        // 게임 일시정지
        if (!GameManager.Instance.IsGamePause && TutorialManager.Instance != null && TutorialManager.Instance.IsTutorial)
        {
            GameManager.Instance.IsGamePause = true;
        }

        // Esc메시지의 활성화 여부에 따라 게임 시간 정지 또는 재생
        if (m_EscMessage.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void Removeblur()
    {
        for (int i = 0; i < m_BeforeStartActiveFalseObjs.Count; i++)
        {
            // 힌트는 아직 활성화 안함
            // 이후 모바일 버전에서 사용할 예정
            if (m_BeforeStartActiveFalseObjs[i].name == "HintUI")
            {
                continue;
            }

            m_BeforeStartActiveFalseObjs[i].SetActive(true);
        }
    }

    // 모든 총알 개수 설정
    public void AllBulletCountSet(BulletType p_SelectType, List<int> p_bullet)
    {
        for (int i = 0; i < m_RifleImageInfo.Count; i++)
        {
            if ((int)p_SelectType >= m_RifleImageInfo.Count)
            {
                p_SelectType = BulletType.BulletType_Normal;
            }

            for (int j = 0; j < m_RifleImageInfo[(int)p_SelectType].BulletCount.Count; j++)
            {
                if (p_bullet[i] < (j + 1))
                {
                    m_RifleImageInfo[(int)p_SelectType].BulletCount[j].gameObject.SetActive(false);
                    continue;
                }
                m_RifleImageInfo[(int)p_SelectType].BulletCount[j].gameObject.SetActive(true);
            }

            p_SelectType++;
        }
    }

    // 현재 사용중인 총알 개수 설정
    public void BulletCountSet(int p_bullet)
    {
        for (int i = 0; i < m_RifleImageInfo[0].BulletCount.Count; i++)
        {
            if (p_bullet < (i + 1))
            {
                m_RifleImageInfo[0].BulletCount[i].gameObject.SetActive(false);
                continue;
            }
            m_RifleImageInfo[0].BulletCount[i].gameObject.SetActive(true);
        }
    }

    // 총 및 총알 스프라이트 변경
    public void RifleInfoSpriteChange(BulletType p_SelectType, List<int> p_bullet)
    {
        AllBulletCountSet(p_SelectType, p_bullet);

        for (int i = 0; i < m_RifleImageInfo.Count; i++)
        {
            if ((int)p_SelectType >= m_RifleImageInfo.Count)
            {
                p_SelectType = BulletType.BulletType_Normal;
            }

            m_RifleImageInfo[i].RifleImage.sprite = m_RifleSprites[(int)p_SelectType];

            for (int j = 0; j < m_RifleImageInfo[i].BulletCount.Count; j++)
            {
                m_RifleImageInfo[i].BulletCount[j].sprite = m_BulletSprites[(int)p_SelectType];
            }

            p_SelectType++;
        }
        
    }

    // 게임종료 메시지
    public void GameOverMessage()
    {
        StartCoroutine(GameOverAnim());
    }
    
    // 게임종료시 상황에 맞게 실행
    private IEnumerator GameOverAnim()
    {
        // 스테이지 클리어 못할 경우
        if (m_MissionComplete == false && m_OneChecking == true)
        {
            m_RestartToMessage.gameObject.SetActive(true);
            m_OneChecking = false;
        }
        // 스테이지 클리어 한 경우
        else if (m_MissionComplete == true && m_OneChecking == true)
        {
            m_BlackScreen.SetActive(true);
            yield return new WaitForSeconds(2.3f);
            m_ClearBackGround.SetActive(true);
            if (GameManager.Instance.SceneNumber != (int)StageID.Stage_End)
            {
                m_ClearText.SetActive(true);
            }
            else
            {
                m_VictoryText.SetActive(true);
            }            
            yield return new WaitForSeconds(0.5f);
            if (GameManager.Instance.SceneNumber != (int)StageID.Stage_End)
            {
                m_NotFinalStageUI.SetActive(true);
            }
            else
            {
                m_FinalStageUI.SetActive(true);
            }
            m_OneChecking = false;
        }
    }

    // 다음 스테이지로 이동
    public void NextStageLoadScene()
    {
        // 엑스트라 스테이지일 경우 단순 산술 대신 다음 씬 넘버를 수동으로 참조...
        var nextSceneNumber = GameManager.Instance.isExtraStage
            ? GameManager.Instance.nextExtraStageSceneNumber
            : GameManager.Instance.SceneNumber + 1;

        SceneManager.LoadScene(nextSceneNumber);
        if (HintManager.Instance != null)
        {
            HintManager.Instance.StageChange();
        }
        UIBeforeStart.Instance.IsActiveStageMessage = true;
        SoundManager.Instance.DestroySound();
    }

    // 메인메뉴 씬 로드
    public void MainMenuLoadScene()
    {
        UIBeforeStart.Instance.IsActiveStageMessage = true;
        if (HintManager.Instance != null)
        {
            HintManager.Instance.ExitGame();
        }        
        SceneManager.LoadScene(0);
        SoundManager.Instance.DestroySound();
    }

    // 힌트 버튼 보여주기
    public void ShowHintBtn()
    {
        m_HintBtn.gameObject.SetActive(true);
        m_HintNotification.SetActive(true);

        Invoke("HideHintNotification", 5f);
    }

    private void HideHintNotification()
    {
        m_HintNotification.SetActive(false);
    }

    // 힌트 메시지 보여주기
    public void ShowHintMessage()
    {
        m_HintMessage.SetActive(true);
        //m_HintRemaningCount.text = $"남은 힌트 개수 : {HintManager.Instance.RemaningHintCount}";
    }

    // 힌트 메시지 닫기
    public void CloseHintMessage()
    {
        m_HintMessage.SetActive(false);
    }

    // 힌트 보여주기
    public void ShowHint()
    {
        m_HintMessage.SetActive(false);
        HintManager.Instance.ActiveHint();

        // 힌트 버튼 비활성화
        m_HintBtn.interactable = false;
    }

    // 힌트 완료
    public void CompleteHint()
    {
        // 버튼 다시 활성화
        m_HintBtn.interactable = true;
    }

    // 모든 힌트 완료
    public void CompleteAllHint()
    {
        // 버튼 및 메시지 비활성화
        m_HintBtn.gameObject.SetActive(false);
        m_HintNotification.SetActive(false);
    }

    // Esc메시지에서 Yes버튼 눌렀을 때
    public void OnclickEscMessageYes()
    {
        Time.timeScale = 1.0f;
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsTutorial)
        {
            GameManager.Instance.IsGamePause = true;
        }
        else
        {
            GameManager.Instance.IsGamePause = false;
        }
        
        MainMenuLoadScene();
    }

    // Esc메시지에서 No버튼 눌렀을 때
    public void OnclickEscMessageNo()
    {
        Time.timeScale = 1.0f;
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsTutorial)
        {
            GameManager.Instance.IsGamePause = true;
        }
        else
        {
            GameManager.Instance.IsGamePause = false;
        }
        m_EscMessage.SetActive(false);
    }

    // 재시작 버튼 눌렀을 때
    public void OnClickRestartButton()
    {
        GameManager.Instance.RestartGame();
    }
}
