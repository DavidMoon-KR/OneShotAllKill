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
    //���� ������ �� �ѹ��� �����ϵ��� �ϱ� ���� ��ġ
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
        //��������Ʈ�� �ִٸ�
        if (_wayPoint.Count > 0)
        {
            //�ִϸ��̼� �۵�
            if(_isTriggerAni == true)
            {
                _anim.SetBool("Walk", true);
            }
            //������ ��������Ʈ�� �̵�
            _agent.SetDestination(_wayPoint[_currentTarget].position);
            //�ڽ��� ��ġ�� ��������Ʈ ��ġ ������ �Ÿ��� ����
            _distance = Vector3.Distance(transform.position, _wayPoint[_currentTarget].position);
            //�Ÿ��� 1.0 �̸��̶��
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
                    //������Ʈ ��鸲 ���� ������ ���� �ۼ���
                    Rigidbody rb = GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.isKinematic = false;
                    _explosionDetection = false;
                }
            }
        }
    }
    //�̵� ���� ��ٸ���
    private IEnumerator WaitBeforeMoving()
    {
        //�� ��������Ʈ ���ʰ� �������̶��?
        if(_currentTarget == 0 || _currentTarget == _wayPoint.Count - 1)
        {
            //���
            yield return new WaitForSeconds(_waitBeforeMoving);
            if(_currentTarget == 0)
            {
                //���� ��ȯ
                _isReturned = false;
            }
            else
            {
                //���� ��ȯ
                _isReturned = true;
            }
        }
        //���� ��ȯ ����
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
        //ź�� �浹�ϰų� �������߰� �浹�� ���
        if((other.tag == "Bullet" || other.tag == "GasExplosion") && _triggerExplosion == false)
        {
            //���� ���� true�� ��ȯ
            _triggerExplosion = true;
            //���� �Ŵ������� �� ���������� Ÿ�� �� ����
            GameManager.Instance._targets--;
            //ź�� �浹�� ��� _hasBullet�� true ����
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
        //������ �溮�� �浹�� ���
        if(collision.gameObject.CompareTag("EnergyWall"))
        {
            StartCoroutine(StopMoving());
        }
    }
    //�̵� ����
    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.5f);
        //SetDestination �۵� ����
        _distance = 0;
        _agent.ResetPath();
        _anim.SetBool("Walk", false);
        _explosionDetection = false;
    }

    //���� ������
    private IEnumerator ExplosionDelay(bool _hasBullet)
    {
        //ź�� �浹�� ���
        if(_hasBullet == true)
        {
            yield return new WaitForSeconds(GameManager.Instance._delayExplosion);
        }
        //���� ���߰� �浹�� ���
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        Instantiate(_explosion, transform.position, Quaternion.identity);
        //�����ϱ� �� ���� ������� �Ѵ�.
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        CameraShake.Instance.OnShakeCamera(_impactTime, _impactGauge);
        Destroy(gameObject);
    }
}
