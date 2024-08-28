using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetectMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Graphic m_MouseCheckImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = m_MouseCheckImage.color;
        color.a = 1.0f;
        m_MouseCheckImage.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color color = m_MouseCheckImage.color;
        color.a = 0.0f;
        m_MouseCheckImage.color = color;
    }
}
