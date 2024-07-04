using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int m_BulletCount;

    [SerializeField]
    private float m_FireRate = 0.5f;     // 연사속도 (초당 발사 횟수)
    private float m_NextFireTime = 0f;   // 다음 발사 시간

    // 탄 프리팹
    [SerializeField]
    private GameObject m_BulletPrefab;

    // 탄이 나오는 위치
    [SerializeField]
    private Transform m_BulletTransform;
    private UIManager m_UiManager;

    public List<GameObject> m_WallID; // 각각의 벽들이 갖고 있는 번호

    // 카메라 쉐이킹 효과 시간
    [SerializeField]
    private float m_ImpactTime;

    // 카메라 쉐이킹 효과 수치
    [SerializeField]
    private float m_ImpactGauge;

    [SerializeField]
    private AudioClip m_FireGenerated;
    private AudioSource m_AudioSource;

    private bool m_IsNoAmmo = false;

    void Start()
    {
        m_UiManager = FindObjectOfType<UIManager>();
        m_UiManager.BulletCountSet(m_BulletCount);
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = m_FireGenerated;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RotateTowards(targetPos);
        
        // 사격 개시
        FireBullet();
        
        // 장벽 클릭
        if(Input.GetMouseButtonDown(0))
        {
            ClickWall();
        }

        // 탄이 모두 떨어졌고, 탄 없음 상태가 아닌 경우
        if(m_BulletCount == 0 && m_IsNoAmmo == false)
        {
            // 탄 없음으로 상태 변경
            m_IsNoAmmo = true;

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

    void FireBullet()
    {
        if (Input.GetMouseButtonUp(1) && m_BulletCount != 0 && Time.time > m_NextFireTime)
        {
            // 현재 탄 개수에서 -1 차감
            m_BulletCount--;
            m_AudioSource.Play();

            // 현 탄 개수 UI에 표시
            m_UiManager.BulletCountSet(m_BulletCount);
            Instantiate(m_BulletPrefab, m_BulletTransform.transform.position, m_BulletTransform.rotation);
            m_NextFireTime = Time.time + m_FireRate;
            
            // 카메라 쉐이크 효과
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
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
                for (int i = 0; i < m_WallID.Count; i++)
                {
                    // 클릭한 대상이 벽인 경우
                    if (hitInfo.collider.gameObject == m_WallID[i])
                    {
                        // 화살표 표시
                        GameObject wall = hitInfo.collider.gameObject;
                        wall.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        wall.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    // 클릭한 대상이 벽이 아닌 경우
                    else
                    {
                        // 화살표 미표시
                        GameObject wall = m_WallID[i];
                        wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }

                }
            }
            // 오른쪽 화살표 클릭
            if (hitInfo.collider.name == "RightArrow")
            {
                 WallMovement wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                 wallMovement.Move(1);
            }
            // 왼쪽 화살표 클릭
            else if (hitInfo.collider.name == "LeftArrow")
            {
                WallMovement wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                wallMovement.Move(0);
            }

            // 회전벽 클릭
            if(hitInfo.collider != null && hitInfo.collider.tag == "RotateWall")
            {
                RotatingWall rotatingWall = hitInfo.collider.transform.gameObject.GetComponent<RotatingWall>();
                rotatingWall.Rotation();
            }
        }
    }
}
