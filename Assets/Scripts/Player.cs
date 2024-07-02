using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int _bulletCount;

    [SerializeField]
    private float _fireRate = 0.5f; // 연사속도 (초당 발사 횟수)
    private float _nextFireTime = 0f; // 다음 발사 시간

    [SerializeField]
    private GameObject _bulletPrefab; //탄 프리팹
    [SerializeField]
    private Transform _bulletTransform; //탄이 나오는 위치
    private UIManager _uiManager;

    public List<GameObject> _wallID; //각각의 벽들이 갖고 있는 번호
    [SerializeField]
    private GameManager _gameManager; //게임매니저 스크립트

    [SerializeField]
    private float _impactTime; // 카메라 쉐이킹 효과 시간
    [SerializeField]
    private float _impactGauge; // 카메라 쉐이킹 효과 수치
    [SerializeField]
    private AudioClip _fireGenerated;
    private AudioSource _audioSource;

    private bool _noAmmo = false;

    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.BulletCountSet(_bulletCount);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _fireGenerated;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RotateTowards(targetPos);
        //사격 개시
        FireBullet();
        //장벽 클릭
        if(Input.GetMouseButtonDown(0))
        {
            ClickWall();
        }
        //탄이 모두 떨어졌고, 탄 없음 상태가 아닌 경우
        if(_bulletCount == 0 && _noAmmo == false)
        {
            //탄 없음으로 상태 변경
            _noAmmo = true;
            //게임매니저에게 탄 없다는 것을 알림
            _gameManager._hasNotAmmo = true;
        }
    }

    //마우스 포인터를 바라보는 방향으로 회전
    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        targetRotation.x = transform.rotation.x;
        targetRotation.z = transform.rotation.z;

        transform.rotation = targetRotation;
    }

    void FireBullet()
    {
        if (Input.GetMouseButtonUp(1) && _bulletCount != 0 && Time.time > _nextFireTime)
        {
            //현재 탄 개수에서 -1 차감
            _bulletCount--;
            _audioSource.Play();
            //현 탄 개수 UI에 표시
            _uiManager.BulletCountSet(_bulletCount);
            Instantiate(_bulletPrefab, _bulletTransform.transform.position, _bulletTransform.rotation);
            _nextFireTime = Time.time + _fireRate;
            //카메라 쉐이크 효과
            CameraShake.Instance.OnShakeCamera(_impactTime, _impactGauge);
        }
    }

    void ClickWall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo))
        {
            //클릭한 대상이 벽인 경우
            if (hitInfo.collider != null && hitInfo.collider.tag == "Wall")
            {
                for (int i = 0; i < _wallID.Count; i++)
                {
                    if (hitInfo.collider.gameObject == _wallID[i])
                    {
                        //화살표 표시
                        GameObject _wall = hitInfo.collider.gameObject;
                        _wall.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        _wall.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    //클릭한 대상이 벽이 아닌 경우
                    else
                    {
                        //화살표 미표시
                        GameObject _wall = _wallID[i];
                        _wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        _wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }

                }
            }
            //오른쪽 화살표 클릭
            if (hitInfo.collider.name == "RightArrow")
            {
                 WallMovement _wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                _wallMovement.Move(1);
            }
            //왼쪽 화살표 클릭
            else if (hitInfo.collider.name == "LeftArrow")
            {
                WallMovement _wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                _wallMovement.Move(0);
            }
            //회전벽 클릭
            if(hitInfo.collider != null && hitInfo.collider.tag == "RotateWall")
            {
                RotatingWall _rotatingWall = hitInfo.collider.transform.gameObject.GetComponent<RotatingWall>();
                _rotatingWall.Rotation();
            }
        }
    }
}
