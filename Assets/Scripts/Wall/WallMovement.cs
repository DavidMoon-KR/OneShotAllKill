using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    // �ִ� +�̵��� �� �ִ� �Ÿ�
    [SerializeField] private float m_MaxDirection;

    // �ּ� -�̵��� �� �ִ� �Ÿ�
    [SerializeField] private float m_MinDirection;

    // ����
    [SerializeField] private AudioClip m_HitArrowClip;
    private AudioSource m_HitSource;

    private bool m_EnterMouse = false;
    private Vector3 m_MouseP;
    private float m_Walldistance = 3.9f;//�� ������ �Ÿ�
    private NavMeshSurface m_NavMeshSurface;
    RaycastHit m_RaycastHit;
    private float m_tempF;
    private void Update()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        if(Input.GetMouseButtonDown(1) && m_EnterMouse)
        {
            MouseRDown();
        }

        if (Input.GetMouseButtonUp(1))
        {
            m_EnterMouse = false;
            m_IsClick = false;
            PrintArrow();
        }
        if (m_EnterMouse && m_IsClick)
            MoveUpdate();
    }

    private bool m_IsClick = false;
    void Start()
    {
        if (null != GameObject.Find("Navigation"))
            m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();
        m_HitSource = GetComponent<AudioSource>();
    }

    private void OnMouseEnter()
    {
        m_EnterMouse = true;
    }
    private void OnMouseExit()
    {
        if(m_IsClick == false)
            m_EnterMouse=false;
    }

    private void MouseRDown()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        m_IsClick = true;
        PrintArrow();
    }

    // �� �����̱�
    private void MoveUpdate()
    {
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }
        m_MouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        if ((gameObject.transform.eulerAngles.y == 0 && this.gameObject.transform.position.x + 2 < m_MouseP.x) ||
            (gameObject.transform.eulerAngles.y == 270 && this.gameObject.transform.position.z + 2 < m_MouseP.z))
        {
            Debug.DrawRay(transform.position, transform.right * m_Walldistance * 2, UnityEngine.Color.clear, 0.3f);
            if (Physics.Raycast(transform.position, transform.right, out m_RaycastHit, m_Walldistance*2))
            {
                if (m_RaycastHit.transform.CompareTag("RotateWall"))
                {
                    if (Vector3.Distance(m_RaycastHit.transform.position, transform.position) < m_Walldistance * 2)
                        return;
                }
            }
            Debug.DrawRay(transform.position, transform.right * m_Walldistance, UnityEngine.Color.clear, 0.3f);
            if (!Physics.Raycast(transform.position, transform.right, out m_RaycastHit, m_Walldistance))
            {
                m_HitSource.clip = m_HitArrowClip;
                m_HitSource.Play();
                gameObject.transform.Translate(Vector3.right * 2, Space.Self);
            }
        }
        else if ((gameObject.transform.eulerAngles.y == 0 && this.gameObject.transform.position.x - 2 > m_MouseP.x) ||
            (gameObject.transform.eulerAngles.y == 270 && this.gameObject.transform.position.z - 2 > m_MouseP.z))
        {
            Debug.DrawRay(transform.position, transform.right * -1 * m_Walldistance * 2, UnityEngine.Color.clear, 0.3f);
            if (Physics.Raycast(transform.position, transform.right * -1, out m_RaycastHit, m_Walldistance*2))
            {
                if (m_RaycastHit.transform.CompareTag("RotateWall"))
                {
                    if (Vector3.Distance(m_RaycastHit.transform.position, transform.position) < m_Walldistance * 2)
                        return;
                }
            }
            Debug.DrawRay(transform.position, transform.right * -1 * m_Walldistance, UnityEngine.Color.clear, 0.3f);
            if (!Physics.Raycast(transform.position, transform.right * -1, out m_RaycastHit, m_Walldistance))
            {
                m_HitSource.clip = m_HitArrowClip;
                m_HitSource.Play();
                gameObject.transform.Translate(Vector3.left * 2, Space.Self);
            }
        }
        if (m_NavMeshSurface != null)
        {
            m_NavMeshSurface.BuildNavMesh();
        }
    }
private void PrintArrow()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject wall = hitInfo.collider.gameObject;
            if (m_IsClick == true)
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(m_IsClick);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(m_IsClick);
            }
            else
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(m_IsClick);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(m_IsClick);
            }
        }
    }
}


