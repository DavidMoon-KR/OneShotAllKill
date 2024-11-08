using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool m_IsGameOver = false;       // 게임이 끝났는지 판단하는 변수
    [SerializeField] private bool m_IsFailed = false;         // 스테이지 클리어 실패했는지 판단하는 변수
    [SerializeField] private bool m_HasExplosioned = false;   // 스테이지 내에서 폭발이 일어났는지 판단하는 변수
    [SerializeField] private bool m_IsNotAmmo = false;        // 플레이어가 탄을 소유하고 있는지 확인하는 변수

    [SerializeField] private bool m_IsHumanoid; // 스테이지에 휴머노이드가 있는지 여부
    [SerializeField] private int m_SceneNumber; // 현 스테이지 단계

    public int m_Targets;           // 스테이지 내에 총 타겟 개수
    private int m_TurretCount;      // 스테이지 내에 터렛 개수
    private int m_HumanoidCount;    // 스테이지 내에 휴머노이드 개수
    private int m_HintWallCount;    // 올바르게 힌트에 벽을 위치한 개수

    public Vector3 m_ExplosionedPos; // 폭발이 일어난 위치
    
    // 게임이 끝나기 전에 잠시 기다리는 시간
    [SerializeField] private float m_GameOverDelay;

    // 타겟이 폭발하는 데 잠시 기다리는 시간
    [SerializeField] public float m_DelayExplosion;

    [SerializeField] private List<GameObject> m_HintClearTargets;   // 힌트를 통해서 처리해야 할 타겟들
    [SerializeField] private List<float> m_HintShowDelay;   // 힌트 메시지 띄우기까지 걸리는 시간
    [SerializeField] private List<int> m_AllHintWallCount;  // 힌트를 적용할 벽의 수
    [SerializeField] private List<bool> m_IsApplyHint;      // 힌트를 바로 적용할 지 시간이 흐른 후 적용할 지 여부
    [SerializeField] private int m_AllHintCount;            // 힌트 수
    private int m_NowHintCount = 0;                         // 현재 힌트 번호

    private bool m_IsGamePause = false;         // 게임 일시정지 여부
    private bool m_IsShowHintMessage = false;   // 힌트 메시지 활성화 여부
    private bool m_IsActiveHint = false;        // 힌트 활성화 여부

    // 게임매니저 스크립트를 인스턴스화 한 것
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    // 프로퍼티
    public bool IsGamePause { get => m_IsGamePause; set => m_IsGamePause = value; }
    public bool IsNotAmmo { set => m_IsNotAmmo = value; }
    public int SceneNumber { get => m_SceneNumber; }
    public bool IsGameOver { get => m_IsGameOver; set => m_IsGameOver = value; }
    public bool IsFailed { get => m_IsFailed; set => m_IsFailed = value; }
    public bool HasExplosioned { get => m_HasExplosioned; set => m_HasExplosioned = value; }
    public int HintWallCount { get => m_HintWallCount; set => m_HintWallCount = value; }
    public bool IsActiveHint { get => m_IsActiveHint; }
    public int NowHintCount { get  => m_NowHintCount; }

    void Start()
    {
        m_Instance = GetComponent<GameManager>();
        m_TurretCount = GameObject.FindGameObjectsWithTag("Turret").Length;
        m_HumanoidCount = GameObject.FindGameObjectsWithTag("Humanoid").Length;
        m_Targets = m_TurretCount + m_HumanoidCount;

        // 힌트가 존재하는 맵이면 힌트 타이머 작동
        if (m_AllHintCount >= 1)
        {
            StartCoroutine(HintTimer());
        }        
    }

    void Update()
    {
        // 게임 내 타겟이 없거나, 플레이어가 소지한 탄알이 없다면
        if (m_Targets == 0 || m_IsNotAmmo == true)
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

        // 힌트를 바로 적용하기로 설정했다면 무조건 바로 힌트 생성
        // 힌트 메시지가 활성화 되었고, F키를 눌렀다면
        if (m_NowHintCount > 0 && m_IsApplyHint[m_NowHintCount-1] || m_IsShowHintMessage && Input.GetKeyDown(KeyCode.F))
        {
            // 힌트 메시지 끄기
            UIManager.Instance.CloseHint();

            // 힌트 활성화
            m_IsActiveHint = true;
            m_IsShowHintMessage = false;
        }
        // 모든 벽이 올바르게 힌트에 위치하였다면
        else if (m_IsActiveHint && m_HintWallCount >= m_AllHintWallCount[m_NowHintCount - 1])
        {
            // 만약 위치하는 것 이외의 조건으로 처리해야할 오브젝트가 아직 남아있다면 힌트 활성화 상태 유지
            if (m_HintClearTargets != null && !ClearHintTargetCheck())
            {
                return;
            }

            // 힌트 비활성화
            m_IsActiveHint = false;
            if ((m_NowHintCount) < m_AllHintCount)
            {
                StartCoroutine(HintTimer());
            }
        }
        m_HintWallCount = 0;
    }

    private bool ClearHintTargetCheck()
    {
        foreach (GameObject obj in m_HintClearTargets)
        {
            if (obj == null || !obj.activeSelf)
            {
                continue;
            }
            return false;
        }

        return true;
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

    // 힌트
    public IEnumerator HintTimer()
    {
        yield return new WaitForSeconds(m_HintShowDelay[m_NowHintCount]);

        UIManager.Instance.ShowHint();
        m_IsShowHintMessage = true;
        m_NowHintCount++;
    }
}
