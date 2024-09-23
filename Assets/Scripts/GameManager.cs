using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool m_IsGameOver = false;        // 게임이 끝났는지 판단하는 변수
    public bool m_IsFailed = false;          // 스테이지 클리어 실패했는지 판단하는 변수
    public bool m_HasExplosioned = false;    // 스테이지 내에서 폭발이 일어났는지 판단하는 변수
    public bool m_HasNotAmmo = false;        // 플레이어가 탄을 소유하고 있는지 확인하는 변수

    [SerializeField] private bool m_IsHumanoid; // 스테이지에 휴머노이드가 있는지 여부
    [SerializeField] private int m_SceneNumber; // 현 스테이지 단계

    public int m_Targets;           // 스테이지 내에 총 타겟 개수
    private int m_TurretCount;      // 스테이지 내에 터렛 개수
    private int m_HumanoidCount;    // 스테이지 내에 휴머노이드 개수

    public Vector3 m_ExplosionedPos; // 폭발이 일어난 위치

    // 게임이 끝나기 전에 잠시 기다리는 시간
    [SerializeField] private float m_GameOverDelay;

    // 타겟이 폭발하는 데 잠시 기다리는 시간
    [SerializeField] public float m_DelayExplosion;

    private bool m_IsGamePause = false;

    // 게임매니저 스크립트를 인스턴스화 한 것
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    // 프로퍼티
    public bool IsGamePause { get => m_IsGamePause; set => m_IsGamePause = value; }
    public int SceneNumber { get => m_SceneNumber; }

    void Start()
    {
        m_Instance = GetComponent<GameManager>();
        m_TurretCount = GameObject.FindGameObjectsWithTag("Turret").Length;
        m_HumanoidCount = GameObject.FindGameObjectsWithTag("Humanoid").Length;
        m_Targets = m_TurretCount + m_HumanoidCount;
    }

    void Update()
    {
        // 게임 내 타겟이 없거나, 플레이어가 소지한 탄알이 없다면
        if (m_Targets == 0 || m_HasNotAmmo == true)
        {
            GameOver();
        }

        // 폭발이 일어났고, 휴머노이드에게 해당 위치의 좌표를 알려줌
        if (m_HasExplosioned == true && m_IsHumanoid == true)
        {
            GameObject[] humanoids = GameObject.FindGameObjectsWithTag("Humanoid");
            foreach (var humanoid in humanoids)
            {
                Humanoid currentHumanoid = humanoid.GetComponent<Humanoid>();
                currentHumanoid.m_ExplosionedPos = m_ExplosionedPos;

                // 타겟 감지 true
                currentHumanoid.m_ExplosionDetection = true;
            }
            m_HasExplosioned = false;
        }

        // 재시작
        if (Input.GetKeyUp(KeyCode.R) && !m_IsGamePause)
        {
            RestartGame();
        }

        // 게임이 끝났고 클리어 성공한 경우
        if (m_IsGameOver == true && m_IsFailed == false)
        {
            UIManager.Instance.m_MissionComplete = true;
            UIManager.Instance.GameOverMessage();
        }
        // 게임이 끝났고 클리어 실패한 경우
        else if (m_IsGameOver == true && m_IsFailed == true)
        {
            UIManager.Instance.m_MissionComplete = false;
            UIManager.Instance.GameOverMessage();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(m_SceneNumber);
    }

    // 게임오버
    public void GameOver()
    {
        StartCoroutine(GameOverNow());
    }

    public IEnumerator GameOverNow()
    {
        yield return new WaitForSeconds(m_GameOverDelay);

        // 타겟이 없을경우
        if (m_Targets == 0)
        {
            m_IsGameOver = true;
            m_IsFailed = false;
        }
        // 있을 경우
        else
        {
            m_IsGameOver = true;
            m_IsFailed = true;
        }
    }
}
