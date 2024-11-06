using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpExplosionRange : MonoBehaviour
{
    [SerializeField] private int m_Radius;
    private LineRenderer m_Line;
    void Start()
    {
        m_Radius = 3;
        m_Line = gameObject.GetComponent<LineRenderer>();
        m_Line.positionCount = 81;
        m_Line.startWidth = 0.05f;
        m_Line.endWidth = 0.05f;
        m_Line.startColor = Color.cyan;
        Color temp = m_Line.startColor;
        temp.a = 0.0f;
        m_Line.startColor = temp;
        m_Line.loop = true;
    }
    public void DrawEmpExplosionRange(bool p_IsEmp, Vector3 p_ExplosionPosition)
    {
        if (p_IsEmp)
        {
            Color temp = m_Line.material.color;
            temp.a = 1.0f;
            m_Line.material.color = temp;
        }
        else
        {
            Color temp = m_Line.material.color;
            temp.a = 0.0f;
            m_Line.material.color = temp;
        }
        
        float p_angle = 0;
        for (int i = 0; i < m_Line.positionCount; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * p_angle) * m_Radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * p_angle) * m_Radius;
            m_Line.SetPosition(i, new Vector3(x + p_ExplosionPosition.x, 0, z + p_ExplosionPosition.z));
            p_angle += 360f / (m_Line.positionCount - 1);
        }
    }

}
