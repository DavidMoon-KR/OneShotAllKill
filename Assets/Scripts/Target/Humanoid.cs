using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    private static Humanoid _instance;
    public static Humanoid Instance => _instance;

    [SerializeField]
    private List<Transform> _wayPoint;

    [SerializeField]
    private int _currentTarget = 0;

    [SerializeField]
    private float _waitBeforeMoving;
    [SerializeField]
    private float _impactTime;
    [SerializeField]
    private float _impactGauge;
    private float _distance;

    [SerializeField]
    private GameObject _explosion;

    private bool _targetReached = false;
    private bool _isReturned = false;
    //폭발 감지를 딱 한번만 실행하도록 하기 위한 장치
    private bool _triggerExplosion = false;

    public NavMeshAgent _agent;

    private bool _isTriggerAni = true;
    private Animator _anim;

    public bool _explosionDetection = false;

    public Vector3 _explosionedPos;

    // Start is called before the first frame update
    void Start()
    {
        _instance = gameObject.GetComponent<Humanoid>();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //웨이포인트가 있다면
        if (_wayPoint.Count > 0)
        {
            //애니메이션 작동
            if(_isTriggerAni == true)
            {
                _anim.SetBool("Walk", true);
            }
            //설정한 웨이포인트로 이동
            _agent.SetDestination(_wayPoint[_currentTarget].position);
            //자신의 위치와 웨이포인트 위치 사이의 거리를 구함
            _distance = Vector3.Distance(transform.position, _wayPoint[_currentTarget].position);
            //거리가 1.0 미만이라면
            if (_distance < 1.0f && _targetReached == false)
            {
                _targetReached = true;
                _anim.SetBool("Walk", false);
                _isTriggerAni = false;
                StartCoroutine(WaitBeforeMoving());
            }
        }
        else
        {
            if(_explosionDetection == true)
            {
                _anim.SetBool("Walk", true);
                float _distance = Vector3.Distance(transform.position, _explosionedPos);
                _agent.SetDestination(_explosionedPos);
                if (_distance < 3.3f)
                {
                    _anim.SetBool("Walk", false);
                    //오브젝트 흔들림 버그 방지를 위해 작성함
                    Rigidbody rb = GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.isKinematic = false;
                    _explosionDetection = false;
                }
            }
        }
    }
    //이동 전에 기다리기
    private IEnumerator WaitBeforeMoving()
    {
        //현 웨이포인트 차례가 마지막이라면?
        if(_currentTarget == 0 || _currentTarget == _wayPoint.Count - 1)
        {
            //대기
            yield return new WaitForSeconds(_waitBeforeMoving);
            if(_currentTarget == 0)
            {
                //방향 전환
                _isReturned = false;
            }
            else
            {
                //방향 전환
                _isReturned = true;
            }
        }
        //방향 전환 연산
        if(_isReturned == true)
        {
            _currentTarget--;
        }
        else
        {
            _currentTarget++;
        }

        _isTriggerAni = true;
        _targetReached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //탄과 충돌하거나 가스폭발과 충돌할 경우
        if((other.tag == "Bullet" || other.tag == "GasExplosion") && _triggerExplosion == false)
        {
            //폭발 상태 true로 전환
            _triggerExplosion = true;
            //게임 매니저에서 현 스테이지에 타겟 수 감소
            GameManager.Instance._targets--;
            //탄과 충돌할 경우 _hasBullet에 true 전달
            if(other.tag == "Bullet")
            {
                StartCoroutine(ExplosionDelay(true));
            }
            else
            {
                StartCoroutine(ExplosionDelay(false));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //에너지 방벽과 충돌한 경우
        if(collision.gameObject.CompareTag("EnergyWall"))
        {
            StartCoroutine(StopMoving());
        }
    }
    //이동 멈춤
    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.5f);
        //SetDestination 작동 끄기
        _distance = 0;
        _agent.ResetPath();
        _anim.SetBool("Walk", false);
        _explosionDetection = false;
    }

    //폭발 딜레이
    private IEnumerator ExplosionDelay(bool _hasBullet)
    {
        //탄과 충돌한 경우
        if(_hasBullet == true)
        {
            yield return new WaitForSeconds(GameManager.Instance._delayExplosion);
        }
        //가스 폭발과 충돌한 경우
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        Instantiate(_explosion, transform.position, Quaternion.identity);
        //폭발하기 전 몸을 사라지게 한다.
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        CameraShake.Instance.OnShakeCamera(_impactTime, _impactGauge);
        Destroy(gameObject);
    }
}
