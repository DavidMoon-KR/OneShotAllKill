using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour
{
    private int m_NowGimicNumber = 1;

    // 각종 마크 및 표시들
    [SerializeField] private List<GameObject> m_GimicObjects;

    // 튜토리얼에 사용할 오브젝트들
    [SerializeField] private Transform m_RicochetWall1; // 이동 장벽1
    [SerializeField] private Transform m_Turret1;       // 터렛

    [SerializeField] private GameObject m_WallObjects;

    [SerializeField] private TMP_Text m_PopupDialogue;
    private string m_Dialogue = "벽을 우클릭으로 잡고 끌어당기세요";
    private bool m_IsTyping = false;
    private float m_TypingSpeed = 0.05f;

    // 튜토리얼1 스크립트를 인스턴스화 한 것
    private static Tutorial1 m_Instance;
    public static Tutorial1 Instance => m_Instance;

    void Awake()
    {
        m_Instance = GetComponent<Tutorial1>();
    }

    void Update()
    {
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        // 1번 기믹
        // 벽 클릭
        // OnClickWall()

        // 2번 기믹
        // 팝업 버튼 클릭
        // OnClickPopupButton()

        // 3번 기믹
        // 벽을 지정된 위치로 옮기기
        if (3 == m_NowGimicNumber)
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

            // 벽을 제대로 된 위치에 옮겼다면 다음 기믹으로 넘어가기
            if (m_RicochetWall1.position.x < 1)
            {
                m_GimicObjects[0].SetActive(false);
                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);

                m_NowGimicNumber++;
            }
        }

        // 4번 기믹
        // 터렛 파괴하기
        if (4 == m_NowGimicNumber)
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

            // 터렛을 파괴했다면 다음 대사로 넘어가기
            if (m_Turret1 == null)
            {
                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);

                m_NowGimicNumber++;
            }
        }
    }

    public void OnClickWall()
    {
        m_NowGimicNumber++;
        m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

        StartCoroutine(Typing());
    }

    // 대화창에서 확인 버튼을 클릭했을 경우
    public void OnClickPopupButton()
    {
        if (m_IsTyping)
        {
            m_PopupDialogue.text = m_Dialogue;
            m_IsTyping = false;
            return;
        }

        m_WallObjects.gameObject.SetActive(false);
        m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);

        m_NowGimicNumber++;
        m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);
    }

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
