using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageObjectInfo : MonoBehaviour
{
    // 오브젝트 정보
    public Image m_ObjectImage;
    public TextMeshProUGUI m_ObjectCount;

    public void Init(Vector3 p_Postion, Sprite p_Sprite, int p_Count)
    {
        RectTransform trans = GetComponent<RectTransform>();
        trans.localPosition = p_Postion;
        m_ObjectImage.sprite = p_Sprite;
        m_ObjectCount.text = p_Count.ToString();
    }
}
