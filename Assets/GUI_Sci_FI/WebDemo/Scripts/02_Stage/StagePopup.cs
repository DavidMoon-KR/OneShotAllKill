using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;

public class StagePopup : MonoBehaviour
{
    public Text m_TitleText; // 팝업창 제목
    public Image m_MapImage;            // 팝업창 맵 이미지
    public Transform m_PopupInfo;       // 팝업 정보창
    public Button m_StartBtn;           // 시작 버튼

    public GameObject m_ObjectPrefab;   // 오브젝트 프리팹
    public Sprite[] m_MapSprites;       // 맵 이미지들
    public Sprite[] m_ObjectSprites;    // 오브젝트 이미지들 (적, 방해물 등)

    private int m_StageNum;
    private StageInfo m_StageInfo;

    public void Init(int p_StageNum, StageInfo p_Info)
    {
        m_StageNum = p_StageNum;
        m_StageInfo = p_Info;

        var localizedString = new LocalizedString("Stage_Popup", "StagePopup_Key_Title");
        localizedString.StringChanged += (translatedText) =>
        {
            if (this != null && m_TitleText != null)
            {
                m_TitleText.text = translatedText + " " + p_StageNum;
            }
        };

        m_MapImage.sprite = m_MapSprites[m_StageNum - 1];
        PlaceObject();
    }

    private void PlaceObject()
    {
        switch (m_StageInfo.MonsterTypeCount)
        {
            case 1:
                GameObject obj = Instantiate(m_ObjectPrefab, m_PopupInfo);
                obj.GetComponent<StageObjectInfo>().Init(new Vector3(0, -170, 0), m_ObjectSprites[(int)m_StageInfo.MonsterType[0]], m_StageInfo.MonsterCount[0]);
                break;
            case 2:
                GameObject obj1 = Instantiate(m_ObjectPrefab, m_PopupInfo);
                obj1.GetComponent<StageObjectInfo>().Init(new Vector3(-120, -170, 0), m_ObjectSprites[(int)m_StageInfo.MonsterType[0]], m_StageInfo.MonsterCount[0]);
                GameObject obj2 = Instantiate(m_ObjectPrefab, m_PopupInfo);
                obj2.GetComponent<StageObjectInfo>().Init(new Vector3(120, -170, 0), m_ObjectSprites[(int)m_StageInfo.MonsterType[1]], m_StageInfo.MonsterCount[1]);
                break;
            default:
                break;
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene(m_StageNum);
        SoundManager.Instance.DestroySound();
    }
}
