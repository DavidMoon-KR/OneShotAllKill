using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasExplosion : MonoBehaviour
{
    // ���� �ð�
    [SerializeField]
    private float _explosionTime;

    void Start()
    {
        StartCoroutine(ExplosionTime());
    }

    // �����ϴ� �ð� ����
    private IEnumerator ExplosionTime()
    {
        yield return new WaitForSeconds(_explosionTime);
        Destroy(gameObject, _explosionTime);
    }
}