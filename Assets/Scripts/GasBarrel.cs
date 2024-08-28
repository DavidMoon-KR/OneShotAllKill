using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBarrel : MonoBehaviour
{
    // Æø¹ß ÇÁ¸®ÆÕ
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
    // Åº ¶Ç´Â °¡½ºÆø¹ß°ú Ãæµ¹ÇÑ °æ¿ì
    private void OnTriggerEnter(Collider other)
    {

    }
}
