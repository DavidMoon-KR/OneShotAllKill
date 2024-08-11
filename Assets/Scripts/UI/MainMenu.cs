using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum StageID
{
    _3rd_Tutorial = 1010,
    _3rd_Stage1,
    _3rd_Stage2,

    _2rd_Tutorial = 1020,
    _2rd_Stage1,
    _2rd_Stage2,

    _1rd_Tutorial = 1030,
    _1rd_Stage1,
    _1rd_Stage2,

}

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_GameTitle; // ���� ����
    [SerializeField]
    private GameObject m_Level; // ���� �׷�
    [SerializeField]
    private List<GameObject> m_LevelWindowList = new List<GameObject>(); // ���� ����Ʈ â
    [SerializeField]
    private List<GameObject> m_LevelWindowInfo = new List<GameObject>(); // ���� ���� â
    [SerializeField]
    private List<GameObject> m_StageWindowInfo = new List<GameObject>(); // �������� ���� â
    [SerializeField]
    private List<GameObject> m_LevelWindowInfo_Title = new List<GameObject>(); // ���� �� Ÿ��Ʋ ����

    [SerializeField]
    private Image m_LevelWindowBackGround; // ���� â ���
    [SerializeField]
    private List<Image> m_LevelMarkOn; // Ȱ��ȭ ���� ��ũ
    [SerializeField]
    private List<Image> m_LevelMarkOff; // ��Ȱ��ȭ ���� ��ũ

    // ȭ��ǥ ���� �̹��� �� ��������Ʈ
    [SerializeField]
    private Image m_PreviousArrowImage;
    [SerializeField]
    private Image m_NextArrowImage;
    [SerializeField]
    private Sprite m_ArrowOnSprite;
    [SerializeField]
    private Sprite m_ArrowOffSprite;

    private GameObject m_ClickLevel; // Ŭ���� ����
    private int m_SelectLevelNumber;   // Ŭ���� ���� ����

    private StageID m_SelectStageID;
    private int m_SelectStageIDValue = 0;
    private const int m_3rd_StageCount = 3;
    private const int m_2nd_StageCount = 3;
    private const int m_1st_StageCount = 3;

    // �������� ��ư ������ ��
    public void OnClickStage()
    {
        m_Level.SetActive(true);
    }

    // ���� �׷� ��ư ������ ��
    public void OnClickLevelGruop()
    {
        m_GameTitle.gameObject.SetActive(false);
        m_Level.SetActive(false);

        m_LevelWindowBackGround.gameObject.SetActive(true);

        m_PreviousArrowImage.sprite = m_ArrowOnSprite;
        m_NextArrowImage.sprite = m_ArrowOnSprite;

        m_ClickLevel = EventSystem.current.currentSelectedGameObject;
        m_SelectLevelNumber = m_ClickLevel.name[0] - '0';

        m_LevelMarkOn[m_SelectLevelNumber - 1].gameObject.SetActive(true);
        m_LevelMarkOff[m_SelectLevelNumber - 1].gameObject.SetActive(false);

        // ���� ���� �ܰ�(�ְ�ܰ�)��� �������� ���� ȭ��ǥ ����
        if (m_LevelWindowList.Count == m_SelectLevelNumber)
        {
            m_PreviousArrowImage.sprite = m_ArrowOffSprite;
        }
        // ����������� �ܰ�(�����ܰ�)��� ���������� ���� ȭ��ǥ ����
        else if (1 == m_SelectLevelNumber)
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
            if (m_LevelWindowList.Count == m_SelectLevelNumber)
                return;

            // ���� ������ ��Ȱ��ȭ
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
            m_LevelMarkOn[m_SelectLevelNumber - 1].gameObject.SetActive(false);
            m_LevelMarkOff[m_SelectLevelNumber - 1].gameObject.SetActive(true);

            // ���� ���õ� ���� ���� ����
            // �̿� �´� ������ Ȱ��ȭ
            m_SelectLevelNumber++;
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(true);
            m_LevelMarkOn[m_SelectLevelNumber - 1].gameObject.SetActive(true);
            m_LevelMarkOff[m_SelectLevelNumber - 1].gameObject.SetActive(false);

            // ȭ��ǥ ���� ����
            // �������� ���ٸ� �������� ���� ȭ��ǥ�� ������ �����־�� ��
            m_NextArrowImage.sprite = m_ArrowOnSprite;

            // ���� ���� �ܰ�(�ְ�ܰ�)��� �������� ���� ȭ��ǥ ����
            if (m_LevelWindowList.Count == m_SelectLevelNumber)
            {
                m_PreviousArrowImage.sprite = m_ArrowOffSprite;
            }
        }
        // �������� ���� ȭ��ǥ ���� ��
        else
        {
            // ���̻� �������� �� �� ������ ���� ����
            if (1 == m_SelectLevelNumber)
                return;

            // ���� ������ ��Ȱ��ȭ
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
            m_LevelMarkOn[m_SelectLevelNumber - 1].gameObject.SetActive(false);
            m_LevelMarkOff[m_SelectLevelNumber - 1].gameObject.SetActive(true);

            // ���� ���õ� ���� ���� ����
            // �̿� �´� ������ Ȱ��ȭ
            m_SelectLevelNumber--;
            m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(true);
            m_LevelMarkOn[m_SelectLevelNumber - 1].gameObject.SetActive(true);
            m_LevelMarkOff[m_SelectLevelNumber - 1].gameObject.SetActive(false);

            // ȭ��ǥ ���� ����
            // �������� ���ٸ� �������� ���� ȭ��ǥ�� ������ �����־�� ��
            m_PreviousArrowImage.sprite = m_ArrowOnSprite;

            // ����������� �ܰ�(�����ܰ�)��� ���������� ���� ȭ��ǥ ����
            if (1 == m_SelectLevelNumber)
            {
                m_NextArrowImage.sprite = m_ArrowOffSprite;
            }
        }
    }

    // ���� ������ ��ư ������ ��
    public void OnClickLevelExit()
    {
        m_LevelWindowBackGround.gameObject.SetActive(false);
        m_GameTitle.gameObject.SetActive(true);

        m_LevelWindowList[m_SelectLevelNumber - 1].SetActive(false);
        m_LevelMarkOn[m_SelectLevelNumber - 1].gameObject.SetActive(false);
        m_LevelMarkOff[m_SelectLevelNumber - 1].gameObject.SetActive(true);

        m_LevelWindowInfo[m_SelectLevelNumber - 1].SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].gameObject.SetActive(false);

        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(false);
    }

    // ������ ���������� ������ ��
    public void OnClickEachStage()
    {
        int stageid = int.Parse(EventSystem.current.currentSelectedGameObject.name.Substring(0, 4));
        m_SelectStageID = (StageID)stageid;

        switch (((stageid % 100) / 10))
        {
            case 1:
                m_SelectStageIDValue = stageid % 10;
                break;
            case 2:
                m_SelectStageIDValue = stageid % 10 + m_3rd_StageCount;
                break;
            case 3:
                m_SelectStageIDValue = stageid % 10 +m_3rd_StageCount + m_2nd_StageCount; ;
                break;
            default:
                break;
        }

        m_LevelWindowInfo[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].gameObject.SetActive(true);
    }

    // �� ��ư�� ������ ��
    public void OnClickMap()
    {
        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(true);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(false);
    }

    // Ÿ�� ��ư�� ������ ��
    public void OnClickTarget()
    {
        m_LevelWindowInfo_Title[m_SelectLevelNumber - 1].SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("MapInfo").gameObject.SetActive(false);
        m_StageWindowInfo[m_SelectStageIDValue].transform.Find("TargetInfo").gameObject.SetActive(true);
    }

    // ���� ��ư ������ ��
    public void OnClickOption()
    {

    }

    // ���� ���� ��ư ������ ��
    public void OnClickGameExit()
    {

    }
}
