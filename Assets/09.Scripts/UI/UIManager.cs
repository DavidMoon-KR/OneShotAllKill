using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private List<GameObject> m_BeforeStartActiveFalseObjs;

    // ��, �Ѿ� UI
    [SerializeField] private List<RifleImageInfo> m_RifleImageInfo;

    //[SerializeField] private Image m_RifleImage;
    //[SerializeField] private List<Image> m_BulletCount;

    // ��, �Ѿ� ��������Ʈ
    [SerializeField] private List<Sprite> m_RifleSprites;
    [SerializeField] private List<Sprite> m_BulletSprites;

    // ���� ������Ʈ��
    [SerializeField] private GameObject m_RestartToMessage; // ����� �޽���
    [SerializeField] private GameObject m_ClearBackGround;  // Ŭ���� ȭ�� �⺻ ���
    [SerializeField] private GameObject m_BlackScreen;      // ���� ȭ��
    [SerializeField] private GameObject m_NotFinalStageUI;  // ������ ���������� �ƴ� ��� UI
    [SerializeField] private GameObject m_FinalStageUI;     // ������ ���������� ��� UI
    [SerializeField] private GameObject m_ClearText;        // Clear ����
    [SerializeField] private GameObject m_VictoryText;      // Victory ����
    [SerializeField] private GameObject m_EscMessage;       // Esc ������ �� ������ �޽���

    [SerializeField] private Button m_HintBtn;          // ��Ʈ ��ư
    [SerializeField] private GameObject m_HintNotification; // ��Ʈ �˸�
    [SerializeField] private GameObject m_HintMessage;              // ��Ʈ �޽���
    [SerializeField] private TextMeshProUGUI m_HintRemaningCount;   // ���� ��Ʈ ����

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
            PressEscBtn();
        }
    }

    // Esc ��ư ������ ��
    public void PressEscBtn()
    {
        EventSystem.current.SetSelectedGameObject(null);

        // Esc �޽��� Ȱ��ȭ �� ���� �Ͻ����� ���¸� �ݴ�� ����
        m_EscMessage.SetActive(!m_EscMessage.activeSelf);
        GameManager.Instance.IsGamePause = m_EscMessage.activeSelf;

        // ���� �Ͻ������� �Ǿ����� �ʰ�, Ʃ�丮���� �������̶��
        // ���� �Ͻ�����
        if (!GameManager.Instance.IsGamePause && TutorialManager.Instance != null && TutorialManager.Instance.IsTutorial)
        {
            GameManager.Instance.IsGamePause = true;
        }

        // Esc�޽����� Ȱ��ȭ ���ο� ���� ���� �ð� ���� �Ǵ� ���
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
            // ��Ʈ�� ���� Ȱ��ȭ ����
            // ���� ����� �������� ����� ����
            if (m_BeforeStartActiveFalseObjs[i].name == "HintUI")
            {
                continue;
            }

            m_BeforeStartActiveFalseObjs[i].SetActive(true);
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

    // ���� ���������� �̵�
    public void NextStageLoadScene()
    {
        SceneManager.LoadScene(GameManager.Instance.SceneNumber + 1);
        if(HintManager.Instance != null)
        {
            HintManager.Instance.StageChange();
        }
        UIBeforeStart.Instance.IsActiveStageMessage = true;
        SoundManager.Instance.DestroySound();
    }

    // ���θ޴� �� �ε�
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

    // ��Ʈ ��ư �����ֱ�
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

    // ��Ʈ �޽��� �����ֱ�
    public void ShowHintMessage()
    {
        m_HintMessage.SetActive(true);
        //m_HintRemaningCount.text = $"���� ��Ʈ ���� : {HintManager.Instance.RemaningHintCount}";
    }

    // ��Ʈ �޽��� �ݱ�
    public void CloseHintMessage()
    {
        m_HintMessage.SetActive(false);
    }

    // ��Ʈ �����ֱ�
    public void ShowHint()
    {
        m_HintMessage.SetActive(false);
        HintManager.Instance.ActiveHint();

        // ��Ʈ ��ư ��Ȱ��ȭ
        m_HintBtn.interactable = false;
    }

    // ��Ʈ �Ϸ�
    public void CompleteHint()
    {
        // ��ư �ٽ� Ȱ��ȭ
        m_HintBtn.interactable = true;
    }

    // ��� ��Ʈ �Ϸ�
    public void CompleteAllHint()
    {
        // ��ư �� �޽��� ��Ȱ��ȭ
        m_HintBtn.gameObject.SetActive(false);
        m_HintNotification.SetActive(false);
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
