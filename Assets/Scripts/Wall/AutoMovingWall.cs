using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovingWall : MonoBehaviour
{
    [SerializeField] private float _movingSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _minDir;
    [SerializeField] private float _maxDir;
    [SerializeField] private float _waitingTime;

    [SerializeField] private bool _isTurned = false;
    [SerializeField] private bool _isStopped = false;

    [SerializeField]
    private GameObject _wheel;

    void Start()
    {
        
    }

    void Update()
    {
        // 현 위치가 최대거리보다 짧고 반대쪽 턴이 아닌 상태이며, 멈추지 않았을 경우
        if(transform.position.x < _maxDir && _isTurned == true && _isStopped == false)
        {
            transform.Translate(Vector3.right * _movingSpeed * Time.deltaTime);
            _wheel.transform.Rotate(Vector3.back * _rotateSpeed * Time.deltaTime);
            
            // 현 위치가 최대거리보다 같거나 길경우
            if(transform.position.x >= _maxDir)
            {
                // 방향 유턴
                _isTurned = false;
                StartCoroutine(TimetoStopped());
            }
        }
        // 현 위치가 최소거리보다 길고 반대쪽 턴이 아닌 상태이며, 멈추지 않았을 경우
        else if (transform.position.x > _minDir && _isTurned == false && _isStopped == false)
        {
            transform.Translate(Vector3.left * _movingSpeed * Time.deltaTime);
            _wheel.transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
            if (transform.position.x <= _minDir)
            {
                _isTurned = true;
                StartCoroutine(TimetoStopped());
            }
        }
    }
    
    // 일정시간 동안 이동을 멈춤
    private IEnumerator TimetoStopped()
    {
        _isStopped = true;
        yield return new WaitForSeconds(_waitingTime);
        _isStopped = false;
    }
}
