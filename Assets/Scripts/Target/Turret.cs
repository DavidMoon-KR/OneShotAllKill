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
            StartCoroutine(ExplosionDelay());
            m_ExploionTrigger = true;

            // ź�˰� �浹�� ��� 3�ʵڿ� �ı�
            if(other.tag == "Bullet")
            {
                Destroy(gameObject, 3f);
            }
            // �ƴ� ��� �ٷ� �ı�
            else
            {
                Instantiate(m_Explosion, transform.position, Quaternion.identity);
                CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
                Destroy(gameObject);
            }
        }
    }

    // �����ϱ� �� ����� ª�� ������
    private IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(GameManager.Instance.m_DelayExplosion);
        m_Audio.clip = m_ExplosionSound;
        m_Audio.Play();

        // ���� ������ ����
        Instantiate(m_Explosion, transform.position, Quaternion.identity);
        
        // �������� ������ Ÿ���� ���ĵǾ��ٴ� ���� �˸�
        GameManager.Instance.m_HasExplosioned = true;
        
        // ���ӸŴ������� �ڽ��� ������ ��ġ�� �����ϱ�
        GameManager.Instance.m_ExplosionedPos = transform.position;
        
        // �����ϱ� ���� �ڽ��� ��ġ�� �� ������ �ű��.
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -14f, gameObject.transform.position.z);
        
        // ī�޶� ����ũ ȿ��
        CameraShake.Instance.OnShakeCamera(m_ImpactTime, m_ImpactGauge);
    }
}
