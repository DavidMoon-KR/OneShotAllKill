using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovingWall : MonoBehaviour
{
    [SerializeField] private float m_MovingSpeed;
    [SerializeField] private float m_RotateSpeed;
    [SerializeField] private float m_MinDir;
    [SerializeField] private float m_MaxDir;
    [SerializeField] private float m_WaitingTime;

    [SerializeField] private bool m_IsTurned = false;
    [SerializeField] private bool m_IsStopped = false;

    [SerializeField]
    private GameObject m_Wheel;

    void Start()
    {
        
    }

    void Update()
    {
        // 현 위치가 최대거리보다 짧고 반대쪽 턴이 아닌 상태이며, 멈추지 않았을 경우
        if(transform.position.z < m_MaxDir && m_IsTurned == true && m_IsStopped == false)
        {
            transform.Translate(Vector3.left * m_MovingSpeed * Time.deltaTime);
            m_Wheel.transform.Rotate(Vector3.forward * m_RotateSpeed * Time.deltaTime);
            
            // 현 위치가 최대거리보다 같거나 길경우
            if(transform.position.z >= m_MaxDir)
            {
                // 방향 유턴
                m_IsTurned = false;
                StartCoroutine(TimetoStopped());
            }
        }
        // 현 위치가 최소거리보다 길고 반대쪽 턴이 아닌 상태이며, 멈추지 않았을 경우
        else if (transform.position.z > m_MinDir && m_IsTurned == false && m_IsStopped == false)
        {
            transform.Translate(Vector3.right * m_MovingSpeed * Time.deltaTime);
            m_Wheel.transform.Rotate(Vector3.back * m_RotateSpeed * Time.deltaTime);
            if (transform.position.z <= m_MinDir)
            {
                m_IsTurned = true;
                StartCoroutine(TimetoStopped());
            }
        }
    }
    
    // 일정시간 동안 이동을 멈춤
    private IEnumerator TimetoStopped()
    {
        m_IsStopped = true;
        yield return new WaitForSeconds(m_WaitingTime);
        m_IsStopped = false;
    }
}
