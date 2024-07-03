using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWallButton : MonoBehaviour
{
    // 버튼을 눌렀는지 상태 여부 판단
    private bool _getPress = false;

    [SerializeField]
    private List<GameObject> _shield;

    [SerializeField]
    private AudioClip _clip;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 탄과 충돌할 경우
        if(collision.collider.tag == "Bullet")
        {
            // 에너지 방벽이 켜있다면 꺼준다.
            if (_getPress == false)
            {
                _audioSource.clip = _clip;
                _audioSource.Play();
                
                // 에너지 방벽이 두개 이상일 수 있기 때문에 모든 방벽의 개수를 파악해서 끄기
                for(int i = 0; i < _shield.Count; i++)
                {
                    _shield[i].SetActive(false);
                }
                _getPress = true;
            }
            // 에너지 방벽이 꺼져있다면 켜준다.
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
