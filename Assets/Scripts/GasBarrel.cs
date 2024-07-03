using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBarrel : MonoBehaviour
{
    // ���� ������
    [SerializeField]
    private GameObject _explosionObject;
    [SerializeField]
    private float _impactTime;
    [SerializeField]
    private float _impactGauage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ź �Ǵ� �������߰� �浹�� ���
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet" || other.tag == "GasExplosion")
        {
            CameraShake.Instance.OnShakeCamera(_impactTime, _impactGauage);
            Instantiate(_explosionObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
