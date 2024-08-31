using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering.PostProcessing;
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

    void Update()
    {
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        // 1번 기믹
        // 벽을 지정된 위치로 옮기기
        if (1 == m_NowGimicNumber)
        {
            m_GimicObjects[m_NowGimicNumber - 1].SetActive(true);

            // 벽을 제대로 된 위치에 옮겼다면 다음 기믹으로 넘어가기
            if (m_RicochetWall1.position.x < 1)
            {
                m_GimicObjects[m_NowGimicNumber - 1].SetActive(false);

                m_NowGimicNumber++;
            }
        }

        // 2번 기믹
        // 터렛 파괴하기
        if (2 == m_NowGimicNumber)
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
}
