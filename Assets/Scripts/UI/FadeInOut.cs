using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private float m_FadeTime;  // ���̵� �Ǵ� �ð�
    [SerializeField] private Graphic m_FadeUI;  // ���̵� ȿ���� ���Ǵ� Image UI

    [Tooltip ("0 : Infinity | 1~ : RepeatCount")]
    [SerializeField] private int m_RepeatFadeCount = 0;
    private int m_NowRepeatFadeCount = 0;

    [SerializeField] private bool m_IsFadeIn = false;
    [SerializeField] private bool m_IsFadeOut = false;
    [SerializeField] private bool m_IsFadeInOut = false;

    // Ȱ��ȭ �Ǿ��� �� FadeInOut ����
    private void OnEnable()
    {
        StartCoroutine("IFadeInOut");
    }

    // ��Ȱ��ȭ �Ǿ��� �� FadeInOut ����
    private void OnDisable()
    {
        StopCoroutine("IFadeInOut");
    }

    private IEnumerator IFadeInOut()
    {
        while (true)
        {
            if (m_IsFadeIn || m_IsFadeInOut)
            {
                yield return StartCoroutine(Fade(1, 0));    // Fade In
            }
            if (m_IsFadeOut || m_IsFadeInOut)
            {
                yield return StartCoroutine(Fade(0, 1));    // Fade Out
            }

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
