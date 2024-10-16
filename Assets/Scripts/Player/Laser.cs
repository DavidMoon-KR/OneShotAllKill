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
    private bool m_IsCollidedAccessButton;

    private bool m_HitGasBarrel = false;
    private bool m_IsReflected;
    private Vector3 m_Reflect;
    private Ray m_Ray;
    private Vector3 m_Direction;

    private void Start()
    {
        m_IsCollidedAccessButton = false;
        //������ �ʱ� ����(��, ����) ����
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
        m_IsCollidedAccessButton = false;
        m_HitGasBarrel = false;
        //�ǽð����� �������� �浹�� �����Ͽ� ���� ������ �ø��� ���� ���� ������ �׻� 1���� �ʱ�ȭ
        m_Laser.SetPosition(0, transform.position);
        float remainLength = m_DefaultLength;
        for (int i = 0; i < m_Reflections; i++)
        {
            if (Physics.Raycast(m_Ray.origin, m_Ray.direction, out m_CollidedObject, remainLength))//�浹�Ͽ��°�?
            {
                m_Laser.positionCount++;//�������� ������ ���� ���� ������ 1�� �ø�
                if ((m_CollidedObject.collider.tag == "Wall" || m_CollidedObject.collider.tag == "RotateWall" || m_CollidedObject.collider.tag == "AccessButton"))//ƨ��� �ִ� ���� �浹�Ͽ��°�?
                {    
                    if(m_CollidedObject.collider.tag == "AccessButton")
                        m_IsCollidedAccessButton = true;
                    m_Laser.SetPosition(m_Laser.positionCount - 1, m_CollidedObject.point);//�ε��� �� ���� ���� ��� �� ����
                    remainLength -= Vector3.Distance(m_Ray.origin, m_CollidedObject.point);//���� ���̸� ����
                    m_Reflect = Vector3.Reflect(m_Ray.direction, m_CollidedObject.normal);
                    m_IsReflected = true;
                    m_Ray = new Ray(m_CollidedObject.point, Vector3.Reflect(m_Ray.direction, m_CollidedObject.normal));//�ݻ簢 ���ϱ�
                }
                else if(m_CollidedObject.collider.tag == "Broken" || (m_CollidedObject.collider.tag == "EnergyWall" && m_IsCollidedAccessButton == false))//�Ѿ��� �ε������� �ı��Ǵ� ������Ʈ�� ��������                                    
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
                if (m_CollidedObject.collider.tag == "GasBarrel")
                {
                    m_HitGasBarrel = true;
                    GameObject[] gasbarrels = GameObject.FindGameObjectsWithTag("GasBarrel");
                    foreach (var gasbarrel in gasbarrels)
                    {
                        if (m_CollidedObject.transform == gasbarrel.transform)
                        {
                            GasBarrel currentgasbarrel = gasbarrel.GetComponent<GasBarrel>();
                            currentgasbarrel.HitLaser = true;
                        }
                    }
                }
                else
                {
                    if (!m_HitGasBarrel)
                    {
                        GameObject[] gasbarrels = GameObject.FindGameObjectsWithTag("GasBarrel");
                        foreach (var gasbarrel in gasbarrels)
                        {
                            GasBarrel currentgasbarrel = gasbarrel.GetComponent<GasBarrel>();
                            currentgasbarrel.HitLaser = false;
                        }
                    }
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

*/
