using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GasBarrel : MonoBehaviour
{
    // Æø¹ß ÇÁ¸®ÆÕ
    [SerializeField] private GameObject m_ExplosionObject;
    [SerializeField] private float m_ImpactTime;
    [SerializeField] private float m_mpactGauage;
    private bool m_HitLaser = false;
    [SerializeField] private float m_Radius;
    private LineRenderer m_Line;    
    private NavMeshSurface m_NavMeshSurface;
    public bool HitLaser { set { m_HitLaser = value; } }
    private void Start()
    {
        m_Line = gameObject.GetComponent<LineRenderer>();
        m_Line.positionCount = 81;
        m_Line.useWorldSpace = false;
        m_Line.startWidth = 0.05f;
        m_Line.endWidth = 0.05f;
        Color temp = m_Line.startColor;
        temp.a = 0.5f;
        m_Line.startColor = temp;
        m_Line.loop = true;
        m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();
    }

    private void Update()
    {
        if (m_HitLaser)
        {
            if(m_Line.material.color.a == 0.0f)
            {
                Color temp = m_Line.material.color;
                temp.a = 1.0f;
                m_Line.material.color = temp;
            }
            DrawExplosionRange();
        }
            
        else
        {
            Color temp = m_Line.material.color;
            temp.a = 0.0f;
            m_Line.material.color = temp;
            DrawExplosionRange();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_mpactGauage);
            Instantiate(m_ExplosionObject, transform.position, transform.rotation);
            if (m_NavMeshSurface != null)            
                m_NavMeshSurface.BuildNavMesh();            
            Destroy(gameObject);
        }
    }
    // Åº ¶Ç´Â °¡½ºÆø¹ß°ú Ãæµ¹ÇÑ °æ¿ì
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("GasExplosion"))
        {
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_mpactGauage);
            Instantiate(m_ExplosionObject, transform.position, transform.rotation);
            if (m_NavMeshSurface != null)            
                m_NavMeshSurface.BuildNavMesh();            
            Destroy(gameObject);
        }
    }

    public void DrawExplosionRange()
    {
        float p_angle = 0;
        for(int i = 0;i<m_Line.positionCount;i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * p_angle) * m_Radius;
            float z = Mathf.Cos(Mathf.Deg2Rad * p_angle) * m_Radius;
            m_Line.SetPosition(i,new Vector3(x, 0, z));
            p_angle += 360f / (m_Line.positionCount - 1);
        }
    }

}
