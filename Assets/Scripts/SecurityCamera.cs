using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField]
    private float m_RotateAngle;
    private bool m_isRotate = true;
    [SerializeField]
    private float m_WatingTime;
    [SerializeField]
    private int m_RotatingSpot;
    private float m_Timer;
    private int m_TurnCnt;

    private void Start()
    {
        m_TurnCnt = 0;
        m_Timer = 0.0f;
    }

    void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_isRotate && m_Timer > m_WatingTime)
        {
            transform.Rotate(0, m_RotateAngle * Time.deltaTime, 0);
            if (m_Timer > m_WatingTime + 1 && m_RotatingSpot != 0)
            {
                m_Timer = 0.0f;
                if(m_RotateAngle*m_RotatingSpot < 360)
                {
                    m_TurnCnt++;
                    if (m_TurnCnt == m_RotatingSpot - 1)
                        m_RotateAngle *= -1;
                    else if (m_TurnCnt == 2 * (m_RotatingSpot - 1))
                    {
                        m_RotateAngle = Mathf.Abs(m_RotateAngle);
                        m_TurnCnt = 0;
                    }
                }                                 
            }
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
