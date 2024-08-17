using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<Image> m_BulletCount;
    [SerializeField]
    private GameObject m_RestartToMessage;
    [SerializeField]
    private GameObject m_BlackScreen;
    [SerializeField]
    private GameObject m_ClearText;
    [SerializeField]
    private GameObject m_NextStageButton;
    [SerializeField]
    private GameObject m_GameExitButton;
    [SerializeField]
    private GameObject m_EscMessage;

    public bool m_MissionComplete = false;   // 스테이지 클리어 여부 판단
    private bool m_OneChecking = true;       // 결과 애니메이션이 한번만 나오게 하는 변수

    private static UIManager m_Instance;
    public static UIManager Instance => m_Instance;

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

    public void GameOverMessage()
    {
        StartCoroutine(GameOverAnim());
    }

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
        SceneManager.LoadScene(GameManager.Instance.m_SceneNumber + 1);
    }

    // 메인메뉴 씬 로드
    public void MainMenuLoadScene()
    {
        SceneManager.LoadScene(0);
    }

    // Esc메시지에서 Yes버튼 눌렀을 때
    public void EscMessageYes()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsGamePause = false;
        MainMenuLoadScene();
    }

    // Esc메시지에서 No버튼 눌렀을 때
    public void EscMessageNo()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsGamePause = false;
        m_EscMessage.SetActive(false);
    }
}
