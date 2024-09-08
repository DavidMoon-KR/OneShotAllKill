using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum BulletType
{
    BulletType_Normal,
    BulletType_Emp,
}

public class Player : MonoBehaviour
{
    [SerializeField] private float m_FireRate = 0.5f; // 연사속도 (초당 발사 횟수)
    private float m_NextFireTime = 0f;   // 다음 발사 시간

    [SerializeField] private List<GameObject> m_BulletPrefab;   // 탄 프리팹
    [SerializeField] private Transform m_BulletTransform;       // 탄이 나오는 위치

    private BulletType m_SelectBulletType = BulletType.BulletType_Normal; // 현재 선택된 총알 타입
    private int m_BulletSum = 0;    // 전체 총알 개수 합
    public List<int> m_BulletCount; // 각 총알 개수    

    // 카메라 쉐이킹
    [SerializeField] private float m_ImpactTime;    // 카메라 쉐이킹 효과 시간
    [SerializeField] private float m_ImpactGauge;   // 카메라 쉐이킹 효과 수치

    // 사운드
    [SerializeField] private AudioClip m_FireGenerated;
    private AudioSource m_AudioSource;

    // 플레이어 스크립트를 인스턴스화 한 것
    private static Player m_Instance;
    public static Player Instance => m_Instance;

    // 프로퍼티
    public int BulletSum { get => m_BulletSum; set => m_BulletSum = value; }

    void Start()
    {
        m_Instance = GetComponent<Player>();
        UIManager.Instance.BulletCountSet(m_BulletCount[(int)m_SelectBulletType]);
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = m_FireGenerated;

        for (int i = 0; i < m_BulletCount.Count; i++)
        {
            m_BulletSum += m_BulletCount[i];
        }        
    }

    void Update()
    {
        // 튜토리얼이 진행중이라면 상호작용 불가
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RotateTowards(targetPos);

        // 총알 종류 변경
        ChangeBulletType();

        // 사격 개시
        FireBullet();

        // 탄이 모두 떨어졌다면
        if (m_BulletSum <= 0)
        {
            // 게임매니저에게 탄 없다는 것을 알림
            GameManager.Instance.m_HasNotAmmo = true;
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

    // 총알 종류 변경
    void ChangeBulletType()
    {
        // 만약 바꿀 총이 없다면 바꾸지 못하므로 바로 종료
        if (1 >= m_BulletPrefab.Count)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_SelectBulletType++;
            if (((int)m_SelectBulletType) >= m_BulletPrefab.Count)
            {
                m_SelectBulletType = BulletType.BulletType_Normal;
            }

            // 바뀐 총 UI에 표시
            UIManager.Instance.BulletSpriteChange(m_SelectBulletType);

            // 바뀐 총알 개수 UI에 표시
            UIManager.Instance.BulletCountSet(m_BulletCount[(int)m_SelectBulletType]);
        }
    }

    void FireBullet()
    {
        if (Input.GetMouseButtonUp(0) && m_BulletCount[(int)m_SelectBulletType] != 0 && Time.time > m_NextFireTime)
        {
            m_AudioSource.Play();

            // 현재 총알 개수에서 1 차감
            // 전체 총알 개수에서 1 차감
            m_BulletCount[(int)m_SelectBulletType]--;
            m_BulletSum--;

            // 현재 총알 개수 UI에 표시
            UIManager.Instance.BulletCountSet(m_BulletCount[(int)m_SelectBulletType]);
            Instantiate(m_BulletPrefab[(int)m_SelectBulletType], m_BulletTransform.transform.position, m_BulletTransform.rotation);
            m_NextFireTime = Time.time + m_FireRate;

            // 카메라 쉐이크 효과
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
        }
    }
}
