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

    public bool _missionComplete = false; // �������� Ŭ���� ���� �Ǵ�
    private bool _oneChecking = true;   // ��� �ִϸ��̼��� �ѹ��� ������ �ϴ� ����

    private static UIManager _instance;
    public static UIManager Instance => _instance;


    // Start is called before the first frame update
    void Start()
    {
        _instance = GetComponent<UIManager>();
    }

    //ź ���� ǥ��
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
        //�������� Ŭ���� ���� ���
        if (_missionComplete == false && _oneChecking == true)
        {
            _restartToMessage.gameObject.SetActive(true);
            _oneChecking = false;
        }
        //�������� Ŭ���� �� ���
        else if (_missionComplete == true && _oneChecking == true)
        {
            _blackScreen.SetActive(true);
            yield return new WaitForSeconds(2.3f);
            _hightlightedNext.SetActive(true);
            _hightlightedRestart.SetActive(true);
            _oneChecking = false;
        }
    }
    //���� ���������� �̵�
    public void NextStageLoadScene()
    {
        SceneManager.LoadScene(GameManager.Instance._sceneNumber + 1);
    }
    //���θ޴� �� �ε�
    public void MainMenuLoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
