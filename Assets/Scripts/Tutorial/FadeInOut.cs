using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    private float m_FadeTime;   // ���̵� �Ǵ� �ð�
    [SerializeField]
    private Graphic m_FadeText;    // ���̵� ȿ���� ���Ǵ� Image UI

    private void OnEnable()
    {
        // Fade ȿ���� In -> Out ���� �ݺ��Ѵ�.
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

            Color color = m_FadeText.color;
            color.a = Mathf.Lerp(start, end, percent);
            m_FadeText.color = color;

            yield return null;
        }
    }
}
