using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int m_BulletCount;

    [SerializeField]
    private float m_FireRate = 0.5f;     // ����ӵ� (�ʴ� �߻� Ƚ��)
    private float m_NextFireTime = 0f;   // ���� �߻� �ð�

    // ź ������
    [SerializeField]
    private GameObject m_BulletPrefab;

    // ź�� ������ ��ġ
    [SerializeField]
    private Transform m_BulletTransform;
    private UIManager m_UiManager;

    public List<GameObject> m_WallID; // ������ ������ ���� �ִ� ��ȣ

    // ī�޶� ����ŷ ȿ�� �ð�
    [SerializeField]
    private float m_ImpactTime;

    // ī�޶� ����ŷ ȿ�� ��ġ
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
        
        // ��� ����
        FireBullet();
        
        // �庮 Ŭ��
        if(Input.GetMouseButtonDown(0))
        {
            ClickWall();
        }

        // ź�� ��� ��������, ź ���� ���°� �ƴ� ���
        if(m_BulletCount == 0 && m_IsNoAmmo == false)
        {
            // ź �������� ���� ����
            m_IsNoAmmo = true;

            // ���ӸŴ������� ź ���ٴ� ���� �˸�
            GameManager.Instance.m_HasNotAmmo = true;
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
        if (Input.GetMouseButtonUp(1) && m_BulletCount != 0 && Time.time > m_NextFireTime)
        {
            // ���� ź �������� -1 ����
            m_BulletCount--;
            m_AudioSource.Play();

            // �� ź ���� UI�� ǥ��
            m_UiManager.BulletCountSet(m_BulletCount);
            Instantiate(m_BulletPrefab, m_BulletTransform.transform.position, m_BulletTransform.rotation);
            m_NextFireTime = Time.time + m_FireRate;
            
            // ī�޶� ����ũ ȿ��
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
                    // Ŭ���� ����� ���� ���
                    if (hitInfo.collider.gameObject == m_WallID[i])
                    {
                        // ȭ��ǥ ǥ��
                        GameObject wall = hitInfo.collider.gameObject;
                        wall.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        wall.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    // Ŭ���� ����� ���� �ƴ� ���
                    else
                    {
                        // ȭ��ǥ ��ǥ��
                        GameObject wall = m_WallID[i];
                        wall.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        wall.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }

                }
            }
            // ������ ȭ��ǥ Ŭ��
            if (hitInfo.collider.name == "RightArrow")
            {
                 WallMovement wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                 wallMovement.Move(1);
            }
            // ���� ȭ��ǥ Ŭ��
            else if (hitInfo.collider.name == "LeftArrow")
            {
                WallMovement wallMovement = hitInfo.collider.transform.parent.gameObject.GetComponent<WallMovement>();
                wallMovement.Move(0);
            }

            // ȸ���� Ŭ��
            if(hitInfo.collider != null && hitInfo.collider.tag == "RotateWall")
            {
                RotatingWall rotatingWall = hitInfo.collider.transform.gameObject.GetComponent<RotatingWall>();
                rotatingWall.Rotation();
            }
        }
    }
}
