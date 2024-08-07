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
    [SerializeField]
    private float m_MaxDirection;

    // �ּ� -�̵��� �� �ִ� �Ÿ�
    [SerializeField]
    private float m_MinDirection;

    [SerializeField]
    private AudioClip m_HitArrowClip;
    private AudioSource m_HitSource;

    private Vector3 m_MouseP;

    [SerializeField]
    private NavMeshSurface m_NavMeshSurface;

    private void Update()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsActive)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_IsClick = false;
            PrintArrow();
        }
    }

    private bool m_IsClick = false;
    void Start()
    {
        m_HitSource = GetComponent<AudioSource>();        
    }

    private void OnMouseDown()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsActive)
        {
            return;
        }

        m_IsClick = true;
        PrintArrow();
    }

    private void OnMouseDrag()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsActive)
        {
            return;
        }

        MoveUpdate();
    }
    // �� �����̱�
    public void MoveUpdate()
    {
        m_MouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        if (gameObject.transform.eulerAngles.y == 0)
        {
            if (gameObject.transform.localPosition.x > m_MinDirection && this.gameObject.transform.position.x - 2 > m_MouseP.x)
                WMove(true);
            else if (gameObject.transform.localPosition.x < m_MaxDirection && this.gameObject.transform.position.x + 2 < m_MouseP.x)
                WMove(false);
        }
        else
        {
            if (gameObject.transform.localPosition.x > m_MinDirection && this.gameObject.transform.position.z - 2 > m_MouseP.z)
                WMove(true);
            else if (gameObject.transform.localPosition.x < m_MaxDirection && this.gameObject.transform.position.z + 2 < m_MouseP.z)
                WMove(false);
        }
    }
    private void WMove(bool direction)
    {
        if (direction)
        {
            m_HitSource.clip = m_HitArrowClip;
            m_HitSource.Play();
            gameObject.transform.Translate(Vector3.left * 2, Space.Self);
        }
        else
        {
            m_HitSource.clip = m_HitArrowClip;
            m_HitSource.Play();
            gameObject.transform.Translate(Vector3.right * 2, Space.Self);
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


