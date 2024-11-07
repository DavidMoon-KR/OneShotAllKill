using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public enum BulletType
{
    BulletType_Normal,
    BulletType_Emp
}

public class Player : MonoBehaviour
{
    [SerializeField] private float m_FireRate = 0.5f; // ����ӵ� (�ʴ� �߻� Ƚ��)
    private float m_NextFireTime = 0f;   // ���� �߻� �ð�

    [SerializeField] private List<GameObject> m_BulletPrefab;   // ź ������
    [SerializeField] private Transform m_BulletTransform;       // ź�� ������ ��ġ

    private BulletType m_SelectBulletType = BulletType.BulletType_Normal; // ���� ���õ� �Ѿ� Ÿ��
    private int m_BulletSum = 0;    // ��ü �Ѿ� ���� ��
    public List<int> m_BulletCount; // �� �Ѿ� ����    

    // ī�޶� ����ŷ
    [SerializeField] private float m_ImpactTime;    // ī�޶� ����ŷ ȿ�� �ð�
    [SerializeField] private float m_ImpactGauge;   // ī�޶� ����ŷ ȿ�� ��ġ

    // ����
    [SerializeField] private AudioClip m_FireGenerated;
    private AudioSource m_AudioSource;

    // �÷��̾� ��ũ��Ʈ�� �ν��Ͻ�ȭ �� ��
    private static Player m_Instance;
    public static Player Instance => m_Instance;

    // ������Ƽ
    public int BulletSum { get => m_BulletSum; set => m_BulletSum = value; }
    public BulletType SelectBulletType { get { return m_SelectBulletType; } }

    void Start()
    {
        m_Instance = GetComponent<Player>();
        UIManager.Instance.AllBulletCountSet(m_SelectBulletType, m_BulletCount);
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = m_FireGenerated;

        for (int i = 0; i < m_BulletCount.Count; i++)
        {
            m_BulletSum += m_BulletCount[i];
        }
    }

    void Update()
    {
        // Ʃ�丮���� �������̶�� ��ȣ�ۿ� �Ұ�
        // ������ �Ͻ������� ��Ȳ������ �ൿ �Ұ�
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RotateTowards(targetPos);

        // �Ѿ� ���� ����
        ChangeBulletType();

        // ��� ����
        FireBullet();
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
        // ���� �ٲ� ���� ���ٸ� �ٲ��� ���ϹǷ� �ٷ� ����
        if (1 >= m_BulletPrefab.Count)
            return;

        // ���� Ʃ�丮���� �ִٸ� ������ Ÿ�ֿ̹� �ٲ� �� �ֵ��� ��ġ
        if (TutorialManager.Instance != null && TutorialManager.Instance.IsTutorial)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_SelectBulletType++;
            if (((int)m_SelectBulletType) >= m_BulletPrefab.Count)
            {
                m_SelectBulletType = BulletType.BulletType_Normal;
            }

            // �ٲ� �� UI�� ǥ��
            UIManager.Instance.RifleInfoSpriteChange(m_SelectBulletType, m_BulletCount);
        }
    }

    void FireBullet()
    {
        if (Input.GetMouseButtonUp(0) && m_BulletCount[(int)m_SelectBulletType] != 0 && Time.time > m_NextFireTime
            && !EventSystem.current.IsPointerOverGameObject())
        {
            m_AudioSource.Play();

            // ���� �Ѿ� �������� 1 ����
            // ��ü �Ѿ� �������� 1 ����
            m_BulletCount[(int)m_SelectBulletType]--;
            m_BulletSum--;

            // ���� �Ѿ� ���� UI�� ǥ��
            UIManager.Instance.BulletCountSet(m_BulletCount[(int)m_SelectBulletType]);
            Instantiate(m_BulletPrefab[(int)m_SelectBulletType], m_BulletTransform.transform.position, m_BulletTransform.rotation);
            m_NextFireTime = Time.time + m_FireRate;

            // ī�޶� ����ũ ȿ��
            CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
        }
    }
}
