using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUnlock : MonoBehaviour
{
    public Text m_StageNumber;

    public Sprite[] m_Sprites = new Sprite[3];

    private Button m_ThisButton;

    void Start()
    {
        m_ThisButton = GetComponent<Button>();
    }

    public void Init(bool p_IsClear)
    {
        // 스테이지 숫자 활성화
        m_StageNumber.gameObject.SetActive(true);
        m_ThisButton.interactable = true;

        // 클리어 여부에 따라 이미지 교체
        if (p_IsClear)
        {
            m_ThisButton.image.sprite = m_Sprites[0];
        }
        else
        {
            m_ThisButton.image.sprite = m_Sprites[1];
        }
    }

    public void Unlock()
    {
        m_StageNumber.gameObject.SetActive(false);
        m_ThisButton.interactable = false;

        m_ThisButton.image.sprite = m_Sprites[2];
    }
}
