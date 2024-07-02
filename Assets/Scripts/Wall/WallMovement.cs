using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    //�ִ� +�̵��� �� �ִ� �Ÿ�
    [SerializeField]
    private float _maxDirection;
    //�ּ� -�̵��� �� �ִ� �Ÿ�
    [SerializeField]
    private float _minDirection;
    [SerializeField]
    private AudioClip _hitArrowClip;
    private AudioSource _hitSource;

    // Start is called before the first frame update
    void Start()
    {
        _hitSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�� �����̱�
    public void Move(int direction)
    {
        _hitSource.clip = _hitArrowClip;
        _hitSource.Play();
        //���� ȭ��ǥ ������ ��, �������� 2��ŭ �����̱�
        if (gameObject.transform.localPosition.x > _minDirection && direction == 0)
        {
            gameObject.transform.Translate(Vector3.left * 2, Space.Self);
        }
        //������ ȭ��ǥ ������ ��, ���������� 2��ŭ �����̱�
        else if(gameObject.transform.localPosition.x < _maxDirection && direction == 1)
        {
            gameObject.transform.Translate(Vector3.right * 2, Space.Self);
        }
    }
}
