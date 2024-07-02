using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //게임이 끝났는지 판단하는 변수
    private bool _isGameOver = false;
    //스테이지 클리어 실패했는지 판단하는 변수
    public bool _isFailed = false;
    //스테이지 내에서 폭발이 일어났는지 판단하는 변수
    public bool _hasExplosioned = false;
    //플레이어가 탄을 소유하고 있는지 확인하는 변수
    public bool _hasNotAmmo = false;

    //휴머노이드가 스테이지에 존재하는지 판단하는 변수
    [SerializeField]
    private bool _isHumanoid;
    //스테이지 내에 총 타겟 개수
    public int _targets;
    //스테이지 내에 터렛 개수
    private int _turretCount;
    //스테이지 내에 휴머노이드 개수
    private int _humanoidCount;
    //현 스테이지 단계
    public int _sceneNumber;
    //폭발이 일어난 위치
    public Vector3 _explosionedPos;
    //게임이 끝나기 전에 잠시 기다리는 시간
    [SerializeField]
    private float _gameOverDelay;
    //타겟이 폭발하는 데 잠시 기다리는 시간
    [SerializeField]
    public float _delayExplosion;

    //게임매니저 스크립트를 인스턴스화 한 것
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // Start is called before the first frame update
    void Start()
    {
        _instance = GetComponent<GameManager>();
        _turretCount = GameObject.FindGameObjectsWithTag("Turret").Length;
        _humanoidCount = GameObject.FindGameObjectsWithTag("Humanoid").Length;
        _targets = _turretCount + _humanoidCount;
    }

    // Update is called once per frame
    void Update()
    {
        //게임 내 타겟이 없고, 플레이어가 소지한 탄알이 없다면
        if(_targets == 0 || _hasNotAmmo == true)
        {
            GameOver();
        }
        //폭발이 일어났고, 휴머노이드에게 해당 위치의 좌표를 알려줌
        if(_hasExplosioned == true && _isHumanoid == true)
        {
            GameObject[] _humanoids = GameObject.FindGameObjectsWithTag("Humanoid");
            foreach(var _humanoid in _humanoids)
            {
                Humanoid _currentHumanoid = _humanoid.GetComponent<Humanoid>();
                _currentHumanoid._explosionedPos = _explosionedPos;
                // 타겟 감지 true
                _currentHumanoid._explosionDetection = true;
            }
            _hasExplosioned = false;
        }
        //재시작
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(_sceneNumber);
        }

        //게임이 끝났고 클리어 실패한 경우
        if(_isGameOver == true && _isFailed == false)
        {
            UIManager.Instance._missionComplete = true;
            UIManager.Instance.GameOverMessage();
        }
        //게임이 끝났고 클리어 성공한 경우
        else if(_isGameOver == true && _isFailed == true)
        {
            UIManager.Instance._missionComplete = false;
            UIManager.Instance.GameOverMessage();
        }
    }
    //게임오버
    public void GameOver()
    {
        StartCoroutine(GameOverNow());
    }

    public IEnumerator GameOverNow()
    {
        yield return new WaitForSeconds(3.0f);
        //타겟이 없을경우
        if (_targets == 0)
        {
            _isGameOver = true;
            _isFailed = false;
        }
        else //있을 경우
        {
            _isGameOver = true;
            _isFailed = true;
        }
    }
}
