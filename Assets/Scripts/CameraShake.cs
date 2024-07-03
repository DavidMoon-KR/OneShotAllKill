using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _shakeInstensity;
    private float _shakeTime;

    private static CameraShake _instance;
    public static CameraShake Instance => _instance;

    void Start()
    {
        _instance = FindObjectOfType<CameraShake>();
    }

    void Update()
    {

    }

    public void OnShakeCamera(float _shakeTime, float _shakeInstensity)
    {
        this._shakeTime = _shakeTime;
        this._shakeInstensity = _shakeInstensity;
        
        StartCoroutine(ShakeByPosition());
        StopCoroutine(ShakeByPosition());
    }

    private IEnumerator ShakeByPosition()
    {
        Vector3 _startPosition = new Vector3(0f, 35f, 0f);

        while(_shakeTime > 0.0f)
        {
            transform.position = _startPosition + Random.insideUnitSphere * _shakeInstensity;
            _shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = _startPosition;

    }
}
