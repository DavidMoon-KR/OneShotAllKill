using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float m_ShakeInstensity;
    private float m_ShakeTime;

    private static CameraShake m_Instance;
    public static CameraShake Instance => m_Instance;

    void Start()
    {
        m_Instance = FindObjectOfType<CameraShake>();
    }

    void Update()
    {

    }

    public void OnShakeCamera(float p_shakeTime, float p_shakeInstensity)
    {
        m_ShakeTime = p_shakeTime;
        m_ShakeInstensity = p_shakeInstensity;
        
        StartCoroutine(ShakeByPosition());
        StopCoroutine(ShakeByPosition());
    }

    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition = new Vector3(0f, 35f, 0f);

        while(m_ShakeTime > 0.0f)
        {
            transform.position = startPosition + Random.insideUnitSphere * m_ShakeInstensity;
            m_ShakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;

    }
}
