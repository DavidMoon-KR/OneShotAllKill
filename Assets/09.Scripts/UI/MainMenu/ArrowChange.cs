using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ArrowChange : MonoBehaviour
{
    [SerializeField] private Button m_PrevArrow;  // ���� ȭ��ǥ
    [SerializeField] private Button m_NextArrow;  // ���� ȭ��ǥ
    [SerializeField] private TextMeshProUGUI m_String;  // �ٲ� ����

    [SerializeField] private List<string> m_StringList; // �ٲ� ���� ����Ʈ

    [SerializeField] private Sprite m_PrevArrowImage_f;  // ���� ȭ��ǥ Ȱ��ȭ �̹���
    [SerializeField] private Sprite m_PrevArrowImage_n;  // ���� ȭ��ǥ ��Ȱ��ȭ �̹���

    [SerializeField] private Sprite m_NextArrowImage_f;  // ���� ȭ��ǥ Ȱ��ȭ �̹���
    [SerializeField] private Sprite m_NextArrowImage_n;  // ���� ȭ��ǥ ��Ȱ��ȭ �̹���

    private int m_StringIndex = 0;  // ���� ���� �ε���

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

    // ���� ȭ��ǥ�� ������ ��
    public void OnClickPrevArrow()
    {
        // �� �̻� �������� �� �� ���ٸ� �ƿ� ����x
        if (m_StringIndex <= 0) return;

        m_StringIndex--;    // �ε��� ����
        // ���� ���� �����ߴٸ� ���� ȭ��ǥ ��Ȱ��ȭ�� �ٲٱ�
        if (m_StringIndex <= 0)
        {
            m_PrevArrow.image.sprite = m_PrevArrowImage_n;
        }
        
        m_NextArrow.image.sprite = m_NextArrowImage_f;  // ���� ȭ��ǥ Ȱ��ȭ
        ChangeLanguage();   // ��� ����
    }

    // ���� ȭ��ǥ�� ������ ��
    public void OnClickNextArrow()
    {
        // �� �̻� �������� �� �� ���ٸ� �ƿ� ����x
        if (m_StringIndex >= m_StringList.Count - 1) return;

        m_StringIndex++;    // �ε��� ����
        // ������ ���� �����ߴٸ� ���� ȭ��ǥ ��Ȱ��ȭ�� �ٲٱ�
        if (m_StringIndex >= m_StringList.Count - 1)
        {
            m_NextArrow.image.sprite = m_NextArrowImage_n;
        }

        m_PrevArrow.image.sprite = m_PrevArrowImage_f;  // ���� ȭ��ǥ Ȱ��ȭ
        ChangeLanguage();   // ��� ����
    }

    // ��� ����
    private void ChangeLanguage()
    {
        m_String.text = m_StringList[m_StringIndex];
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[m_StringIndex];

        GameDataManager.Instance.Data.Language = (Language)m_StringIndex;
        GameDataManager.Instance.Save();
    }
}
