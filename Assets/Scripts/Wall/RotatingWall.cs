using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    private AudioSource _hitSource;
    [SerializeField]
    private AudioClip _hitWallClip;
    [SerializeField]
    private float _rotationAngle = 15.0f;

    private bool m_IsMousePress = false;
    private bool m_IsMouseCoroutineActive = false;

    void Start()
    {
        _hitSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        m_IsMousePress = true;
        if (!m_IsMouseCoroutineActive)
        {
            StartCoroutine(MouseDownDelay());
            m_IsMouseCoroutineActive = true;
        }
    }

    private void OnMouseUp()
    {
        m_IsMousePress = false;
    }

    public void Rotation()
    {
        _hitSource.clip = _hitWallClip;
        _hitSource.Play();
        this.gameObject.transform.Rotate(new Vector3(0, _rotationAngle, 0));
    }

    public IEnumerator MouseDownDelay()
    {
        while (m_IsMousePress)
        {
            Rotation();
            yield return new WaitForSeconds(0.15f);
        }
        m_IsMouseCoroutineActive = false;
    }
}