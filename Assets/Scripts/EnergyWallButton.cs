using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWallButton : MonoBehaviour
{
    //��ư�� �������� ���� ���� �Ǵ�
    private bool _getPress = false;

    [SerializeField]
    private List<GameObject> _shield;

    [SerializeField]
    private AudioClip _clip;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ź�� �浹�� ���
        if(collision.collider.tag == "Bullet")
        {
            //������ �溮�� ���ִٸ� ���ش�.
            if (_getPress == false)
            {
                _audioSource.clip = _clip;
                _audioSource.Play();
                //������ �溮�� �ΰ� �̻��� �� �ֱ� ������ ��� �溮�� ������ �ľ��ؼ� ����
                for(int i = 0; i < _shield.Count; i++)
                {
                    _shield[i].SetActive(false);
                }
                _getPress = true;
            }
            //������ �溮�� �����ִٸ� ���ش�.
            else if (_getPress == true)
            {
                for (int i = 0; i < _shield.Count; i++)
                {
                    _shield[i].SetActive(true);
                }
                _getPress = false;
            }
        }
    }
}
