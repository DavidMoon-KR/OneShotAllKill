using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasExplosion : MonoBehaviour
{
    // 폭발 시간
    [SerializeField]
    private float m_ExplosionTime;

    void Start()
    {
        StartCoroutine(ExplosionTime());
    }

    // 폭발하는 시간 설정
    private IEnumerator ExplosionTime()
    {
        yield return new WaitForSeconds(m_ExplosionTime);
        Destroy(gameObject, m_ExplosionTime);
    }
}
