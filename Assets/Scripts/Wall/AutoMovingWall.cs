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
        // �� ��ġ�� �ִ�Ÿ����� ª�� �ݴ��� ���� �ƴ� �����̸�, ������ �ʾ��� ���
        if(transform.position.z < m_MaxDir && m_IsTurned == true && m_IsStopped == false)
        {
            transform.Translate(Vector3.left * m_MovingSpeed * Time.deltaTime);
            m_Wheel.transform.Rotate(Vector3.forward * m_RotateSpeed * Time.deltaTime);
            
            // �� ��ġ�� �ִ�Ÿ����� ���ų� ����
            if(transform.position.z >= m_MaxDir)
            {
                // ���� ����
                m_IsTurned = false;
                StartCoroutine(TimetoStopped());
            }
        }
        // �� ��ġ�� �ּҰŸ����� ��� �ݴ��� ���� �ƴ� �����̸�, ������ �ʾ��� ���
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
    
    // �����ð� ���� �̵��� ����
    private IEnumerator TimetoStopped()
    {
        m_IsStopped = true;
        yield return new WaitForSeconds(m_WaitingTime);
        m_IsStopped = false;
    }
}
