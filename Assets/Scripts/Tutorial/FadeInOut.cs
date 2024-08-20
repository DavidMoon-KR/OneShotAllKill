using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    private float m_FadeTime;   // 페이드 되는 시간
    [SerializeField]
    private Graphic m_FadeGraphic;    // 페이드 효과에 사용되는 Image UI

    private void OnEnable()
    {
        // Fade 효과를 In -> Out 무한 반복한다.
        StartCoroutine("IFadeInOut");
    }

    private void OnDisable()
    {
        StopCoroutine("IFadeInOut");
    }

    private IEnumerator IFadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));    // Fade In

            yield return StartCoroutine(Fade(0, 1));    // Fade Out
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / m_FadeTime;

            Color color = m_FadeGraphic.color;
            color.a = Mathf.Lerp(start, end, percent);
            m_FadeGraphic.color = color;

            yield return null;
        }
    }
}
