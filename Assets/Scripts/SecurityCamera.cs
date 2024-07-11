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

    // �浹 ó��
    private void OnTriggerEnter(Collider other)
    {
        // ���� Emp�� �¾����� �۵� ���߱�
        if (other.gameObject.tag == "EmpExplosion")
        {
            m_isRotate = false;

            transform.Find("Robot_Scout_HyperX").transform.Find("Cone").gameObject.SetActive(false);
        }
    }
}
