using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 투명도에 따른 활성화
public class ActiveOnTransparency : MonoBehaviour
{
    private Image m_Image; // 처리할 이미지
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    void Update()
    {
        // 이미지의 투명도가 0보다 작아지면(이미지가 완전히 투명해지면)
        // 오브젝트 비활성화
        if (0 >= m_Image.color.a)
        {
            this.gameObject.SetActive(false);
        }
    }
}
