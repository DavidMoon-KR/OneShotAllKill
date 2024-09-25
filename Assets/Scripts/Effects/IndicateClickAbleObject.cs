using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���콺 Ŭ���� ������ ������Ʈ ǥ�����ֱ�
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
