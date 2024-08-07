using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.AI.Navigation;
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

    [SerializeField]
    private NavMeshSurface m_NavMeshSurface;

    void Start()
    {
        if (GetComponent<NavMeshSurface>() != null)
        {
            m_NavMeshSurface = GetComponent<NavMeshSurface>();
        }
        m_HitSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsActive)
        {
            m_IsClick = false;
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
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsActive)
        {
            return;
        }

        m_IsClick = true;
    }

    private void OnMouseUp()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsActive)
        {
            return;
        }

        m_IsClick = false;
        m_HitSource.clip = m_HitWallClip;
        m_HitSource.Play();
        //m_MoveT = false;

        if (m_NavMeshSurface != null)
        {
            m_NavMeshSurface.BuildNavMesh();
        }
    }
}
