using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // �������� �� ����ŷ �ð�
    [SerializeField]
    private float m_ImpactTime;

    // ���� ����ŷ ����
    [SerializeField]
    private float m_ImpactGauge;

    // ȸ�� �ӵ�
    [SerializeField]
    private float m_RotateSpeed;

    // �ѹ��� ������ �� �ֵ��� �Ѵ�.
    private bool m_ExploionTrigger = false;

    // ���� VFX ������
    [SerializeField]
    GameObject m_Explosion;

    [SerializeField]
    private AudioClip m_ExplosionSound;
    private AudioSource m_Audio;

    void Update()
    {
        transform.Rotate(0, m_RotateSpeed * Time.deltaTime, 0);
        m_Audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ź�˿� �浹�ϰų� ���� ���߰� �浹�ߴٸ�
        if ((other.tag == "Bullet" || other.tag == "GasExplosion") && m_ExploionTrigger == false)
        {
            // ���ӸŴ������� ���� Ÿ�� �� -1
            GameManager.Instance.m_Targets--;
            m_ExploionTrigger = true;

            if (other.tag == "Bullet")
            {
                StartCoroutine(ExplosionDelay(true));
            }

            else
            {
                StartCoroutine(ExplosionDelay(false));
            }
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
        GameManager.Instance.m_HasExplosioned = true;

        // ���ӸŴ������� �ڽ��� ������ ��ġ�� �����ϱ�
        GameManager.Instance.m_ExplosionedPos = transform.position;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -14f, gameObject.transform.position.z);

        CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
        Destroy(gameObject);
    }
}