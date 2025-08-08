using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private float m_RotateAngle;

    [SerializeField] private float m_WatingTime;
    [SerializeField] private int m_RotatingSpot;

    private bool m_isRotate = true;

    private float m_Timer;
    private int m_TurnCnt;

    private GameObject m_Cone;
    private Cone m_CurrentCone;
    private void Start()
    {
        m_TurnCnt = 0;
        m_Timer = 0.0f;
        m_Cone = GameObject.FindGameObjectWithTag("Cone");
        m_CurrentCone = m_Cone.GetComponent<Cone>();
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

    private IEnumerator RotationDown()
    {
        float f = 0.3f;
        while (f > 0)
        {
            f -= 0.005f;
            Quaternion tempRotation = transform.rotation;
            tempRotation.z -= 0.01f;
            transform.rotation = tempRotation;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void StartCoroutines()
    {
        StartCoroutine(m_CurrentCone.FadeOut());
        StartCoroutine(RotationDown());
        m_isRotate = false;
    }
}
