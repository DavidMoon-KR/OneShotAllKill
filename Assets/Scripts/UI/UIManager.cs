using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text m_BulletCount;
    [SerializeField]
    private GameObject m_RestartToMessage;
    [SerializeField]
    private GameObject m_BlackScreen;
    [SerializeField]
    private GameObject m_HightlightedNext;
    [SerializeField]
    private GameObject m_HightlightedRestart;

    public bool m_MissionComplete = false;   // 스테이지 클리어 여부 판단
    private bool m_OneChecking = true;       // 결과 애니메이션이 한번만 나오게 하는 변수

    private static UIManager m_Instance;
    public static UIManager Instance => m_Instance;

    void Awake()
    {
        m_Instance = GetComponent<UIManager>();
    }

    // 탄 개수 표시
    public void BulletCountSet(int p_bullet)
    {
        m_BulletCount.text = p_bullet.ToString();
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
            m_HightlightedNext.SetActive(true);
            m_HightlightedRestart.SetActive(true);
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
}
