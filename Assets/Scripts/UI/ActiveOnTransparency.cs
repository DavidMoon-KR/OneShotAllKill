using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ������ ���� Ȱ��ȭ
public class ActiveOnTransparency : MonoBehaviour
{
    private Image m_Image; // ó���� �̹���
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    void Update()
    {
        // �̹����� ������ 0���� �۾�����(�̹����� ������ ����������)
        // ������Ʈ ��Ȱ��ȭ
        if (0 >= m_Image.color.a)
        {
            this.gameObject.SetActive(false);
        }
    }
}
