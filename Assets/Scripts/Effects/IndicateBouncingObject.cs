using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마우스를 올렸을 때 머티리얼을 변경해서 선택된 느낌을 표현
public class IndicateBouncingObject : MonoBehaviour
{
    [SerializeField] List<Material> m_Materials;

    private void OnMouseEnter()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = m_Materials[1];
    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = m_Materials[0];
    }
}
