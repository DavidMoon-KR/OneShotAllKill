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
        // �� ��ġ�� �ִ�Ÿ����� ª�� �ݴ��� ���� �ƴ� �����̸�, ������ �ʾ��� ���
        if(transform.position.x < _maxDir && _isTurned == true && _isStopped == false)
        {
            transform.Translate(Vector3.right * _movingSpeed * Time.deltaTime);
            _wheel.transform.Rotate(Vector3.back * _rotateSpeed * Time.deltaTime);
            
            // �� ��ġ�� �ִ�Ÿ����� ���ų� ����
            if(transform.position.x >= _maxDir)
            {
                // ���� ����
                _isTurned = false;
                StartCoroutine(TimetoStopped());
            }
        }
        // �� ��ġ�� �ּҰŸ����� ��� �ݴ��� ���� �ƴ� �����̸�, ������ �ʾ��� ���
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
    
    // �����ð� ���� �̵��� ����
    private IEnumerator TimetoStopped()
    {
        _isStopped = true;
        yield return new WaitForSeconds(_waitingTime);
        _isStopped = false;
    }
}
