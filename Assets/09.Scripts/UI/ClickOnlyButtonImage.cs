using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ư���� ���� ���̴� �̹����� Ŭ���ǵ��� �ϱ�
public class ClickOnlyButtonImage : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
