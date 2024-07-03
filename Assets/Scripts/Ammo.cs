using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float speed = 20f;       // 탄 스피드
    public GameObject muzzlePrefab; // 탄 퍼짐 효과
    public GameObject hitPrefab;    // 사물과 충돌했을 경우 탄퍼짐 효과
    private Vector3 direction;      // 탄 거리

    private void Start()
    {
        direction = transform.forward;
        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
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
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed");
        }
    }

    private void LateUpdate()
    {
        direction = transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 장애물에 충돌한 경우
        if (collision.gameObject.CompareTag("Broken") || (collision.gameObject.CompareTag("EnergyWall")))
        {
            Destroy(gameObject);
        }

        // 탄 퍼짐 생성
        Instantiate(hitPrefab, transform.position, Quaternion.identity);
        var firstContact = collision.contacts[0];
        
        // 반대쪽으로 각 전환
        Vector3 newVelocity = Vector3.Reflect(direction.normalized, firstContact.normal);
        Bounce(newVelocity.normalized);
    }

    public void Bounce(Vector3 direction)
    {
        // 마우스 포인터 바라보는 위치로 방향 전환
        transform.rotation = Quaternion.LookRotation(direction);
    }

}