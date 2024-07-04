using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        m_HitSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    // �� �����̱�
    public void Move(int direction)
    {
        m_HitSource.clip = m_HitArrowClip;
        m_HitSource.Play();
        
        // ���� ȭ��ǥ ������ ��, �������� 2��ŭ �����̱�
        if (gameObject.transform.localPosition.x > m_MinDirection && direction == 0)
        {
            gameObject.transform.Translate(Vector3.left * 2, Space.Self);
        }
        // ������ ȭ��ǥ ������ ��, ���������� 2��ŭ �����̱�
        else if(gameObject.transform.localPosition.x < m_MaxDirection && direction == 1)
        {
            gameObject.transform.Translate(Vector3.right * 2, Space.Self);
        }
    }
}
