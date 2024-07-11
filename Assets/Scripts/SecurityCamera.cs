using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField]
    private float m_RotateSpeed;
    private bool m_isRotate = true;

    void Update()
    {
        if (m_isRotate)
        {
            transform.Rotate(0, m_RotateSpeed * Time.deltaTime, 0);
        }
    }

    // 충돌 처리
    private void OnTriggerEnter(Collider other)
    {
        // 만약 Emp에 맞았으면 작동 멈추기
        if (other.gameObject.tag == "EmpExplosion")
        {
            m_isRotate = false;

            transform.Find("Robot_Scout_HyperX").transform.Find("Cone").gameObject.SetActive(false);
        }
    }
}
