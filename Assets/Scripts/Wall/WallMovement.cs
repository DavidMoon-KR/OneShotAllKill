using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    // 최대 +이동할 수 있는 거리
    [SerializeField]
    private float m_MaxDirection;

    // 최소 -이동할 수 있는 거리
    [SerializeField]
    private float m_MinDirection;

    [SerializeField]
    private AudioClip m_HitArrowClip;
    private AudioSource m_HitSource;

    private Vector3 m_MouseP;

    private bool m_IsClick = false;
    void Start()
    {
        m_HitSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        m_IsClick = true;
    }
    private void OnMouseUp()
    {
        m_IsClick = false;
    }

    private void OnMouseDrag()
    {
        MoveUpdate();
    }
    // 벽 움직이기
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
    }
}
