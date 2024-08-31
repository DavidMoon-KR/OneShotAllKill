using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBarrel : MonoBehaviour
{
    // ���� ������
    [SerializeField] private GameObject m_ExplosionObject;
    [SerializeField] private float m_ImpactTime;
    [SerializeField] private float m_mpactGauage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("GasExplosion"))
        {
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_mpactGauage);
            Instantiate(m_ExplosionObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    // ź �Ǵ� �������߰� �浹�� ���
    private void OnTriggerEnter(Collider other)
    {

    }
}
