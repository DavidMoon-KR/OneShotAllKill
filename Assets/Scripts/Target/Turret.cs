using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{    
    [SerializeField] private float m_ImpactTime;    // �������� �� ����ŷ �ð�
    [SerializeField] private float m_ImpactGauge;   // ���� ����ŷ ����
    [SerializeField] private float m_RotateSpeed;   // ȸ�� �ӵ�
    
    [SerializeField] GameObject m_Explosion;    // ���� VFX ������
    [SerializeField] private GameObject m_Spark;
    private bool m_IsExplotion;
    private Animator m_Anim;
    // ����
    [SerializeField] private AudioClip m_ExplosionSound;
    private AudioSource m_Audio;

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Anim.SetBool("turnright", true);
        m_Audio = GetComponent<AudioSource>();
        m_IsExplotion = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ź�˿� �浹�ϰų� ���� ���߰� �浹�ߴٸ�
        if (collision.gameObject.CompareTag("Bullet") && m_IsExplotion == false)
        {
            m_IsExplotion = true;
            m_Anim.SetBool("turnright", false);
            m_Anim.SetBool("die", true);
            Instantiate(m_Spark, transform.position, Quaternion.identity);
            // ���ӸŴ������� ���� Ÿ�� �� -1
            GameManager.Instance.m_Targets--;
            this.GetComponent<BoxCollider>().isTrigger = true;
            StartCoroutine(ExplosionDelay(true));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ź�˿� �浹�ϰų� ���� ���߰� �浹�ߴٸ�
        if (other.gameObject.CompareTag("GasExplosion") && m_IsExplotion == false)
        {
            m_IsExplotion = true;
            m_Anim.SetBool("turnright", false);
            m_Anim.SetBool("die", true);
            Instantiate(m_Spark, transform.position, Quaternion.identity);
            // ���ӸŴ������� ���� Ÿ�� �� -1
            GameManager.Instance.m_Targets--;
            StartCoroutine(ExplosionDelay(false));
        }
    }

    // �����ϱ� �� ����� ª�� ������
    private IEnumerator ExplosionDelay(bool p_hasBullet)
    {
        // ź�� �浹�� ���
        if (p_hasBullet == true)
        {
            yield return new WaitForSeconds(GameManager.Instance.m_DelayExplosion);
        }
        // ���� ���߰� �浹�� ���
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        Instantiate(m_Explosion, transform.position, Quaternion.identity);

        // �������� ������ Ÿ���� ���ĵǾ��ٴ� ���� �˸�
        GameManager.Instance.HasExplosioned = true;

        // ���ӸŴ������� �ڽ��� ������ ��ġ�� �����ϱ�
        GameManager.Instance.m_ExplosionedPos = transform.position;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -14f, gameObject.transform.position.z);

        CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
        Destroy(gameObject);
    }
}