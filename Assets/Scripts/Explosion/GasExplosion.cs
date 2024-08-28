using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasExplosion : MonoBehaviour
{
    // ���� �ð�
    [SerializeField] private float m_ExplosionTime;

    void Start()
    {
        StartCoroutine(ExplosionTime());
    }

    // �����ϴ� �ð� ����
    private IEnumerator ExplosionTime()
    {
        yield return new WaitForSeconds(m_ExplosionTime);
        Destroy(gameObject, m_ExplosionTime);
    }
}
