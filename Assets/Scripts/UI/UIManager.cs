using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 총, 총알 UI
    [SerializeField] private Image m_RifleImage;
    [SerializeField] private List<Image> m_BulletCount;

    // 총, 총알 스프라이트
    [SerializeField] private List<Sprite> m_RifleSprites;
    [SerializeField] private List<Sprite> m_BulletSprites;

    // 각종 오브젝트들
    [SerializeField] private GameObject m_RestartToMessage; // 재시작 메시지
    [SerializeField] private GameObject m_BlackScreen;      // 검은 화면
    [SerializeField] private GameObject m_ClearText;        // 클리어 텍스트
    [SerializeField] private GameObject m_NextStageButton;  // 다음 스테이지 넘어가는 버튼
    [SerializeField] private GameObject m_GameExitButton;   // 게임 종료 버튼
    [SerializeField] private GameObject m_EscMessage;       // Esc 눌렀을 떄 나오는 메시지

    public bool m_MissionComplete = false;   // 스테이지 클리어 여부 판단
    private bool m_OneChecking = true;       // 결과 애니메이션이 한번만 나오게 하는 변수

    // UI매니저를 인스턴스화 한 것
    private static UIManager m_Instance;
    public static UIManager Instance => m_Instance;

    // 다른 것들보다 먼저 실행되도록 Awake로 실행
    void Awake()
    {
        m_Instance = GetComponent<UIManager>();
    }

    void Update()
    {
        // Esc키 누르면 메시지 뜨고 사라지게 하기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_EscMessage.SetActive(!m_EscMessage.activeSelf);

            GameManager.Instance.IsGamePause = m_EscMessage.activeSelf;
            Time.timeScale = 1.0f - Time.timeScale;
        }
    }

    // 탄 개수 표시
    public void BulletCountSet(int p_bullet)
    {
        for (int i = 0; i < m_BulletCount.Count; i++)
        {
            if (p_bullet < (i + 1))
            {
                m_BulletCount[i].gameObject.SetActive(false);
                continue;
            }
            m_BulletCount[i].gameObject.SetActive(true);
        }
    }

    // 총 및 총알 스프라이트 변경
    public void BulletSpriteChange(BulletType p_bullettype)
    {
        m_RifleImage.sprite = m_RifleSprites[(int)p_bullettype];

        for (int i = 0; i < m_BulletCount.Count; i++)
        {
            m_BulletCount[i].sprite = m_BulletSprites[(int)p_bullettype];
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
            m_ClearText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            m_NextStageButton.SetActive(true);
            m_GameExitButton.SetActive(true);
            m_OneChecking = false;
        }
    }

    // 다음 스테이지로 이동
    public void NextStageLoadScene()
    {
        SceneManager.LoadScene(GameManager.Instance.SceneNumber + 1);
    }

    // 메인메뉴 씬 로드
    public void MainMenuLoadScene()
    {
        SceneManager.LoadScene(0);
    }

    // Esc메시지에서 Yes버튼 눌렀을 때
    public void OnclickEscMessageYes()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsGamePause = false;
        MainMenuLoadScene();
    }

    // Esc메시지에서 No버튼 눌렀을 때
    public void OnclickEscMessageNo()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsGamePause = false;
        m_EscMessage.SetActive(false);
    }

    // 재시작 버튼 눌렀을 때
    public void OnClickRestartButton()
    {
        GameManager.Instance.RestartGame();
    }
}
