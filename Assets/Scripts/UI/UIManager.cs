using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �� �̹��� ���� ����
[Serializable]
public class RifleImageInfo
{
    // ��, �Ѿ� �̹���
    public Image RifleImage;
    public List<Image> BulletCount;
}

public class UIManager : MonoBehaviour
{
    // ��, �Ѿ� UI
    [SerializeField] private List<RifleImageInfo> m_RifleImageInfo;

    //[SerializeField] private Image m_RifleImage;
    //[SerializeField] private List<Image> m_BulletCount;

    // ��, �Ѿ� ��������Ʈ
    [SerializeField] private List<Sprite> m_RifleSprites;
    [SerializeField] private List<Sprite> m_BulletSprites;

    // ���� ������Ʈ��
    [SerializeField] private GameObject m_RestartToMessage; // ����� �޽���
    [SerializeField] private GameObject m_BlackScreen;      // ���� ȭ��
    [SerializeField] private GameObject m_ClearText;        // Ŭ���� �ؽ�Ʈ
    [SerializeField] private GameObject m_NextStageButton;  // ���� �������� �Ѿ�� ��ư
    [SerializeField] private GameObject m_GameExitButton;   // ���� ���� ��ư
    [SerializeField] private GameObject m_EscMessage;       // Esc ������ �� ������ �޽���
    [SerializeField] private GameObject m_HintMessage;      // ��Ʈ �޽���

    public bool m_MissionComplete = false;   // �������� Ŭ���� ���� �Ǵ�
    private bool m_OneChecking = true;       // ��� �ִϸ��̼��� �ѹ��� ������ �ϴ� ����

    // UI�Ŵ����� �ν��Ͻ�ȭ �� ��
    private static UIManager m_Instance;
    public static UIManager Instance => m_Instance;

    // �ٸ� �͵麸�� ���� ����ǵ��� Awake�� ����
    void Awake()
    {
        m_Instance = GetComponent<UIManager>();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        // EscŰ ������ �޽��� �߰� ������� �ϱ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_EscMessage.SetActive(!m_EscMessage.activeSelf);

            GameManager.Instance.IsGamePause = m_EscMessage.activeSelf;

            if (!GameManager.Instance.IsGamePause && TutorialManager.Instance != null && TutorialManager.Instance.IsTutorial)
            {
                GameManager.Instance.IsGamePause = true;
            }
                
            if (m_EscMessage.activeSelf)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
            
        }
    }

    // ��� �Ѿ� ���� ����
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

    // ���� ������� �Ѿ� ���� ����
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

    // �� �� �Ѿ� ��������Ʈ ����
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

    // �������� �޽���
    public void GameOverMessage()
    {
        StartCoroutine(GameOverAnim());
    }
    
    // ��������� ��Ȳ�� �°� ����
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
        if (TimerCheckDontDestroy.Instance != null)
        {
            TimerCheckDontDestroy.Instance.InitDatas();
        }

        SceneManager.LoadScene(GameManager.Instance.SceneNumber + 1);
    }

    // ���θ޴� �� �ε�
    public void MainMenuLoadScene()
    {
        SceneManager.LoadScene(0);
    }

    // ��Ʈ �޽��� �����ֱ�
    public void ShowHint()
    {
        m_HintMessage.SetActive(true);

        StartCoroutine(InitHintMessage());
    }

    public void CloseHint()
    {
        m_HintMessage.SetActive(false);
    }

    IEnumerator InitHintMessage()
    {
        yield return new WaitForSeconds(7f);

        CloseHint();
    }

    // Esc�޽������� Yes��ư ������ ��
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

    // Esc�޽������� No��ư ������ ��
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

    // ����� ��ư ������ ��
    public void OnClickRestartButton()
    {
        GameManager.Instance.RestartGame();
    }
}
