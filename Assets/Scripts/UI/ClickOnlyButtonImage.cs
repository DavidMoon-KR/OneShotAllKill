using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 버튼에서 눈에 보이는 이미지만 클릭되도록 하기
public class ClickOnlyButtonImage : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
