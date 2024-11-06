using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마우스 클릭이 가능한 오브젝트 표시해주기
public class IndicateClickAbleObject : MonoBehaviour
{
    [SerializeField] List<Material> m_Materials;

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    private void OnDisable()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = m_Materials[0];
    }

    private void OnMouseEnter()
    {
        if (this.enabled)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = m_Materials[1];
        }        
    }

    private void OnMouseExit()
    {
        if (this.enabled)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = m_Materials[0];
        }
    }
}
