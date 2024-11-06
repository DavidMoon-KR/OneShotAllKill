using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// �� ���������� ���̵�
public enum StageID
{
    NotActive = 0, // ���� ���������� ���� �ʾ����� üũ // �⺻��

    // �⺻ ��� �ܰ�
    Level1_Stage1 = 1010,
    Level1_Stage2,
    Level1_Stage3,

    // ���� �ذ� �ɷ� �ܰ�
    Level2_Stage1 = 1020,
    Level2_Stage2,
    Level2_Stage3,

    // ������ ���� �ܰ�
    Level3_Stage1 = 1030,
    Level3_Stage2,
    Level3_Stage3,
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_GameTitle;   // ���� ����

    // �������� ����
    [Header ("< Stage >")]
    [SerializeField] private Button m_StageButton;  // �������� ��ư
    [SerializeField] private GameObject m_Level;    // ���� �׷�
    [SerializeField] private Image m_LevelWindowBackGround;  // ���� ��â ���

    // �������� ����Ʈ ����
    [Header("- StageList")]
    [SerializeField] private GameObject m_StageListWindow;          // �������� ����Ʈ â
    [SerializeField] private List<GameObject> m_LevelWindowList;    // ���� ����Ʈ â

    [SerializeField] private List<Image> m_LevelMark;   // ���� ��ũ
    [SerializeField] private Sprite m_MarkOnSprite;     // ���� ȭ��ǥ ��������Ʈ
    [SerializeField] private Sprite m_MarkOffSprite;    // ���� ȭ��ǥ ��������Ʈ

    [SerializeField] private Image m_PreviousArrowImage;    // ���� �������� ���� ȭ��ǥ UI
    [SerializeField] private Image m_NextArrowImage;        // ���� �������� ���� ȭ��ǥ UI
    [SerializeField] private Sprite m_ArrowOnSprite;        // ���� ȭ��ǥ ��������Ʈ
    [SerializeField] private Sprite m_ArrowOffSprite;       // ���� ȭ��ǥ ��������Ʈ

    // �������� ���� ����
    [Header("- StageInfo")]
    [SerializeField] private GameObject m_StageInfoWindow;          // �������� ���� â
    [SerializeField] private List<GameObject> m_LevelWindowInfo;    // ���� ���� â
    
    [SerializeField] private List<GameObject> m_StageWindowInfo;        // �������� ���� â
    [SerializeField] private List<GameObject> m_LevelWindowInfo_Title;  // ���� �� Ÿ��Ʋ ����

    private GameObject m_ClickLevel;        // Ŭ���� ����
    private int m_SelectLevelNumber = 0;    // Ŭ���� ���� ����

    private StageID m_SelectStageID = StageID.NotActive; // �������� ���̵�
    private int m_SelectStageIDValue = 0;                // �������� ���̵� �������� �ܰ迡 �°� ������ ��ȯ�� ��

