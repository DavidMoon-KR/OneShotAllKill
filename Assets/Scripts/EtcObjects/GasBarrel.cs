using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GasBarrel : MonoBehaviour
{
    // ���� ������
    [SerializeField] private GameObject m_ExplosionObject;
    [SerializeField] private float m_ImpactTime;
    [SerializeField] private float m_mpactGauage;
    
    private NavMeshSurface m_NavMeshSurface;

    private void Start()
    {
        m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();
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
    // ź �Ǵ� �������߰� �浹�� ���
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
}
