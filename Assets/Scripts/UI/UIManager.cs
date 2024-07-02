using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _bulletCount;
    [SerializeField]
    private GameObject _restartToMessage;
    [SerializeField]
    private GameObject _blackScreen;
    [SerializeField]
    private GameObject _hightlightedNext;
    [SerializeField]
    private GameObject _hightlightedRestart;

    public bool _missionComplete = false; // 스테이지 클리어 여부 판단
    private bool _oneChecking = true;   // 결과 애니메이션이 한번만 나오게 하는 변수

    private static UIManager _instance;
    public static UIManager Instance => _instance;


    // Start is called before the first frame update
    void Start()
    {
        _instance = GetComponent<UIManager>();
    }

    //탄 개수 표시
    public void BulletCountSet(int _bullet)
    {
        _bulletCount.text = _bullet.ToString();
    }

    public void GameOverMessage()
    {
        StartCoroutine(GameOverAnim());
    }

    private IEnumerator GameOverAnim()
    {
        //스테이지 클리어 못할 경우
        if (_missionComplete == false && _oneChecking == true)
        {
            _restartToMessage.gameObject.SetActive(true);
            _oneChecking = false;
        }
        //스테이지 클리어 한 경우
        else if (_missionComplete == true && _oneChecking == true)
        {
            _blackScreen.SetActive(true);
            yield return new WaitForSeconds(2.3f);
            _hightlightedNext.SetActive(true);
            _hightlightedRestart.SetActive(true);
            _oneChecking = false;
        }
    }
    //다음 스테이지로 이동
    public void NextStageLoadScene()
    {
        SceneManager.LoadScene(GameManager.Instance._sceneNumber + 1);
    }
    //메인메뉴 씬 로드
    public void MainMenuLoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
