using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[Serializable]
public class HintEtcTargets
{
    [SerializeField] private List<GameObject> m_HintClearTargets;   // 힌트를 통해서 처리해야 할 타겟들

    public List<GameObject> HintClearTargets { get => m_HintClearTargets; }
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Volume m_Volume;

    [SerializeField] private bool m_IsGameOver = false;       // 게임이 끝났는지 판단하는 변수
    [SerializeField] private bool m_IsFailed = false;         // 스테이지 클리어 실패했는지 판단하는 변수
    [SerializeField] private bool m_HasExplosioned = false;   // 스테이지 내에서 폭발이 일어났는지 판단하는 변수
    [SerializeField] private bool m_IsNotAmmo = false;        // 플레이어가 탄을 소유하고 있는지 확인하는 변수

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

    // 단계별 힌트별로 파괴해야할 오브젝트들
    [SerializeField] private List<HintEtcTargets> m_HintClearTargets;

    private bool m_IsGamePause = false; // 게임 일시정지 여부

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

    void Awake()
    {
        m_Instance = GetComponent<GameManager>();

        QualitySettings.vSyncCount = 0;     // V-Sync 비활성화
        Application.targetFrameRate = 240;  // 프레임 전환
    }

    void Start()
    {
        m_TurretCount = GameObject.FindGameObjectsWithTag("Turret").Length;
        m_HumanoidCount = GameObject.FindGameObjectsWithTag("Humanoid").Length;
        m_Targets = m_TurretCount + m_HumanoidCount;

        if (HintManager.Instance != null)
        {
            HintManager.Instance.Init(m_HintClearTargets);
        }

        UIBeforeStart.Instance.GameStart();

        
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
        if (Input.GetKeyUp(KeyCode.R))
        {
            RestartGame();
            if (HintManager.Instance != null)
            {
                HintManager.Instance.IsActiveHint = false;
            }            
        }

        // 게임이 끝났고 클리어 성공한 경우
        if (m_IsGameOver == true && m_IsFailed == false)
        {
            UIManager.Instance.m_MissionComplete = true;
            UIManager.Instance.GameOverMessage();

            GameDataManager.Instance.Data.stageCleared[m_SceneNumber - 1] = true;
            GameDataManager.Instance.Save();
        }
        // 게임이 끝났고 클리어 실패한 경우
        else if (m_IsGameOver == true && m_IsFailed == true)
        {
            UIManager.Instance.m_MissionComplete = false;
            m_IsGamePause = true;
            UIManager.Instance.GameOverMessage();
        }
    }

    // 게임 재시작
    public void RestartGame()
    {
        m_IsGamePause = false;
        SceneManager.LoadScene(m_SceneNumber);
    }

    // 게임오버
    public void GameOver()
    {
        StartCoroutine(GameOverNow());
    }

    // 게임 시작 메시지 지나간 후 블러 효과 지우기
    public void RemoveBlur()
    {
        UnityEngine.Rendering.Universal.DepthOfField depthOfField;
        if (m_Volume.profile.TryGet(out depthOfField))
        {
            depthOfField.active = false;
        }
        UIManager.Instance.Removeblur();
    }

    public IEnumerator GameOverNow()
    {
        yield return new WaitForSeconds(m_GameOverDelay);

        // 타겟이 없을경우
        if (m_Targets <= 0)
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
