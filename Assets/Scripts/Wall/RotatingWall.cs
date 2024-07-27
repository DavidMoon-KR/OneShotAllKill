using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotatingWall : MonoBehaviour
{
    private AudioSource m_HitSource;
    [SerializeField]
    private AudioClip m_HitWallClip;

    [SerializeField]
    private bool m_IsClick = false;

    //private bool m_MoveT = false;

    [SerializeField]
    private Vector3 m_MouseP;

    private Vector3 FinalDir = Vector3.zero;

    void Start()
    {
        m_HitSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 튜토리얼이 진행중이라면 상호작용 불가
        if (Tutorial1.Instance != null && Tutorial1.Instance.m_IsActive)
        {
            return;
        }

        if (m_IsClick)
        {
            m_MouseP = Input.mousePosition;
            //m_MouseP.z = Camera.main.transform.position.z;

            Vector3 p_target = Camera.main.ScreenToWorldPoint(m_MouseP);

            Vector3 tmpPosition = transform.position;
            tmpPosition.y = 0.0f;
            p_target.y = 0.0f;

            Vector3 mouseDirection = (p_target - transform.position);
            mouseDirection.y = 0;
            mouseDirection.Normalize();

            float scala = Vector3.Dot(transform.forward, mouseDirection);
            float angle = Mathf.Acos(scala);

            Vector3 axis = Vector3.Cross(transform.forward, mouseDirection);
            Vector3 Dir = Quaternion.AngleAxis(angle, axis) * transform.forward;
            Dir.Normalize();
            transform.rotation = Quaternion.LookRotation(Dir);


            //if (angle >= (30f * Mathf.Deg2Rad) && m_MoveT == false)
            //{
            //    m_MoveT = true;
            //}
            //else
            //{
            //    //MoveT = transform.forward != FinalDir;

            //}

            //if (m_MoveT)
            //{
            //    Vector3 axis = Vector3.Cross(transform.forward, mouseDirection);
            //    Vector3 Dir = Quaternion.AngleAxis(angle, axis) * transform.forward;
            //    Dir.Normalize();
            //    transform.rotation = Quaternion.LookRotation(Dir);
            //}
        }
    }

    private void OnMouseDown()
    {
        m_IsClick = true;
    }

    private void OnMouseUp()
    {
        m_IsClick = false;
        m_HitSource.clip = m_HitWallClip;
        m_HitSource.Play();
        //m_MoveT = false;
    }
}
