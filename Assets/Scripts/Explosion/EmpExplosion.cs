using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EmpExplosion : MonoBehaviour
{
    private float DestructionDelay = 1.0f;

    void Start()
    {
        Destroy(gameObject, DestructionDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 만약 Emp에 맞았으면 작동 멈추기
        if (other.gameObject.tag == "SecurityCamera")
        {
            other.GetComponent<SecurityCamera>().StartCoroutines();
        }
    }
}
