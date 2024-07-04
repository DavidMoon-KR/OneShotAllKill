using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text m_BulletCount;
    [SerializeField]
    private GameObject m_RestartToMessage;
    [SerializeField]
    private GameObject m_BlackScreen;
    [SerializeField]
    private GameObject m_HightlightedNext;
    [SerializeField]
    private GameObject m_HightlightedRestart;

    public bool m_MissionComplete = false;   // �������� Ŭ���� ���� �Ǵ�
    private bool m_OneChecking = true;       // ��� �ִϸ��̼��� �ѹ��� ������ �ϴ� ����

    private static UIManager m_Instance;
    public static UIManager Instance => m_Instance;

    void Start()
    {
        m_Instance = GetComponent<UIManager>();
    }

    // ź ���� ǥ��
    public void BulletCountSet(int p_bullet)
    {
        m_BulletCount.text = p_bullet.ToString();
    }

    public void GameOverMessage()
    {
        StartCoroutine(GameOverAnim());
    }

    private IEnumerator GameOverAnim()
    {
        // �������� Ŭ���� ���� ���
        if (m_MissionComplete == false && m_OneChecking == true)
        {
            m_RestartToMessage.gameObject.SetActive(true);
            m_OneChecking = false;
        }
        // �������� Ŭ���� �� ���
        else if (m_MissionComplete == true && m_OneChecking == true)
        {
            m_BlackScreen.SetActive(true);
            yield return new WaitForSeconds(2.3f);
            m_HightlightedNext.SetActive(true);
            m_HightlightedRestart.SetActive(true);
            m_OneChecking = false;
        }
    }

    // ���� ���������� �̵�
    public void NextStageLoadScene()
    {
        SceneManager.LoadScene(GameManager.Instance.m_SceneNumber + 1);
    }

    // ���θ޴� �� �ε�
    public void MainMenuLoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
