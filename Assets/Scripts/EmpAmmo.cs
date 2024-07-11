using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpAmmo : MonoBehaviour
{
    public float m_Speed = 70f;       // ź ���ǵ�
    public GameObject m_MuzzlePrefab; // ź ���� ȿ��
    public GameObject m_HitPrefab;    // �繰�� �浹���� ��� ź���� ȿ��
    private Vector3 m_Direction;      // ź �Ÿ�

    [SerializeField]
    private GameObject m_EmpExplosion;

    private void Start()
    {
        m_Direction = transform.forward;
        if (m_MuzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(m_MuzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    void Update()
    {
        if (m_Speed != 0)
        {
            transform.position += transform.forward * (m_Speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No m_Speed");
        }
    }

    private void LateUpdate()
    {
        m_Direction = transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ��ֹ��� �浹�� ���
        if (collision.gameObject.CompareTag("Broken") || (collision.gameObject.CompareTag("EnergyWall")))
        {
            // Emp ����
            EmpExplosion();

            Destroy(gameObject);
            return;
        }

        // ź ���� ����
        Instantiate(m_HitPrefab, transform.position, Quaternion.identity);
        var firstContact = collision.contacts[0];

        // �ݴ������� �� ��ȯ
        Vector3 newVelocity = Vector3.Reflect(m_Direction.normalized, firstContact.normal);
        Bounce(newVelocity.normalized);
    }

    // Emp ����
    private void EmpExplosion()
    {
        Instantiate(m_EmpExplosion, transform.position, Quaternion.identity);
    }

    private void Bounce(Vector3 p_direction)
    {
        // ���콺 ������ �ٶ󺸴� ��ġ�� ���� ��ȯ
        transform.rotation = Quaternion.LookRotation(p_direction);
    }
}
