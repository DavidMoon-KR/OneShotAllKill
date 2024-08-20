using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.PostProcessing;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial3 : MonoBehaviour
{
    private int m_NowGimicNumber = 1;

    // 각종 마크 및 표시들
    [SerializeField]
    private GameObject m_FirstGimicObjects;
    [SerializeField]
    private GameObject m_SecondGimicObjects;

    void Update()
    {
        if (1 == m_NowGimicNumber)
        {
            TutorialManager.Instance.IsActive = false;
            m_FirstGimicObjects.SetActive(true);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_FirstGimicObjects.SetActive(false);

                m_NowGimicNumber++;
                TutorialManager.Instance.IsActive = true;
            }
        }

        if (2 == m_NowGimicNumber)
        {
            TutorialManager.Instance.IsActive = false;
            m_SecondGimicObjects.SetActive(true);
        }
    }
}
