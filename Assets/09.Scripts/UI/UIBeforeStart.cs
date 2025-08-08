using UnityEngine;

public class UIBeforeStart : MonoBehaviour
{
    [SerializeField] private bool m_IsActiveStageMessage;
    [SerializeField] private GameObject m_StageMessage;

    // 힌트매니저 스크립트를 인스턴스화 한 것
    private static UIBeforeStart m_Instance;
    public static UIBeforeStart Instance => m_Instance;

    public bool IsActiveStageMessage { set => m_IsActiveStageMessage = value; }

    void Awake()
    {
        var obj = FindObjectsOfType<UIBeforeStart>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        m_Instance = GetComponent<UIBeforeStart>();
        m_IsActiveStageMessage = true;
    }

    public void GameStart()
    {
        if (m_IsActiveStageMessage)
        {
            m_StageMessage.SetActive(true);
            m_IsActiveStageMessage = false;
        }
        else
        {
            GameManager.Instance.RemoveBlur();
        }
    }
}
