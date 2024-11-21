using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerCheckDontDestroy : MonoBehaviour
{
    private float m_HintShowDelay = 0f;
    private int m_NowHintCount = 0;

    private bool m_IsTimerPlay = false;
    private bool m_IsHintActive = false;

    public bool IsHintActive { get => m_IsHintActive; set => m_IsHintActive = value; }

    public static TimerCheckDontDestroy Instance = null;

    void Awake()
    {
        // SoundManager 인스턴스가 이미 있는지 확인, 이 상태로 설정
        if (Instance == null)
            Instance = this;

        // 인스턴스가 이미 있는 경우 오브젝트 제거
        else if (Instance != this)
            Destroy(gameObject);

        // 이렇게 하면 다음 scene으로 넘어가도 오브젝트가 사라지지 않습니다.
        DontDestroyOnLoad(gameObject);
    }

    public void InitDatas()
    {
        m_HintShowDelay = 0f;
        m_NowHintCount = 0;
        m_IsTimerPlay = false;
        m_IsHintActive = false;
    }

    public void Setup(float p_time)
    {
        if (m_IsTimerPlay || m_IsHintActive)
            return;

        m_HintShowDelay = p_time;
        m_IsTimerPlay = true;
        StartCoroutine(HintTimer());
    }

    public void SetTimerOptions()
    {
        GameManager.Instance.NowHintCount = m_NowHintCount;
        GameManager.Instance.IsShowHintMessage = m_IsHintActive;

        Debug.Log(m_NowHintCount);
        Debug.Log(m_IsHintActive);
    }

    public IEnumerator HintTimer()
    {
        yield return new WaitForSeconds(m_HintShowDelay);

        m_NowHintCount++;

        GameManager.Instance.HintTimer(m_NowHintCount);

        m_HintShowDelay = 0f;
        m_IsTimerPlay = false;
        m_IsHintActive = true;
    }
}
