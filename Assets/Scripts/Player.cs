using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int _bulletCount;

    [SerializeField]
    private float _fireRate = 0.5f;     // ����ӵ� (�ʴ� �߻� Ƚ��)
    private float _nextFireTime = 0f;   // ���� �߻� �ð�

    // ź ������
    [SerializeField]
    private GameObject _bulletPrefab;

    // ź�� ������ ��ġ
    [SerializeField]
    private Transform _bulletTransform;
    private UIManager _uiManager;

    public List<GameObject> _wallID; // ������ ������ ���� �ִ� ��ȣ

    // ī�޶� ����ŷ ȿ�� �ð�
    [SerializeField]
    private float _impactTime;

    // ī�޶� ����ŷ ȿ�� ��ġ
    [SerializeField]
    private float _impactGauge;

    [SerializeField]
    private AudioClip _fireGenerated;
    private AudioSource _audioSource;

    private bool _noAmmo = false;

    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.BulletCountSet(_bulletCount);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _fireGenerated;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RotateTowards(targetPos);
        
        // ��� ����
        FireBullet();
        
        // �庮 Ŭ��
        if(Input.GetMouseButtonDown(0))
        {
            ClickWall();
        }

        // ź�� ��� ��������, ź ���� ���°� �ƴ� ���
        if(_bulletCount == 0 && _noAmmo == false)
        {
            // ź �������� ���� ����
            _noAmmo = true;

            // ���ӸŴ������� ź ���ٴ� ���� �˸�
            GameManager.Instance._hasNotAmmo = true;
        }
    }

    //���콺 �����͸� �ٶ󺸴� �������� ȸ��
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
            // ���� ź �������� -1 ����
            _bulletCount--;
            _audioSource.Play();

            // �� ź ���� UI�� ǥ��
            _uiManager.BulletCountSet(_bulletCount);
            Instantiate(_bulletPrefab, _bulletTransform.transform.position, _bulletTransform.rotation);
            _nextFireTime = Time.time + _fireRate;
            
            // ī�޶� ����ũ ȿ��
            CameraShake.Instance.OnShakeCamera(_impactTime, _impactGauge);
        }
    }

    void ClickWall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider != null && hitInfo.collider.tag == "Wall")
            {
                for (int i = 0; i < _wallID.Count; i++)
                {
                    // Ŭ���� ����� ���� ���
                    if (hitInfo.collider.gameObject == _wallID[i])
                    {
                        // ȭ��ǥ ǥ��
                        GameObject _wall = hitInfo.collider.gameObject;
                        _wall.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        _wall.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    // Ŭ���� ����� ���� �ƴ� ���
                    else
                    {
                        // ȭ��ǥ ��ǥ��
                        GameObject _wall = _wallID[i];
                        _wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        _wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }

                }
            }
            // ������ ȭ��ǥ Ŭ��
            if (hitInfo.collider.name == "RightArrow")
            {
                 WallMovement _wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                _wallMovement.Move(1);
            }
            // ���� ȭ��ǥ Ŭ��
            else if (hitInfo.collider.name == "LeftArrow")
            {
                WallMovement _wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                _wallMovement.Move(0);
            }
            // ȸ���� Ŭ��
            if(hitInfo.collider != null && hitInfo.collider.tag == "RotateWall")
            {
                RotatingWall _rotatingWall = hitInfo.collider.transform.gameObject.GetComponent<RotatingWall>();
                _rotatingWall.Rotation();
            }
        }
    }
}
