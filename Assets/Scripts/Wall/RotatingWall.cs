using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    private AudioSource _hitSource;
    [SerializeField]
    private AudioClip _hitWallClip;
    [SerializeField]
    private float _rotationAngle = 15.0f;

    private bool m_EnterMouse = false;
    private bool m_IsMousePress = false;
    private bool m_IsMouseCoroutineActive = false;

    private NavMeshSurface m_NavMeshSurface;

    void Start()
    {
        if (null != GameObject.Find("Navigation"))
            m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();

        _hitSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(m_EnterMouse)
        {
            if (Input.GetMouseButtonDown(1))
                MouseRButtonDown();
            else if (Input.GetMouseButtonUp(1))
                MouseRButtonUp();
        }
    }
    private void OnMouseEnter()
    {
        m_EnterMouse = true;
    }
    private void OnMouseExit()
    {
        m_EnterMouse = false;
    }

    private void MouseRButtonDown()
    {
        m_IsMousePress = true;
        if (!m_IsMouseCoroutineActive)
        {
            StartCoroutine(MouseDownDelay());
            m_IsMouseCoroutineActive = true;
        }
    }

    private void MouseRButtonUp()
    {
        m_IsMousePress = false;
    }

    public void Rotation()
    {
        _hitSource.clip = _hitWallClip;
        _hitSource.Play();
        this.gameObject.transform.Rotate(new Vector3(0, _rotationAngle, 0));

        if (m_NavMeshSurface != null)
        {
            m_NavMeshSurface.BuildNavMesh();
        }
    }

    public IEnumerator MouseDownDelay()
    {
        while (m_IsMousePress && m_EnterMouse)
        {
            Rotation();
            yield return new WaitForSeconds(0.15f);
        }
        m_IsMouseCoroutineActive = false;
    }
}