using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBarrel : MonoBehaviour
{
    //Æø¹ß ÇÁ¸®ÆÕ
    [SerializeField]
    private GameObject _explosionObject;
    [SerializeField]
    private float _impactTime;
    [SerializeField]
    private float _impactGauage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Åº ¶Ç´Â °¡½ºÆø¹ß°ú Ãæµ¹ÇÑ °æ¿ì
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
