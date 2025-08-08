using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Tutorial2 : MonoBehaviour
{
    // 각종 오브젝트들
    [SerializeField] private GameObject m_GimicObject;         // 기믹 오브젝트
    [SerializeField] private GameObject m_TopRotateWall1;      // 회전 벽 오브젝트 1
    [SerializeField] private GameObject m_BottomRotateWall2;   // 회전 벽 오브젝트 2

    [SerializeField] private GameObject m_Laser;    // 레이저 이미지
    [SerializeField] private GameObject m_Gimic2;   // 기믹2

    [SerializeField] private GameObject m_EnergyWall; // 에너지벽 오브젝트

    private bool m_IsTopWallClear = false;
    private bool m_IsBottomWallClear = false;

    // 타이핑 관련 변수들
    [SerializeField] private TMP_Text m_PopupDialogue;
    private string m_TableName = "Tutorial2"; // Localization 테이블 이름
    private string entryPrefix = "Tuto2_Tutorial_0"; // Entry 키 prefix
    private string m_Dialogue;
    private bool m_IsTyping = false;
    private float m_TypingSpeed = 0.05f;

    void Start()
    {
        Invoke("Gimic1", 1f);
        StartCoroutine(LoadLocalizedString());
    }

    IEnumerator LoadLocalizedString()
    {
        var table = LocalizationSettings.StringDatabase;
        var localizedString = table.GetLocalizedStringAsync(m_TableName, entryPrefix);
        yield return localizedString;
        m_Dialogue = localizedString.Result;
    }

    void Update()
    {
        if (!m_EnergyWall.activeSelf)
        {
            m_Laser.gameObject.SetActive(false);
            m_Gimic2.SetActive(true);

            m_BottomRotateWall2.GetComponent<IndicateClickAbleObject>().enabled = true;
            m_BottomRotateWall2.GetComponent<RotatingWall>().enabled = true;

            m_TopRotateWall1.GetComponent<IndicateClickAbleObject>().enabled = true;
            m_TopRotateWall1.GetComponent<RotatingWall>().enabled = true;

            return;
        }

        if (!m_IsTopWallClear &&
            m_TopRotateWall1.transform.eulerAngles.y >= 45 && m_TopRotateWall1.transform.eulerAngles.y <= 225)
        {
            m_IsTopWallClear = true;

            m_TopRotateWall1.GetComponent<IndicateClickAbleObject>().enabled = false;
            RotatingWall temp = m_TopRotateWall1.GetComponent<RotatingWall>();
            temp.MouseRButtonUp();
            temp.enabled = false;
            m_TopRotateWall1.transform.eulerAngles = new Vector3(0, 45, 0);
        }
        
        if (!m_IsBottomWallClear &&
            m_BottomRotateWall2.transform.eulerAngles.y >= 45 && m_BottomRotateWall2.transform.eulerAngles.y <= 225)
        {
            m_IsBottomWallClear = true;

            m_BottomRotateWall2.GetComponent<IndicateClickAbleObject>().enabled = false;
            RotatingWall temp = m_BottomRotateWall2.GetComponent<RotatingWall>();
            temp.MouseRButtonUp();
            temp.enabled = false;
            m_BottomRotateWall2.transform.eulerAngles = new Vector3(0, 45, 0);
        }

        if (m_IsBottomWallClear && m_IsTopWallClear)
        {
            m_Laser.SetActive(true);
            m_Gimic2.SetActive(true);
        }
    }

    private void Gimic1()
    {
        m_GimicObject.SetActive(true);

        // 메시지창 뜨는 동안에는 게임 일시정지
        GameManager.Instance.IsGamePause = true;
        TutorialManager.Instance.IsTutorial = true;

        StartCoroutine(Typing());
    }

    // 대화창에서 확인 버튼을 클릭했을 경우
    public void OnClickPopupButton()
    {
        // 아직 타이핑 중이라면
        // 나머지 대사가 모두 나오도록 설정
        if (m_IsTyping)
        {
            m_PopupDialogue.text = m_Dialogue;
            m_IsTyping = false;
            return;
        }

        // 일시정지 해제
        GameManager.Instance.IsGamePause = false;
        TutorialManager.Instance.IsTutorial = false;

        m_GimicObject.SetActive(false);
    }

    // 타이핑 효과
    IEnumerator Typing()
    {
        m_PopupDialogue.text = null;
        m_IsTyping = true;

        for (int i = 0; i < m_Dialogue.Length; i++)
        {
            // 타이핑이 끝났다면 더이상 진행하지 않음
            if (!m_IsTyping)
                break;

            m_PopupDialogue.text += m_Dialogue[i];
            yield return new WaitForSeconds(m_TypingSpeed);
        }
        m_IsTyping = false;
    }
}
