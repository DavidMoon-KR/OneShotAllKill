using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    //최대 +이동할 수 있는 거리
    [SerializeField]
    private float _maxDirection;
    //최소 -이동할 수 있는 거리
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

    //벽 움직이기
    public void Move(int direction)
    {
        _hitSource.clip = _hitArrowClip;
        _hitSource.Play();
        //왼쪽 화살표 눌렀을 시, 왼쪽으로 2만큼 움직이기
        if (gameObject.transform.localPosition.x > _minDirection && direction == 0)
        {
            gameObject.transform.Translate(Vector3.left * 2, Space.Self);
        }
        //오른쪽 화살표 눌렀을 시, 오른쪽으로 2만큼 움직이기
        else if(gameObject.transform.localPosition.x < _maxDirection && direction == 1)
        {
            gameObject.transform.Translate(Vector3.right * 2, Space.Self);
        }
    }
}
