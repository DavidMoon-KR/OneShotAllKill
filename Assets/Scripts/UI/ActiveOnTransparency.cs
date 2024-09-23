using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOnTransparency : MonoBehaviour
{
    private Image m_Image;
    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (0 >= m_Image.color.a)
        {
            this.gameObject.SetActive(false);
        }
    }
}
