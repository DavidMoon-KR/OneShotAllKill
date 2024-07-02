using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasExplosion : MonoBehaviour
{
    //폭발 시간
    [SerializeField]
    private float _explosionTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplosionTime());
    }

    //폭발하는 시간 설정
    private IEnumerator ExplosionTime()
    {
        yield return new WaitForSeconds(_explosionTime);
        Destroy(gameObject, _explosionTime);
    }
}
