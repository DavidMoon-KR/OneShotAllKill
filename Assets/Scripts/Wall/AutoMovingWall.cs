using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class AutoMovingWall : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    [SerializeField] private float m_MaxRightPos;
    [SerializeField] private float m_MaxLeftPos;

    private Vector3 m_MoveDirection = Vector3.right;

    private bool m_IsStopped = false;

    private NavMeshSurface m_NavMeshSurface;

    void Start()
    {
        if (null != GameObject.Find("Navigation"))
            m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();
    }

    void Update()
    {
        if (!m_IsStopped)
        {
            Moving();
        }
    }

    private void Moving()
    {
        transform.Translate(m_MoveDirection * m_Speed * Time.deltaTime);
        if (m_NavMeshSurface != null)
        {
            m_NavMeshSurface.BuildNavMesh();
        }

        if (transform.localPosition.x >= m_MaxRightPos)
        {
            StartCoroutine(TimetoStopped());
            m_MoveDirection = Vector3.left;
        }
        else if (transform.localPosition.x <= m_MaxLeftPos)
        {
            StartCoroutine(TimetoStopped());
            m_MoveDirection = Vector3.right;
        }
    }

    IEnumerator TimetoStopped()
    {
        m_IsStopped = true;
        yield return new WaitForSeconds(2.0f);
        m_IsStopped = false;
    }
}
