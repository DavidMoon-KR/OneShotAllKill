using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool m_IsGameOver = false;       // 게임이 끝났는지 판단하는 변수
    public bool m_IsFailed = false;          // 스테이지 클리어 실패했는지 판단하는 변수
    public bool m_HasExplosioned = false;    // 스테이지 내에서 폭발이 일어났는지 판단하는 변수
    public bool m_HasNotAmmo = false;        // 플레이어가 탄을 소유하고 있는지 확인하는 변수

    // 휴머노이드가 스테이지에 존재하는지 판단하는 변수 휴머노이드가 있을 경우 true반환하며, 게임매니저에서 타겟의 위치를 휴머노이에게 모두 전달한다. 하지만 없을경우 false를 반환하며, 위치를 전달하지 않음. 각 스테이지마다 휴머노이드가 없는 경우를 고려하여, 만든 변수
    [SerializeField]
    private bool m_IsHumanoid;

    public int m_Targets;            // 스테이지 내에 총 타겟 개수
    private int m_TurretCount;       // 스테이지 내에 터렛 개수
    private int m_HumanoidCount;     // 스테이지 내에 휴머노이드 개수
    public int m_SceneNumber;        // 현 스테이지 단계
    public Vector3 m_ExplosionedPos; // 폭발이 일어난 위치

    // 게임이 끝나기 전에 잠시 기다리는 시간
    [SerializeField]
    private float m_GameOverDelay;

    // 타겟이 폭발하는 데 잠시 기다리는 시간
    [SerializeField]
    public float m_DelayExplosion;

    // 게임매니저 스크립트를 인스턴스화 한 것
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    void Start()
    {
        m_Instance = GetComponent<GameManager>();
        m_TurretCount = GameObject.FindGameObjectsWithTag("Turret").Length;
        m_HumanoidCount = GameObject.FindGameObjectsWithTag("Humanoid").Length;
        m_Targets = m_TurretCount + m_HumanoidCount;
    }

    void Update()
    {
        // 게임 내 타겟이 없고, 플레이어가 소지한 탄알이 없다면
        if(m_Targets == 0 || m_HasNotAmmo == true)
        {
            GameOver();
        }

        // 폭발이 일어났고, 휴머노이드에게 해당 위치의 좌표를 알려줌
        if(m_HasExplosioned == true && m_IsHumanoid == true)
        {
            GameObject[] humanoids = GameObject.FindGameObjectsWithTag("Humanoid");
            foreach(var humanoid in humanoids)
            {
                Humanoid currentHumanoid = humanoid.GetComponent<Humanoid>();
                currentHumanoid.m_ExplosionedPos = m_ExplosionedPos;
                
                // 타겟 감지 true
                currentHumanoid.m_ExplosionDetection = true;
            }
            m_HasExplosioned = false;
        }

        // 재시작
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(m_SceneNumber);
        }

        // 게임이 끝났고 클리어 실패한 경우
        if(m_IsGameOver == true && m_IsFailed == false)
        {
            UIManager.Instance.m_MissionComplete = true;
            UIManager.Instance.GameOverMessage();
        }
        // 게임이 끝났고 클리어 성공한 경우
        else if(m_IsGameOver == true && m_IsFailed == true)
        {
            UIManager.Instance.m_MissionComplete = false;
            UIManager.Instance.GameOverMessage();
        }
    }

    // 게임오버
    public void GameOver()
    {
        StartCoroutine(GameOverNow());
    }

    public IEnumerator GameOverNow()
    {
        yield return new WaitForSeconds(3.0f);

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
