using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    private AudioSource m_HitSource;
    [SerializeField]
    private AudioClip m_HitWallClip;
    [SerializeField]
    private float m_RotationAngle = 15.0f;

    void Start()
    {
        m_HitSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void Rotation()
    {
       m_HitSource.clip = m_HitWallClip;
        m_HitSource.Play();
        this.gameObject.transform.Rotate(new Vector3(0, m_RotationAngle, 0));
    }
}
