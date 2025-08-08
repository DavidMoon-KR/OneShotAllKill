using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

public enum MonsterType
{
    Turret = 0,
    humanoid = 1,
}

[Serializable]
public class StageInfo
{
    public int MonsterTypeCount;            // 몬스터 종류 수
    public List<MonsterType> MonsterType;   // 몬스터 타입
    public List<int> MonsterCount;          // 몬스터 수
}

public class StageController : MonoBehaviour
{
    public GameObject m_StageInfoPopup;
    public Transform panelTransform;

    public Button[] allStages;
    public GameObject topBar;

    public GameObject m_MainMenuController; // 메인메뉴 컨트롤러
    public Transform m_MainMenu;            // 메인메뉴
    public Transform m_StageMenu;   // 스테이지 메뉴

    [SerializeField] public List<StageInfo> m_MonsterList = new List<StageInfo>();

    void Awake()
    {
        for (int i = 0; i < allStages.Length; i++)
        {
            allStages[i].gameObject.transform.DOScaleY(0f, 0f);
            int tempindex = i;
            allStages[i].onClick.AddListener(() => StageReplay(tempindex + 1));
        }

        topBar.transform.DOLocalMoveY(300f, 0f).SetRelative(true);
    }

    void Start()
    {
        SetStageUnlockStates();
    }

    private void SetStageUnlockStates()
    {
        bool[] IsStageUnlock = GameDataManager.Instance.Data.stageCleared;
        int i = 0;

        for (i = 0; i < allStages.Length; i++)
        {
            // 클리어 못한 스테이지 나오면 끝내기
            if (!IsStageUnlock[i]) break;

            // 이미 클리어 한 스테이지
            allStages[i].GetComponent<StageUnlock>().Init(true);
        }
        // 클리어 해야하는 스테이지
        if (i < allStages.Length)
        {
            allStages[i].GetComponent<StageUnlock>().Init(false);
        }
    }

    void OnEnable()
    {
        StartCoroutine(InAnimation());
    }

    IEnumerator InAnimation()
    {
        topBar.transform.DOLocalMoveY(-300f, 0.25f).SetEase(Ease.InOutCubic).SetRelative(true);

        for (int i = 0; i < allStages.Length; i++)
        {
            allStages[i].gameObject.transform.DOScaleY(1f, 0.2f);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator OutAniamtion()
    {
        yield return null;

        OutIconStageImage();

        topBar.transform.DOLocalMoveY(300f, 0.2f).SetEase(Ease.InOutCubic).SetRelative(true).OnComplete(() =>
        {
            // 메인메뉴 메뉴 띄우기
            m_StageMenu.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            m_MainMenu.gameObject.SetActive(true);
            m_MainMenuController.SetActive(true);
        });
    }

    public void OutAnimations(string sceneName)
    {
        StartCoroutine(OutAniamtion(sceneName));
    }

    IEnumerator OutAniamtion(string sceneName)
    {
        yield return null;

        OutIconStageImage();

        topBar.transform.DOLocalMoveY(300f, 0.2f).SetEase(Ease.InOutCubic).SetRelative(true).OnComplete(() =>
        {
            PlayManager.Instance.SceneLoad(sceneName);
        });
    }


    void OutIconStageImage()
    {
        for (int i = 0; i < allStages.Length; i++)
        {
            allStages[i].gameObject.transform.DOScaleY(0f, 0.1f);
        }
    }

    public void StageReplay(int p_StageNum)
    {
        GameObject panels = (GameObject)Instantiate(m_StageInfoPopup, new Vector3(0f, 0f, 0f), Quaternion.identity);
        panels.transform.SetParent(panelTransform, false);
        panels.name = m_StageInfoPopup.name;

        panels.GetComponent<StagePopup>().Init(p_StageNum, m_MonsterList[p_StageNum - 1]);
    }

    public void BackTtitle()
    {
        StartCoroutine(OutAniamtion());
    }

    [ContextMenu("모든 스테이지 언락")]
    private void _UnlockAllStage_Dev()
    {
        for (int i = 0; i < allStages.Length; i++)
        {
            GameDataManager.Instance.Data.stageCleared[i] = true;
        }
        GameDataManager.Instance.Save();

        for (int i = 0; i < allStages.Length; i++)
        {
            // 이미 클리어 한 스테이지
            allStages[i].GetComponent<StageUnlock>().Init(true);
        }
    }
    [ContextMenu("모든 스테이지 락")]
    private void _lockAllStage_Dev()
    {
        for (int i = 0; i < allStages.Length; i++)
        {
            GameDataManager.Instance.Data.stageCleared[i] = false;
        }
        GameDataManager.Instance.Save();

        SetStageUnlockStates();
        for (int i = 1; i < allStages.Length; i++)
        {
            // 이미 클리어 한 스테이지
            allStages[i].GetComponent<StageUnlock>().Unlock();
        }
    }
}
