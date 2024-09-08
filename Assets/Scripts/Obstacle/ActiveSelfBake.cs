using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ActiveSelfBake : MonoBehaviour
{
    private NavMeshSurface m_NavMeshSurface;

    void Start()
    {
        if (null != GameObject.Find("Navigation"))
            m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();
    }

    private void OnEnable()
    {
        if (m_NavMeshSurface != null)
        {
            new WaitForSeconds(0.5f);
            m_NavMeshSurface.BuildNavMesh();         
        }
    }

    private void OnDisable()
    {
        if (m_NavMeshSurface != null)
        {
            m_NavMeshSurface.BuildNavMesh();
        }
    }
}

