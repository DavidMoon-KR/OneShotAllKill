using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BulletType
{
    BulletType_Normal,
    BulletType_Emp,
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_FireRate = 0.5f;     // ����ӵ� (�ʴ� �߻� Ƚ��)
    private float m_NextFireTime = 0f;   // ���� �߻� �ð�

    // ź ������
    [SerializeField]
    private List<GameObject> m_BulletPrefab;

    // ���� ���õ� �Ѿ� Ÿ��
    private BulletType m_SelectBulletType = BulletType.BulletType_Normal;

    public List<int> m_BulletCount; // �� �Ѿ� ����
    private int m_BulletSum = 0;    // ��ü �Ѿ� ���� ��

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

    void Start()
    {
        m_UiManager = FindObjectOfType<UIManager>();
        m_UiManager.BulletCountSet(m_BulletCount[(int)m_SelectBulletType]);
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = m_FireGenerated;

        for (int i = 0; i < m_BulletCount.Count; i++)
        {
            m_BulletSum += m_BulletCount[i];
        }        
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RotateTowards(targetPos);

        // �Ѿ� ���� ����
        ChangeBulletType();

        // ��� ����
        FireBullet();

        // �庮 Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            ClickWall();
        }

        // ź�� ��� �������ٸ�
        if (m_BulletSum <= 0)
        {
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

    // �Ѿ� ���� ����
    void ChangeBulletType()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_SelectBulletType++;
            if (((int)m_SelectBulletType) >= m_BulletPrefab.Count)
            {
                m_SelectBulletType = BulletType.BulletType_Normal;
            }

            // �ٲ� �Ѿ� ���� UI�� ǥ��
            m_UiManager.BulletCountSet(m_BulletCount[(int)m_SelectBulletType]);
        }
    }

    void FireBullet()
    {
        if (Input.GetMouseButtonUp(1) && m_BulletCount[(int)m_SelectBulletType] != 0 && Time.time > m_NextFireTime)
        {
            m_AudioSource.Play();

            // ���� �Ѿ� �������� 1 ����
            // ��ü �Ѿ� �������� 1 ����
            m_BulletCount[(int)m_SelectBulletType]--;
            m_BulletSum--;

            // ���� �Ѿ� ���� UI�� ǥ��
            m_UiManager.BulletCountSet(m_BulletCount[(int)m_SelectBulletType]);
            Instantiate(m_BulletPrefab[(int)m_SelectBulletType], m_BulletTransform.transform.position, m_BulletTransform.rotation);
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
            if (hitInfo.collider != null && hitInfo.collider.tag == "RotateWall")
            {
                RotatingWall rotatingWall = hitInfo.collider.transform.gameObject.GetComponent<RotatingWall>();
                rotatingWall.Rotation();
            }
        }
    }
}
