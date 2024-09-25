using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마우스 클릭이 가능한 오브젝트 표시해주기
public class IndicateClickAbleObject : MonoBehaviour
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
