using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool m_IsGameOver = false;       // ������ �������� �Ǵ��ϴ� ����
    public bool m_IsFailed = false;          // �������� Ŭ���� �����ߴ��� �Ǵ��ϴ� ����
    public bool m_HasExplosioned = false;    // �������� ������ ������ �Ͼ���� �Ǵ��ϴ� ����
    public bool m_HasNotAmmo = false;        // �÷��̾ ź�� �����ϰ� �ִ��� Ȯ���ϴ� ����

    // �޸ӳ��̵尡 ���������� �����ϴ��� �Ǵ��ϴ� ����
    [SerializeField]
    private bool m_IsHumanoid;

    public int m_Targets;            // �������� ���� �� Ÿ�� ����
    private int m_TurretCount;       // �������� ���� �ͷ� ����
    private int m_HumanoidCount;     // �������� ���� �޸ӳ��̵� ����
    public int m_SceneNumber;        // �� �������� �ܰ�
    public Vector3 m_ExplosionedPos; // ������ �Ͼ ��ġ

    // ������ ������ ���� ��� ��ٸ��� �ð�
    [SerializeField]
    private float m_GameOverDelay;

    // Ÿ���� �����ϴ� �� ��� ��ٸ��� �ð�
    [SerializeField]
    public float m_DelayExplosion;

    // ���ӸŴ��� ��ũ��Ʈ�� �ν��Ͻ�ȭ �� ��
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    void Start()
    {
        m_Instance = GetComponent<GameManager>();
        m_TurretCount = GameObject.FindGameObjectsWithTag("Turret").Length;
        m_HumanoidCount = GameObject.FindGameObjectsWithTag("Humanoid").Length;
        m_Targets = m_TurretCount + m_HumanoidCount;
    }

    void Update()
    {
        // ���� �� Ÿ���� ����, �÷��̾ ������ ź���� ���ٸ�
        if(m_Targets == 0 || m_HasNotAmmo == true)
        {
            GameOver();
        }

        // ������ �Ͼ��, �޸ӳ��̵忡�� �ش� ��ġ�� ��ǥ�� �˷���
        if(m_HasExplosioned == true && m_IsHumanoid == true)
        {
            GameObject[] humanoids = GameObject.FindGameObjectsWithTag("Humanoid");
            foreach(var humanoid in humanoids)
            {
                Humanoid currentHumanoid = humanoid.GetComponent<Humanoid>();
                currentHumanoid.m_ExplosionedPos = m_ExplosionedPos;
                
                // Ÿ�� ���� true
                currentHumanoid.m_ExplosionDetection = true;
            }
            m_HasExplosioned = false;
        }

        // �����
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(m_SceneNumber);
        }

        // ������ ������ Ŭ���� ������ ���
        if(m_IsGameOver == true && m_IsFailed == false)
        {
            UIManager.Instance.m_MissionComplete = true;
            UIManager.Instance.GameOverMessage();
        }
        // ������ ������ Ŭ���� ������ ���
        else if(m_IsGameOver == true && m_IsFailed == true)
        {
            UIManager.Instance.m_MissionComplete = false;
            UIManager.Instance.GameOverMessage();
        }
    }

    // ���ӿ���
    public void GameOver()
    {
        StartCoroutine(GameOverNow());
    }

    public IEnumerator GameOverNow()
    {
        yield return new WaitForSeconds(3.0f);

        // Ÿ���� �������
        if (m_Targets == 0)
        {
            m_IsGameOver = true;
            m_IsFailed = false;
        }
        // ���� ���
        else
        {
            m_IsGameOver = true;
            m_IsFailed = true;
        }
    }
}
