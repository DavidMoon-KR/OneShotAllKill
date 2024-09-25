using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 페이드 인 아웃 효과
public class FadeInOut : MonoBehaviour
{
    [SerializeField] private float m_FadeTime;  // 페이드 되는 시간
    [SerializeField] private Graphic m_FadeUI;  // 페이드 효과에 사용되는 Image UI

    [Tooltip("0 : Infinity | 1~ : RepeatCount")]
    [SerializeField] private int m_RepeatFadeCount = 0; // 반복할 횟수
    private int m_NowRepeatFadeCount = 0;               // 현재까지 반복한 수

    // 설정할 효과
    [SerializeField] private bool m_IsFadeIn = false;       // 페이드 인
    [SerializeField] private bool m_IsFadeOut = false;      // 페이드 아웃
    [SerializeField] private bool m_IsFadeInOut = false;    // 페이드 인 아웃

    // 활성화 되었을 때 FadeInOut 실행
    private void OnEnable()
    {
        StartCoroutine("IFadeInOut");
    }

    // 비활성화 되었을 때 FadeInOut 정지
    private void OnDisable()
    {
        StopCoroutine("IFadeInOut");
    }

    private IEnumerator IFadeInOut()
    {
        while (true)
        {
            // 페이드 인
            if (m_IsFadeIn || m_IsFadeInOut)
            {
                yield return StartCoroutine(Fade(1, 0));    // Fade In
            }
            // 페이드 아웃
            if (m_IsFadeOut || m_IsFadeInOut)
            {
                yield return StartCoroutine(Fade(0, 1));    // Fade Out
            }

            // 반복횟수에 맞게 반복
            m_NowRepeatFadeCount++;
            if (0 != m_RepeatFadeCount && m_RepeatFadeCount <= m_NowRepeatFadeCount)
            {
                m_NowRepeatFadeCount = 0;
                break;
            }

        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            if (1.0f <= m_FadeUI.color.a)
            {
                current = 0;
            }
            current += Time.deltaTime;
            percent = current / m_FadeTime;

            Color color = m_FadeUI.color;
            color.a = Mathf.Lerp(start, end, percent);
            m_FadeUI.color = color;

            yield return null;
        }
    }
}
