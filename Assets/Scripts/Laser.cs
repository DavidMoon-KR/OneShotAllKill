using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class Laser : MonoBehaviour
{
    //레이저 최대 거리
    [SerializeField]
    private float m_LaserDistance;

    private LineRenderer m_Laser;
    private RaycastHit m_CollidedObject;


    void Start()
    {
        m_Laser = this.gameObject.AddComponent<LineRenderer>();

        //레이저 기본 설정
        //Material material = new Material(Shader.Find("Standard"));
        //material.color = Color.red;
        //m_Laser.material = material;
        m_Laser.material.color = Color.red;
        m_Laser.positionCount = 2;//레이저 꼭짓점 개수

        m_Laser.startWidth = 0.05f;
        m_Laser.endWidth = 0.05f;
    }

    void Update()
    {
        PrintLaser();
    }


    void PrintLaser()
    {
        m_Laser.SetPosition(0, transform.position);
        Debug.DrawRay(transform.position, transform.forward * m_LaserDistance, Color.red, 0.5f);
        if (Physics.Raycast(transform.position, transform.forward, out m_CollidedObject, m_LaserDistance)
        || m_CollidedObject.collider.gameObject.CompareTag("Wall")
            || m_CollidedObject.collider.gameObject.CompareTag("RotateWall"))
            m_Laser.SetPosition(1, m_CollidedObject.point);
        else
            m_Laser.SetPosition(1, transform.position + (transform.forward * m_LaserDistance));
    }

}
