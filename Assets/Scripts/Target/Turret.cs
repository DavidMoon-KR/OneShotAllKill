using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // �������� �� ����ŷ �ð�
    [SerializeField]
    private float _impactTime;

    // ���� ����ŷ ����
    [SerializeField]
    private float _impactGauge;

    // ȸ�� �ӵ�
    [SerializeField]
    private float _rotateSpeed;

    // �ѹ��� ������ �� �ֵ��� �Ѵ�.
    private bool _exploionTrigger = false;

    // ���� VFX ������
    [SerializeField]
    GameObject _explosion;

    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audio;

    void Update()
    {
        transform.Rotate(0, _rotateSpeed * Time.deltaTime, 0);
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ź�˿� �浹�ϰų� ���� ���߰� �浹�ߴٸ�
        if ((other.tag == "Bullet" || other.tag == "GasExplosion") && _exploionTrigger == false)
        {
            // ���ӸŴ������� ���� Ÿ�� �� -1
            GameManager.Instance._targets--;
            StartCoroutine(ExplosionDelay());
            _exploionTrigger = true;

            // ź�˰� �浹�� ��� 3�ʵڿ� �ı�
            if(other.tag == "Bullet")
            {
                Destroy(gameObject, 3f);
            }
            // �ƴ� ��� �ٷ� �ı�
            else
            {
                Instantiate(_explosion, transform.position, Quaternion.identity);
                CameraShake.Instance.OnShakeCamera(_impactTime, _impactGauge);
                Destroy(gameObject);
            }
        }
    }

    // �����ϱ� �� ����� ª�� ������
    private IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(GameManager.Instance._delayExplosion);
        _audio.clip = _explosionSound;
        _audio.Play();

        // ���� ������ ����
        Instantiate(_explosion, transform.position, Quaternion.identity);
        
        // �������� ������ Ÿ���� ���ĵǾ��ٴ� ���� �˸�
        GameManager.Instance._hasExplosioned = true;
        
        // ���ӸŴ������� �ڽ��� ������ ��ġ�� �����ϱ�
        GameManager.Instance._explosionedPos = transform.position;
        
        // �����ϱ� ���� �ڽ��� ��ġ�� �� ������ �ű��.
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -14f, gameObject.transform.position.z);
        
        // ī�޶� ����ũ ȿ��
        CameraShake.Instance.OnShakeCamera(_impactTime, _impactGauge);
    }
}
