using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] public float m_DefaultLength;

    private LineRenderer m_Laser;
    private Camera m_Cam;
    private RaycastHit m_CollidedObject;
    private int m_Reflections = 10;//ƨ��� Ƚ�� ���̰Ÿ� �����ؾ� ������ ƨ��ϴ١�

    private bool m_IsReflected;
    private Vector3 m_Reflect;
    private Ray m_Ray;
    private Vector3 m_Direction;

    private void Start()
    {//������ �ʱ� ����(��, ����) ����
        m_Laser = GetComponent<LineRenderer>();
        m_Cam = Camera.main;
        m_Laser.startWidth = 0.05f;
        m_Laser.endWidth = 0.05f;
    }

    private void Update()
    {
        ReflectLaser();
    }

    void ReflectLaser()
    {

        m_Ray = new Ray(transform.position, transform.forward);
        m_Laser.positionCount = 1;
        m_IsReflected = false;
        //�ǽð����� �������� �浹�� �����Ͽ� ���� ������ �ø��� ���� ���� ������ �׻� 1���� �ʱ�ȭ
        m_Laser.SetPosition(0, transform.position);
        float remainLength = m_DefaultLength;
        for (int i = 0; i < m_Reflections; i++)
        {
            if (Physics.Raycast(m_Ray.origin, m_Ray.direction, out m_CollidedObject, remainLength))//�浹�Ͽ��°�?
            {
                m_Laser.positionCount++;//�������� ������ ���� ���� ������ 1�� �ø�
                if ((m_CollidedObject.collider.tag == "Wall" || m_CollidedObject.collider.tag == "RotateWall"))//ƨ��� �ִ� ���� �浹�Ͽ��°�?
                {                   
                    m_Laser.SetPosition(m_Laser.positionCount - 1, m_CollidedObject.point);//�ε��� �� ���� ���� ��� �� ����
                    remainLength -= Vector3.Distance(m_Ray.origin, m_CollidedObject.point);//���� ���̸� ����
                    m_Reflect = Vector3.Reflect(m_Ray.direction, m_CollidedObject.normal);
                    m_IsReflected = true;
                    m_Ray = new Ray(m_CollidedObject.point, Vector3.Reflect(m_Ray.direction, m_CollidedObject.normal));//�ݻ簢 ���ϱ�
                }
                else if(m_CollidedObject.collider.tag == "Broken")//�Ѿ��� �ε������� �ı��Ǵ� ������Ʈ�� ��������                                    
                    m_Laser.SetPosition(m_Laser.positionCount - 1, m_CollidedObject.point);//���� ��� �� ����                
                else
                {
                    m_Laser.SetPosition(m_Laser.positionCount - 1, m_CollidedObject.point);//���� ��� �� ����                
                    remainLength -= Vector3.Distance(m_Ray.origin, m_CollidedObject.point);//���� ���̸� ����
                    if (m_IsReflected)
                        m_Ray = new Ray(m_CollidedObject.point + m_Reflect, m_Reflect);
                    else
                        m_Ray = new Ray(m_CollidedObject.point + transform.forward, transform.forward);
                }
            }
            else//��𿡵� �ε����� �ʾ��� ���
            {
                m_Laser.positionCount++;//�������� ������ ���� ���� ������ 1�� �ø�
                m_Laser.SetPosition(m_Laser.positionCount - 1, m_Ray.origin + (m_Ray.direction * remainLength));//�������� ���� �Ÿ���ŭ ������ ����
            }
        }
    }
}
    /*void NormalLaser()//Ȥ�� ���� ���ܵ� ƨ���� �ʴ� ������
    {
        m_Laser.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out m_CollidedObject, m_DefaultLength))
        {
            m_Laser.SetPosition(1, m_CollidedObject.point);
        }
        else
        {
            m_Laser.SetPosition(1, transform.position + (transform.forward * m_DefaultLength));
        }
    }
}


using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    //������ �ִ� �Ÿ�
    //[SerializeField]
    //private float m_LaserDistance;
    private LineRenderer m_Laser;
    private RaycastHit m_CollidedObject;
    [SerializeField]
    private int m_Reflections;
    [SerializeField]
    private Transform m_StartPoint;
    private Ray ray;
    //[SerializeField]
    //Vector3 RefelectVec;

    void Start()
    {
        m_Laser = GetComponent<LineRenderer>();
        m_Laser.SetPosition(0, transform.position);
        //������ �⺻ ����
        //Material material = new Material(Shader.Find("Standard"));
        //material.color = Color.red;
        //m_Laser.material = material;

    }

    void Update()
    {
        PrintLaser(transform.position,transform.forward);
    }


    void PrintLaser(Vector3 position,Vector3 direction)
    {
        m_Laser.SetPosition(0, m_StartPoint.position);

        for (int i =0;i<m_Reflections;i++)
        {
            ray = new Ray(position, direction);
            
            if(Physics.Raycast(ray,out m_CollidedObject,300,1))
            {
                position = m_CollidedObject.point;
                direction = Vector3.Reflect(direction, m_CollidedObject.normal);
                m_Laser.SetPosition(i + 1, m_CollidedObject.point);

                if(m_CollidedObject.transform.name != "Wall" || m_CollidedObject.transform.name != "RotatingWall")
                {
                    for(int j = (i+1);j<=m_Reflections;j++)
                    {
                        m_Laser.SetPosition(j, m_CollidedObject.point);
                    }
                    break;
                }
            }
        }

        
        /*
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
*/