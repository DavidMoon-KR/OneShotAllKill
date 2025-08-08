using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ArrowChange : MonoBehaviour
{
    [SerializeField] private Button m_PrevArrow;  // 이전 화살표
    [SerializeField] private Button m_NextArrow;  // 다음 화살표
    [SerializeField] private TextMeshProUGUI m_String;  // 바꿀 글자

    [SerializeField] private List<string> m_StringList; // 바뀔 글자 리스트

    [SerializeField] private Sprite m_PrevArrowImage_f;  // 이전 화살표 활성화 이미지
    [SerializeField] private Sprite m_PrevArrowImage_n;  // 이전 화살표 비활성화 이미지

    [SerializeField] private Sprite m_NextArrowImage_f;  // 다음 화살표 활성화 이미지
    [SerializeField] private Sprite m_NextArrowImage_n;  // 다음 화살표 비활성화 이미지

    private int m_StringIndex = 0;  // 현재 글자 인덱스

    void Start()
    {
        m_StringIndex = (int)GameDataManager.Instance.Data.Language;
        ChangeLanguage();

        if (m_StringIndex <= 0)
        {
            m_PrevArrow.image.sprite = m_PrevArrowImage_n;
        }
        if (m_StringIndex >= m_StringList.Count - 1)
        {
            m_NextArrow.image.sprite = m_NextArrowImage_n;
        }
    }

    // 이전 화살표를 눌렀을 때
    public void OnClickPrevArrow()
    {
        // 더 이상 이전으로 갈 수 없다면 아예 실행x
        if (m_StringIndex <= 0) return;

        m_StringIndex--;    // 인덱스 감소
        // 왼쪽 끝에 도달했다면 이전 화살표 비활성화로 바꾸기
        if (m_StringIndex <= 0)
        {
            m_PrevArrow.image.sprite = m_PrevArrowImage_n;
        }
        
        m_NextArrow.image.sprite = m_NextArrowImage_f;  // 다음 화살표 활성화
        ChangeLanguage();   // 언어 변경
    }

    // 다음 화살표를 눌렀을 때
    public void OnClickNextArrow()
    {
        // 더 이상 다음으로 갈 수 없다면 아예 실행x
        if (m_StringIndex >= m_StringList.Count - 1) return;

        m_StringIndex++;    // 인덱스 감소
        // 오른쪽 끝에 도달했다면 이전 화살표 비활성화로 바꾸기
        if (m_StringIndex >= m_StringList.Count - 1)
        {
            m_NextArrow.image.sprite = m_NextArrowImage_n;
        }

        m_PrevArrow.image.sprite = m_PrevArrowImage_f;  // 이전 화살표 활성화
        ChangeLanguage();   // 언어 변경
    }

    // 언어 변경
    private void ChangeLanguage()
    {
        m_String.text = m_StringList[m_StringIndex];
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[m_StringIndex];

        GameDataManager.Instance.Data.Language = (Language)m_StringIndex;
        GameDataManager.Instance.Save();
    }
}
