using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float m_ShakeInstensity;
    private float m_ShakeTime;

    private static CameraShake m_Instance;
    public static CameraShake Instance => m_Instance;

    private bool m_IsShake = true;

    void Start()
    {
        m_Instance = FindObjectOfType<CameraShake>();

        m_IsShake = GameDataManager.Instance.Data.CameraShakeEnabled;
    }

    public void OnShakeCamera(float p_shakeTime, float p_shakeInstensity)
    {
        if (!m_IsShake) return;

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
            m_ShakeTime -= Time.unscaledDeltaTime;

            yield return null;
        }

        transform.position = startPosition;

    }
}
