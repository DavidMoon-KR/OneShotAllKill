using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial3 : MonoBehaviour
{
    private int m_NowGimicNumber = 1;

    [SerializeField] private List<GameObject> m_GimicObjects;

    [SerializeField] private TMP_Text m_PopupDialogue;
    [SerializeField] private TMP_Text m_PopupBtnText;
    private List<string> m_DialogueList = new List<string>();
    private string m_ClosingText = "닫기";
    private int m_NowDialogue = -1;
    private bool m_IsTyping = false;
    private float m_TypingSpeed = 0.05f;

    [SerializeField] private List<GameObject> m_EnergyWall;
    [SerializeField] private GameObject m_Turret;
    [SerializeField] private GameObject m_SecurityCameraCone;

    void Start()
    {
        m_DialogueList.Add("감시카메라에 감시망이 있습니다");
        m_DialogueList.Add("감시망에 폭발이 포착되지 않도록 주의하세요");
        m_DialogueList.Add("감시카메라는 EMP탄(<sprite=0>)을 활용해서 무력화하세요");

        TutoMeesage();
    }

    void Update()
    {
        if (2 == m_NowGimicNumber)
        {
            TutorialManager.Instance.IsTutorial = false;
            GameManager.Instance.IsGamePause = false;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                TutorialManager.Instance.IsTutorial = true;

                m_EnergyWall[0].SetActive(false);

                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);
                m_NowGimicNumber++;
            }            
        }

        if (3 == m_NowGimicNumber)
        {
            if(m_Turret == null && !m_SecurityCameraCone.activeSelf)
            {
                TutorialManager.Instance.IsTutorial = false;

                m_EnergyWall[0].SetActive(true);
                m_EnergyWall[1].SetActive(false);
                m_EnergyWall[2].SetActive(false);

                m_NowGimicNumber++;
            }
        }
    }

    // 게임 시작하자마자 튜토리얼 메시지 출력
    public void TutoMeesage()
    {
        GameManager.Instance.IsGamePause = true;
        TutorialManager.Instance.IsTutorial = true;

        m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

        OnClickPopupButton();
    }

    // 대화창에서 다음, 확인 버튼을 클릭했을 경우
    public void OnClickPopupButton()
    {
        if (m_IsTyping)
        {
            m_PopupDialogue.text = m_DialogueList[m_NowDialogue];
            m_IsTyping = false;
            return;
        }

        m_NowDialogue++;

        // 닫기 버튼을 눌렀을 경우
        if (m_NowDialogue > (m_DialogueList.Count - 1))
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);
            m_NowGimicNumber++;
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);
            return;
        }
        // 마지막 텍스트인 경우
        if (m_NowDialogue >= (m_DialogueList.Count - 1))
        {
            m_PopupBtnText.text = m_ClosingText;
        }

        StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        m_PopupDialogue.text = null;
        m_IsTyping = true;

        for (int i = 0; i < m_DialogueList[m_NowDialogue].Length; i++)
        {
            // 타이핑이 끝났다면 더이상 진행하지 않음
            if (!m_IsTyping)
                break;

            // <sprite=0>을 쓰는 과정을 최소한으로 만들어서 눈에 안보이게 만들기
            if (m_DialogueList[m_NowDialogue][i] == '<')
            {
                m_TypingSpeed = 0;
            }
            // <sprite=0>을 쓰고나면 다시 정상속도로 변경
            else if (m_DialogueList[m_NowDialogue][i] == '>')
            {
                m_TypingSpeed = 0.05f;
            }

            m_PopupDialogue.text += m_DialogueList[m_NowDialogue][i];
            yield return new WaitForSeconds(m_TypingSpeed);
        }
        m_IsTyping = false;
    }
}
