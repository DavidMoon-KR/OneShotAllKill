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
    private Player m_Player;
    private RaycastHit m_CollidedObject;
    private int m_Reflections = 10;//튕기는 횟수 ※이거를 수정해야 여러번 튕깁니다※
    private bool m_IsCollidedAccessButton;    
    private bool m_HitGasBarrel = false;
    private bool m_IsReflected;
    private Vector3 m_Reflect;
    private Ray m_Ray;
    private Vector3 m_Direction;

    private void Start()
    {
        m_Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        m_IsCollidedAccessButton = false;
        //레이저 초기 설정(색, 굵기) 설정
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
        //실시간으로 레이저의 충돌을 감지하여 선의 개수를 늘리기 위해 선의 개수를 항상 1개로 초기화
        m_Laser.SetPosition(0, transform.position);
        float remainLength = m_DefaultLength;
        for (int i = 0; i < m_Reflections; i++)
        {
            if (Physics.Raycast(m_Ray.origin, m_Ray.direction, out m_CollidedObject, remainLength))//충돌하였는가?
            {
                m_Laser.positionCount++;//레이저를 구현할 선의 점의 개수를 1개 늘림
                if ((m_CollidedObject.collider.tag == "Wall" || m_CollidedObject.collider.tag == "RotateWall" || m_CollidedObject.collider.tag == "AccessButton"))//튕길수 있는 벽과 충돌하였는가?
                {    
                    if(m_CollidedObject.collider.tag == "AccessButton")
                        m_IsCollidedAccessButton = true;
                    m_Laser.SetPosition(m_Laser.positionCount - 1, m_CollidedObject.point);//부딛힌 그 벽에 점을 찍어 선 생성
                    remainLength -= Vector3.Distance(m_Ray.origin, m_CollidedObject.point);//남은 길이를 구함
                    m_Reflect = Vector3.Reflect(m_Ray.direction, m_CollidedObject.normal);
                    m_IsReflected = true;
                    m_Ray = new Ray(m_CollidedObject.point, Vector3.Reflect(m_Ray.direction, m_CollidedObject.normal));//반사각 구하기
                }
                else if(m_CollidedObject.collider.tag == "Broken" || (m_CollidedObject.collider.tag == "EnergyWall" && m_IsCollidedAccessButton == false))//총알이 부딛혔을때 파괴되는 오브젝트를 만났을때                                    
                {
                    m_Laser.SetPosition(m_Laser.positionCount - 1, m_CollidedObject.point);//점을 찍어 선 생성                                              
                }                    
                else
                {
                    m_Laser.SetPosition(m_Laser.positionCount - 1, m_CollidedObject.point);//점을 찍어 선 생성                
                    remainLength -= Vector3.Distance(m_Ray.origin, m_CollidedObject.point);//남은 길이를 구함
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
            else//어디에도 부딛히지 않았을 경우
            {
                m_Laser.positionCount++;//레이저를 구현할 선의 점의 개수를 1개 늘림
                m_Laser.SetPosition(m_Laser.positionCount - 1, m_Ray.origin + (m_Ray.direction * remainLength));//레이저의 남은 거리만큼 레이저 생성
            }
            if ((int)m_Player.SelectBulletType == 1 && i == m_Reflections - 1)
                GameObject.FindWithTag("Player").GetComponent<EmpExplosionRange>().DrawEmpExplosionRange(true, m_Laser.GetPosition(m_Laser.positionCount - 1));            
            else
                GameObject.FindWithTag("Player").GetComponent<EmpExplosionRange>().DrawEmpExplosionRange(false, m_Laser.GetPosition(m_Laser.positionCount - 1));
        }
    }
}    

    /*void NormalLaser()//혹시 몰라서 남겨둔 튕기지 않는 레이저
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
