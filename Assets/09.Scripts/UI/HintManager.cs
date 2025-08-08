using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HintContents
{
    private List<GameObject> m_HintClearTargets;        // ��Ʈ�� ���ؼ� ó���ؾ� �� Ÿ�ٵ�
    //[SerializeField] private float m_HintShowDelay;     // ��Ʈ �޽��� ������� �ɸ��� �ð�
    [SerializeField] private int m_AllHintWallCount;    // ��Ʈ�� ������ ���� ��

    public List<GameObject> HintClearTargets { get => m_HintClearTargets; set => m_HintClearTargets = value; }
    //public float HintShowDelay { get => m_HintShowDelay; }
    public int AllHintWallCount { get => m_AllHintWallCount; }
}

public class HintManager : MonoBehaviour
{
    [SerializeField] private List<HintContents> m_HintContents; // ��Ʈ ������
    [SerializeField] private int m_HintBtnDelay;       // ��Ʈ ��ư �߱������ ������
    private int m_NowHintCount = 0;                 // ���� ��Ʈ ��ȣ
    private int m_HintWallCount;                    // �ùٸ��� ��Ʈ�� ���� ��ġ�� ����

    private bool m_IsActiveHint = false;            // ��Ʈ Ȱ��ȭ ����
    private bool m_IsActiveHintBtnTimer = false;    // ��Ʈ Ÿ�̸Ӱ� Ȱ��ȭ �Ǿ��ִ��� ����
    private bool m_IsExitGame = false;              // ���ӿ��� �������� ����
    private bool m_AllHintClear = false;            // ��� ��Ʈ�� ����ߴ��� ����

    // ��Ʈ�Ŵ��� ��ũ��Ʈ�� �ν��Ͻ�ȭ �� ��
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

        // ������� Ŭ���� �� ��Ʈ ��Ȳ �ʱ�ȭ �� �����ð� �Ŀ� ��Ʈ��ư Ȱ��ȭ
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
        // ��� ��Ʈ�� �Ϸ������� �������� ���� ���� ����
        if (m_AllHintClear)
        {
            return;
        }

        // ���� ��Ʈ�� �ı� ��ǥ�� �����ϰ�, �� ��ǥ�� �ı��Ǿ��ٸ� ��Ʈ ��ȣ ����
        if (m_HintContents[m_NowHintCount].HintClearTargets != null && ClearHintTargetCheck())
        {
            // ��� ��Ʈ�� �Ϸ������� ��Ʈ ��ư ���ֱ� �� ��Ʈ �Ϸ� üũ
            if ((++m_NowHintCount) >= m_HintContents.Count)
            {
                UIManager.Instance.CompleteAllHint();
                m_AllHintClear = true;
            }

            // ��Ʈ�� ����ϴ� �߿� ��ǥ�� �ı��ߴٸ� ���� �ùٸ� ��ġ�� ���� �ʾƵ� ��Ʈ Ŭ����
            if (m_IsActiveHint)
            {
                m_IsActiveHint = false;
                UIManager.Instance.CompleteHint();
            }
        }
        // ��� ���� �ùٸ��� ��Ʈ�� ��ġ�Ͽ��ٸ�
        else if (m_IsActiveHint && m_HintWallCount >= m_HintContents[m_NowHintCount].AllHintWallCount)
        {
            // ���� ��ġ�ϴ� �� �̿��� �������� �ı��ؾ��� ������Ʈ�� ���ٸ� ��Ʈ ��Ȱ��ȭ �� ��Ʈ ��ȣ ����
            if (m_HintContents[m_NowHintCount].HintClearTargets == null)
            {
                m_NowHintCount++;
            }
            // �ı��ؾ��� ������Ʈ�� �ְ�, ���� �ı����� ���ߴٸ� ��Ʈ ����
            else if(!ClearHintTargetCheck())
            {
                return;
            }

            // ��Ʈ ��Ȱ��ȭ �� ��Ʈ ��ư Ȱ��ȭ
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

    // ���������� �ٲ�� ��Ʈ �ʱ�ȭ
    public void StageChange()
    {
        Destroy(this.gameObject);
    }

    // ��Ʈ
    public IEnumerator HintTimer()
    {
        m_IsActiveHintBtnTimer = true;
        yield return new WaitForSeconds(m_HintBtnDelay);

        m_IsActiveHintBtnTimer = false;
        UIManager.Instance.ShowHintBtn();
    }

    public void ActiveHint()
    {
        // ��Ʈ Ȱ��ȭ
        m_IsActiveHint = true;
    }

    // ������ ������ ��Ʈ ���õ� �۵� ���߱�
    public void ExitGame()
    {
        m_AllHintClear = true;
        m_IsActiveHint = false;
    }
}
