using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HintContents
{
    private List<GameObject> m_HintClearTargets;        // 힌트를 통해서 처리해야 할 타겟들
    //[SerializeField] private float m_HintShowDelay;     // 힌트 메시지 띄우기까지 걸리는 시간
    [SerializeField] private int m_AllHintWallCount;    // 힌트를 적용할 벽의 수

    public List<GameObject> HintClearTargets { get => m_HintClearTargets; set => m_HintClearTargets = value; }
    //public float HintShowDelay { get => m_HintShowDelay; }
    public int AllHintWallCount { get => m_AllHintWallCount; }
}

public class HintManager : MonoBehaviour
{
    [SerializeField] private List<HintContents> m_HintContents; // 힌트 정보들
    [SerializeField] private int m_HintBtnDelay;       // 힌트 버튼 뜨기까지의 딜레이
    private int m_NowHintCount = 0;                 // 현재 힌트 번호
    private int m_HintWallCount;                    // 올바르게 힌트에 벽을 위치한 개수

    private bool m_IsActiveHint = false;            // 힌트 활성화 여부
    private bool m_IsActiveHintBtnTimer = false;    // 힌트 타이머가 활성화 되어있는지 여부
    private bool m_IsExitGame = false;              // 게임에서 나갔는지 여부
    private bool m_AllHintClear = false;            // 모든 힌트를 사용했는지 여부

    // 힌트매니저 스크립트를 인스턴스화 한 것
    private static HintManager m_Instance;
    public static HintManager Instance => m_Instance;

    public int HintWallCount { get => m_HintWallCount; set => m_HintWallCount = value; }
    public bool IsActiveHint { get => m_IsActiveHint; set => m_IsActiveHint = value; }
    public int NowHintCount { get => m_NowHintCount; }
    public int RemaningHintCount { get => m_HintContents.Count - m_NowHintCount; }
    public bool IsExitGame { set => m_IsExitGame = value; }

    void Awake()
    {
        var obj = FindObjectsOfType<HintManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(List<HintEtcTargets> p_objs)
    {
        for (int i = 0; i < p_objs.Count; i++)
        {
            m_HintContents[i].HintClearTargets = p_objs[i].HintClearTargets;
        }

        // 현재까지 클리어 한 힌트 현황 초기화 및 일정시간 후에 힌트버튼 활성화
        m_NowHintCount = 0;
        m_AllHintClear = false;
        if (!m_IsActiveHintBtnTimer)
        {
            StartCoroutine(HintTimer());
        }
    }

    void Start()
    {
        m_Instance = GetComponent<HintManager>();
    }

    void Update()
    {
        // 모든 힌트를 완료했으면 쓸데없이 로직 실행 안함
        if (m_AllHintClear)
        {
            return;
        }

        // 현재 힌트의 파괴 목표가 존재하고, 그 목표가 파괴되었다면 힌트 번호 증가
        if (m_HintContents[m_NowHintCount].HintClearTargets != null && ClearHintTargetCheck())
        {
            // 모든 힌트를 완료했으면 힌트 버튼 없애기 및 힌트 완료 체크
            if ((++m_NowHintCount) >= m_HintContents.Count)
            {
                UIManager.Instance.CompleteAllHint();
                m_AllHintClear = true;
            }

            // 힌트를 사용하던 중에 목표를 파괴했다면 벽이 올바른 위치에 있지 않아도 힌트 클리어
            if (m_IsActiveHint)
            {
                m_IsActiveHint = false;
                UIManager.Instance.CompleteHint();
            }
        }
        // 모든 벽이 올바르게 힌트에 위치하였다면
        else if (m_IsActiveHint && m_HintWallCount >= m_HintContents[m_NowHintCount].AllHintWallCount)
        {
            // 만약 위치하는 것 이외의 조건으로 파괴해야할 오브젝트가 없다면 힌트 비활성화 및 힌트 번호 증가
            if (m_HintContents[m_NowHintCount].HintClearTargets == null)
            {
                m_NowHintCount++;
            }
            // 파괴해야할 오브젝트가 있고, 아직 파괴하지 못했다면 힌트 유지
            else if(!ClearHintTargetCheck())
            {
                return;
            }

            // 힌트 비활성화 및 힌트 버튼 활성화
            m_IsActiveHint = false;
            if (m_NowHintCount < m_HintContents.Count)
            {
                UIManager.Instance.CompleteHint();
            }
            else
            {
                UIManager.Instance.CompleteAllHint();
                m_AllHintClear = true;
            }
        }
        m_HintWallCount = 0;
    }

    private bool ClearHintTargetCheck()
    {
        foreach (GameObject obj in m_HintContents[m_NowHintCount].HintClearTargets)
        {
            if (obj == null || !obj.activeSelf)
            {
                continue;
            }
            return false;
        }

        return true;
    }

    // 스테이지가 바뀌면 힌트 초기화
    public void StageChange()
    {
        Destroy(this.gameObject);
    }

    // 힌트
    public IEnumerator HintTimer()
    {
        m_IsActiveHintBtnTimer = true;
        yield return new WaitForSeconds(m_HintBtnDelay);

        m_IsActiveHintBtnTimer = false;
        UIManager.Instance.ShowHintBtn();
    }

    public void ActiveHint()
    {
        // 힌트 활성화
        m_IsActiveHint = true;
    }

    // 게임을 나가면 힌트 관련된 작동 멈추기
    public void ExitGame()
    {
        m_AllHintClear = true;
        m_IsActiveHint = false;
    }
}
