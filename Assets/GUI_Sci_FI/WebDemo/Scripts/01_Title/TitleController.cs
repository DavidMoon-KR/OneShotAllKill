using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Localization.Settings;

public class TitleController : MonoBehaviour {

	//Bottom Menu Bar
	public GameObject imgButtom;
	public GameObject startButton;
	public Text textStart;
	private Color textActiveColor;
	public Color dimColor;

	public Image imageTitle;

    public GameObject m_StageMenuController; // 스테이지 메뉴 컨트롤러
    public Transform m_StageMenu;   // 스테이지 메뉴

    public Transform m_MainMenu;   // 메인메뉴

	private int a = 0;

    void Awake () {
        Application.targetFrameRate = 60;
		PlayManager.Instance.InIt ();
        imgButtom.transform.DOLocalMoveY (-800, 0).SetRelative (true);
        startButton.transform.DOLocalMoveY (-700, 0).SetRelative (true);
        textActiveColor = textStart.color;
        textStart.color = dimColor;
        imageTitle.DOFade (0f, 0f);

        if (PlayManager.Instance._isFirstPlay)
			imageTitle.transform.DOLocalMoveY (0f, 0f);
    }

	void Start () {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)GameDataManager.Instance.Data.Language];

        StartCoroutine (InAnimation ());
	}

	//In Animation
	IEnumerator InAnimation () {
		//Title Image Fade
		if (PlayManager.Instance._isFirstPlay)
			yield return new WaitForSeconds (1.5f);

		imageTitle.DOFade (1f, 2f).SetEase (Ease.Linear);

		if (PlayManager.Instance._isFirstPlay) {
			PlayManager.Instance._isFirstPlay = false;
			imageTitle.transform.DOLocalMoveY (90f, 1f).SetEase (Ease.InOutQuart).SetRelative (true).SetDelay (3f);
			yield return new WaitForSeconds (3.7f);
		}

		//Start Bottom Mennu Animation
		StartCoroutine (BottomMenuInAnimation ());
	}

	IEnumerator BottomMenuInAnimation () { 
		imgButtom.transform.DOLocalMoveY (800, 0.3f).SetEase (Ease.InOutCubic).SetRelative (true);
		yield return new WaitForSeconds (0.15f);
		startButton.transform.DOLocalMoveY (700, 0.25f).SetRelative (true);
		yield return new WaitForSeconds (0.35f);
		textStart.DOColor (new Color (textActiveColor.r, textActiveColor.g, textActiveColor.b, 1f), 0.5f).SetEase (Ease.OutCubic);
		startButton.transform.SetAsLastSibling ();
	}

	//OutAnimation
	IEnumerator OutAnimation (string name) {
		yield return null;
		//Title Image Fade
		imageTitle.DOKill ();
		imageTitle.DOFade (0f, 0.3f).SetEase (Ease.Linear);

		//Start Bottom Mennu Animation
		StartCoroutine (BottomMenuOutAnimation ());
	}

	IEnumerator BottomMenuOutAnimation () {
		startButton.transform.SetAsFirstSibling ();
		startButton.transform.DOLocalMoveY (-500, 0.2f).SetEase(Ease.InOutCubic).SetRelative (true);
		yield return new WaitForSeconds (0.1f);
		imgButtom.transform.DOLocalMoveY(-800, 0.3f).SetEase(Ease.InOutCubic).SetRelative(true).OnComplete(() =>
		{
            // 스테이지 메뉴 띄우기
            m_MainMenu.gameObject.SetActive (false);
            this.gameObject.SetActive(false);
            m_StageMenu.gameObject.SetActive(true);
            m_StageMenuController.SetActive(true);

			MainMenuInit();
        });        
    }

	private void MainMenuInit()
	{
        Color color = imageTitle.color;
        color.a = 1f;
        imageTitle.color = color;
        imageTitle.transform.localPosition = new Vector3(2.4f, 90f, 0f);

        startButton.transform.localPosition = new Vector3(0f, -352.5f, 0f);

        imgButtom.transform.localPosition = new Vector3(0f, -412.5f, 0f);
    }

    #region - Panel Load!!!

    public GameObject panleSettings;
	public GameObject panleExit;

	public Transform panelTransform;

	public void MenuSettings ()
	{ 
		LoadPanel (panleSettings, panleSettings.name);
	}
	
	public void MenuExit()
	{
        LoadPanel(panleExit, panleExit.name);
    }

	public void StartButton () {
		StartCoroutine (OutAnimation ("stage"));
	}
	
	private GameObject activePopup;
	void LoadPanel (GameObject panel, string name) { 
		GameObject panels = (GameObject) Instantiate (panel, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		panels.transform.SetParent (panelTransform, false);
		activePopup = panels;
		panels.name = name;
	}

	#endregion


}
