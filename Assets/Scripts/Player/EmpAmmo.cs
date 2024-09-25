using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpAmmo : MonoBehaviour
{
    public float m_Speed = 25f;       // 탄 스피드
    public GameObject m_MuzzlePrefab; // 탄 퍼짐 효과
    public GameObject m_HitPrefab;    // 사물과 충돌했을 경우 탄퍼짐 효과
    private Vector3 m_Direction;      // 탄 거리
    private Rigidbody m_Rigidbody;
    // Emp탄 폭발 이펙트
    [SerializeField] private GameObject m_EmpExplosion;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
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
            m_Rigidbody.velocity = (transform.forward * (m_Speed * Time.deltaTime)) * 1000;
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
        // 장애물에 충돌한 경우
        if (collision.gameObject.CompareTag("Broken") || (collision.gameObject.CompareTag("EnergyWall")))
        {
            if (0 >= Player.Instance.BulletSum)
            {
                GameManager.Instance.IsNotAmmo = true;
            }

            EmpExplosion();
            Destroy(gameObject);
            return;
        }

        // 탄 퍼짐 생성
        Instantiate(m_HitPrefab, transform.position, Quaternion.identity);
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("RotateWall") || collision.gameObject.CompareTag("AccessButton"))
        {
            var firstContact = collision.contacts[0];

            // 반대쪽으로 각 전환
            Vector3 newVelocity = Vector3.Reflect(m_Direction.normalized, firstContact.normal);
            Bounce(newVelocity.normalized);
        }
    }

    // Emp 폭발
    private void EmpExplosion()
    {
        Instantiate(m_EmpExplosion, transform.position, Quaternion.identity);
    }

    private void Bounce(Vector3 p_direction)
    {
        // 마우스 포인터 바라보는 위치로 방향 전환
        transform.rotation = Quaternion.LookRotation(p_direction);
    }
}