    private const int m_Level1_StageCount = 3; // �⺻ ��� �ܰ� �������� ��
    private const int m_Level2_StageCount = 3; // ���� �ذ� �ɷ� �ܰ� �������� ��
    private const int m_Level3_StageCount = 3; // ������ ���� �ܰ� �������� ��

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickLevelExit();
        }
    }

    // �������� ��ư ������ ��
    public void OnClickStage()
    {
        m_Level.SetActive(true);            // �� ���� �����ֱ�
        m_StageButton.interactable = false; // ���̻� �������� ��ư �������� �������� ��ư ��Ȱ��ȭ
    }

    // ���� �׷� ��ư ������ ��
    public void OnClickLevelGruop()
    {
        m_GameTitle.gameObject.SetActive(false);    // ���� ���� ���ֱ�
        m_Level.SetActive(false);                   // �� ���� �����ִ��� ���ֱ�

        m_LevelWindowBackGround.gameObject.SetActive(true); // ���� ��â ��� Ȱ��ȭ

        // ȭ��ǥ �̹��� Ȱ��ȭ �⺻ ����
        m_PreviousArrowImage.sprite = m_ArrowOnSprite;
        m_NextArrowImage.sprite = m_ArrowOnSprite;

        // � ������ �����ߴ��� üũ
        m_ClickLevel = EventSystem.current.currentSelectedGameObject;
        m_SelectLevelNumber = m_ClickLevel.name[0] - '0';

        // ������ ������ �°� ���׶�� ǥ�� ����
        m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOnSprite;

        // ���� ���� �ܰ�(�����ܰ�)��� �������� ���� ȭ��ǥ ����
        if (1 == m_SelectLevelNumber)
        {
            m_PreviousArrowImage.sprite = m_ArrowOffSprite;
        }
        // ���� ������ �ܰ�(�ְ�ܰ�)��� ���������� ���� ȭ��ǥ ����
        else if (m_LevelWindowList.Count == m_SelectLevelNumber)
        {
            m_NextArrowImage.sprite = m_ArrowOffSprite;
        }
    }

    // �������� �ٲٴ� ȭ��ǥ ��ư ������ ��
    public void OnClickArrow()
    {
        // �������� ���� ȭ��ǥ ���� ��
        if (m_PreviousArrowImage.gameObject == EventSystem.current.currentSelectedGameObject)
        {
            // ���̻� �������� �� �� ������ ���� ����
            if (1 == m_SelectLevelNumber)
                return;

            // ���� ������ ��Ȱ��ȭ
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOffSprite;

            // ���� ���õ� ���� ���� ����
            // �̿� �´� ������ Ȱ��ȭ
            m_SelectLevelNumber--;
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(true);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOnSprite;

            // ȭ��ǥ ���� ����
            // �������� ���ٸ� �������� ���� ȭ��ǥ�� ������ �����־�� ��
            m_NextArrowImage.sprite = m_ArrowOnSprite;

            // ���� ���� �ܰ�(�����ܰ�)��� �������� ���� ȭ��ǥ ����
            if (1 == m_SelectLevelNumber)
            {
                m_PreviousArrowImage.sprite = m_ArrowOffSprite;
            }
        }
        // �������� ���� ȭ��ǥ ���� ��
        else
        {
            // ���̻� �������� �� �� ������ ���� ����
            if (m_LevelWindowList.Count == m_SelectLevelNumber)
                return;

            // ���� ������ ��Ȱ��ȭ
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOffSprite;

            // ���� ���õ� ���� ���� ����
            // �̿� �´� ������ Ȱ��ȭ
            m_SelectLevelNumber++;
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(true);
            m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOnSprite;

            // ȭ��ǥ ���� ����
            // �������� ���ٸ� �������� ���� ȭ��ǥ�� ������ �����־�� ��
            m_PreviousArrowImage.sprite = m_ArrowOnSprite;

            // ���� ������ �ܰ�(�ְ�ܰ�)��� ���������� ���� ȭ��ǥ ����
            if (m_LevelWindowList.Count == m_SelectLevelNumber)
            {
                m_NextArrowImage.sprite = m_ArrowOffSprite;
            }
        }
    }

    // ���� ������ ��ư ������ ��
    public void OnClickLevelExit()
    {
        // ���� �������� ���� â�� �����ִٸ� (�������� ���̵� ���� ��ȭ�� �ִٸ�)
        if (StageID.NotActive != m_SelectStageID)
        {
            // �������� ���̵� �⺻������ �ʱ�ȭ
            m_SelectStageID = StageID.NotActive;

            // �������� ����Ʈ â Ȱ��ȭ
            // �������� ���� â ��Ȱ��ȭ
            m_StageListWindow.SetActive(true);
            m_StageInfoWindow.SetActive(false);
        }
        // �������� ���� â�� �������� �ʴٸ�
        else
        {
            // �������� ��ư Ȱ��ȭ
            m_StageButton.interactable = true;
            m_Level.SetActive(false); // �� ������ư ��Ȱ��ȭ

            // ���� ��â ��� ��Ȱ��ȭ �� ���� ���� Ȱ��ȭ
            m_LevelWindowBackGround.gameObject.SetActive(false);
            m_GameTitle.gameObject.SetActive(true);

            // �������� ����Ʈ â ��Ȱ��ȭ
            // �������� ���� â ��Ȱ��ȭ
            m_StageListWindow.SetActive(false);
            m_StageInfoWindow.SetActive(false);

            // Esc��ư�� ���� ���ù�ư���� ���� ��쿣 ���� ������ ������ ����. ���� ����ó��
            if (0 != m_SelectLevelNumber)
            {
                // �����ߴ� ���� ����Ʈ â, ���׶�� ��ũ ��Ȱ��ȭ
                m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
                m_LevelMark[m_SelectLevelNumber - 1].sprite = m_MarkOffSprite;
            }            
        }
        // Esc��ư�� ���� ���ù�ư���� ���� ��쿣 ���� ������ ������ ����. ���� ����ó��
        if (0 != m_SelectLevelNumber)
        {
            // ���� ���� ó��
            // �����ߴ� ���������� �°� ���� â ���õ� �͵� ��Ȱ��ȭ
            m_LevelWindowInfo[m_SelectLevelNumber - 1].SetActive(false);
            m_StageWindowInfo[m_SelectStageIDValue].gameObject.SetActive(false);

            // ������ ���� ���� Ȱ��ȭ
            // �� ���� Ȱ��ȭ, Ÿ�� ���� ��Ȱ��ȭ
            m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(true);
            m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(true);
            m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(false);
        }
    }

    // ������ ���������� ������ ��
    public void OnClickEachStage()
    {
        // ������ �������� ���̵� ����
        int stageid = int.Parse(EventSystem.current.currentSelectedGameObject.name.Substring(0, 4));
        m_SelectStageID = (StageID)stageid;

        // ������ �������� ���̵� ���� �������� �ܰ迡 �°� ������ ��ȯ�� ������ ����
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

        // �������� ����Ʈ â ��Ȱ��ȭ
        // �������� ���� â Ȱ��ȭ
        m_StageListWindow.SetActive(false);
        m_StageInfoWindow.SetActive(true);

        // ������ ���������� �°� ���� â ���õ� �͵� Ȱ��ȭ
        m_LevelWindowInfo[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].gameObject.SetActive(true);
    }

    // �� ��ư�� ������ ��
    public void OnClickMap()
    {
        // Ÿ�� ���� ���� ��Ȱ��ȭ �� ��
        // �� ���� ���� Ȱ��ȭ
        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(false);
    }

    // Ÿ�� ��ư�� ������ ��
    public void OnClickTarget()
    {
        // �� ���� ���� ��Ȱ��ȭ �� ��
        // Ÿ�� ���� ���� Ȱ��ȭ
        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(true);
    }

    // ���ӽ��� ��ư�� ������ ��
    public void OnClickGameStart()
    {
        // ������ ���������� �°� ���� ����
        SceneManager.LoadScene(m_SelectStageIDValue + 1);
    }

    // ���� ��ư ������ ��
    public void OnClickOption()
    {

    }

    // ���� ���� ��ư ������ ��
    public void OnClickGameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ������ ����
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
}
