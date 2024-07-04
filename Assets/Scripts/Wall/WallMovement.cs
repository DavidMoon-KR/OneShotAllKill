using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        m_HitSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    // 벽 움직이기
    public void Move(int direction)
    {
        m_HitSource.clip = m_HitArrowClip;
        m_HitSource.Play();
        
        // 왼쪽 화살표 눌렀을 시, 왼쪽으로 2만큼 움직이기
        if (gameObject.transform.localPosition.x > m_MinDirection && direction == 0)
        {
            gameObject.transform.Translate(Vector3.left * 2, Space.Self);
        }
        // 오른쪽 화살표 눌렀을 시, 오른쪽으로 2만큼 움직이기
        else if(gameObject.transform.localPosition.x < m_MaxDirection && direction == 1)
        {
            gameObject.transform.Translate(Vector3.right * 2, Space.Self);
        }
    }
}
