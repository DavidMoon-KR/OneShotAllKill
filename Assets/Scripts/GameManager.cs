using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;       // ������ �������� �Ǵ��ϴ� ����
    public bool _isFailed = false;          // �������� Ŭ���� �����ߴ��� �Ǵ��ϴ� ����
    public bool _hasExplosioned = false;    // �������� ������ ������ �Ͼ���� �Ǵ��ϴ� ����
    public bool _hasNotAmmo = false;        // �÷��̾ ź�� �����ϰ� �ִ��� Ȯ���ϴ� ����

    // �޸ӳ��̵尡 ���������� �����ϴ��� �Ǵ��ϴ� ����
    [SerializeField]
    private bool _isHumanoid;

    public int _targets;            // �������� ���� �� Ÿ�� ����
    private int _turretCount;       // �������� ���� �ͷ� ����
    private int _humanoidCount;     // �������� ���� �޸ӳ��̵� ����
    public int _sceneNumber;        // �� �������� �ܰ�
    public Vector3 _explosionedPos; // ������ �Ͼ ��ġ

    // ������ ������ ���� ��� ��ٸ��� �ð�
    [SerializeField]
    private float _gameOverDelay;

    // Ÿ���� �����ϴ� �� ��� ��ٸ��� �ð�
    [SerializeField]
    public float _delayExplosion;

    // ���ӸŴ��� ��ũ��Ʈ�� �ν��Ͻ�ȭ �� ��
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    void Start()
    {
        _instance = GetComponent<GameManager>();
        _turretCount = GameObject.FindGameObjectsWithTag("Turret").Length;
        _humanoidCount = GameObject.FindGameObjectsWithTag("Humanoid").Length;
        _targets = _turretCount + _humanoidCount;
    }

    void Update()
    {
        // ���� �� Ÿ���� ����, �÷��̾ ������ ź���� ���ٸ�
        if(_targets == 0 || _hasNotAmmo == true)
        {
            GameOver();
        }

        // ������ �Ͼ��, �޸ӳ��̵忡�� �ش� ��ġ�� ��ǥ�� �˷���
        if(_hasExplosioned == true && _isHumanoid == true)
        {
            GameObject[] _humanoids = GameObject.FindGameObjectsWithTag("Humanoid");
            foreach(var _humanoid in _humanoids)
            {
                Humanoid _currentHumanoid = _humanoid.GetComponent<Humanoid>();
                _currentHumanoid._explosionedPos = _explosionedPos;
                
                // Ÿ�� ���� true
                _currentHumanoid._explosionDetection = true;
            }
            _hasExplosioned = false;
        }

        // �����
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(_sceneNumber);
        }

        // ������ ������ Ŭ���� ������ ���
        if(_isGameOver == true && _isFailed == false)
        {
            UIManager.Instance._missionComplete = true;
            UIManager.Instance.GameOverMessage();
        }
        // ������ ������ Ŭ���� ������ ���
        else if(_isGameOver == true && _isFailed == true)
        {
            UIManager.Instance._missionComplete = false;
            UIManager.Instance.GameOverMessage();
        }
    }

    // ���ӿ���
    public void GameOver()
    {
        StartCoroutine(GameOverNow());
    }

    public IEnumerator GameOverNow()
    {
        yield return new WaitForSeconds(3.0f);

        // Ÿ���� �������
        if (_targets == 0)
        {
            _isGameOver = true;
            _isFailed = false;
        }
        // ���� ���
        else
        {
            _isGameOver = true;
            _isFailed = true;
        }
    }
}
