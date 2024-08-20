using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image m_RifleImage;
    [SerializeField]
    private List<Image> m_BulletCount;

    [SerializeField]
    private List<Sprite> m_RifleSprites;
    [SerializeField]
    private List<Sprite> m_BulletSprites;

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

    public bool m_MissionComplete = false;   // �������� Ŭ���� ���� �Ǵ�
    private bool m_OneChecking = true;       // ��� �ִϸ��̼��� �ѹ��� ������ �ϴ� ����

    private static UIManager m_Instance;
    public static UIManager Instance => m_Instance;

    void Awake()
    {
        m_Instance = GetComponent<UIManager>();
    }

    void Update()
    {
        // EscŰ ������ �޽��� �߰� ������� �ϱ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_EscMessage.SetActive(!m_EscMessage.activeSelf);

            GameManager.Instance.IsGamePause = m_EscMessage.activeSelf;
            Time.timeScale = 1.0f - Time.timeScale;
        }
    }

    // ź ���� ǥ��
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

    public void BulletSpriteChange(BulletType p_bullettype)
    {
        m_RifleImage.sprite = m_RifleSprites[(int)p_bullettype];

        for (int i = 0; i < m_BulletCount.Count; i++)
        {
            m_BulletCount[i].sprite = m_BulletSprites[(int)p_bullettype];
        }
    }

    public void GameOverMessage()
    {
        StartCoroutine(GameOverAnim());
    }

    private IEnumerator GameOverAnim()
    {
        // �������� Ŭ���� ���� ���
        if (m_MissionComplete == false && m_OneChecking == true)
        {
            m_RestartToMessage.gameObject.SetActive(true);
            m_OneChecking = false;
        }
        // �������� Ŭ���� �� ���
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

    // ���� ���������� �̵�
    public void NextStageLoadScene()
    {
        SceneManager.LoadScene(GameManager.Instance.SceneNumber + 1);
    }

    // ���θ޴� �� �ε�
    public void MainMenuLoadScene()
    {
        SceneManager.LoadScene(0);
    }

    // Esc�޽������� Yes��ư ������ ��
    public void OnclickEscMessageYes()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsGamePause = false;
        MainMenuLoadScene();
    }

    // Esc�޽������� No��ư ������ ��
    public void OnclickEscMessageNo()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsGamePause = false;
        m_EscMessage.SetActive(false);
    }

    public void OnClickRestartButton()
    {
        GameManager.Instance.RestartGame();
    }
}