using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    private AudioSource _hitSource;
    
    [SerializeField] private AudioClip m_hitWallClip;
    [SerializeField] private float m_rotationAngle = 15.0f;

    [SerializeField] private GameObject m_LeftArrow;
    [SerializeField] private GameObject m_RightArrow;

    private bool m_EnterMouse = false;
    private bool m_IsMousePress = false;
    private bool m_IsMouseCoroutineActive = false;

    private NavMeshSurface m_NavMeshSurface;

    public bool IsMousePress { set =>  m_IsMousePress = value; }

    void Start()
    {
        if (null != GameObject.Find("Navigation"))
            m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();

        _hitSource = GetComponent<AudioSource>();

        _hitSource.volume = (float)GameDataManager.Instance.Data.SfxVolume;
    }

    void Update()
    {
        // 튜토리얼이 진행중이라면 상호작용 불가
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        if(m_EnterMouse)
        {
            if (Input.GetMouseButtonDown(1))
                MouseRButtonDown();
            else if (Input.GetMouseButtonUp(1))
                MouseRButtonUp();
            if (m_IsMousePress)
            {
                if (Input.GetKeyDown(KeyCode.A))
                    Rotation(-1);
                else if (Input.GetKeyDown(KeyCode.D))
                    Rotation(1);
            }
        }
    }
    private void OnMouseEnter()
    {
        m_EnterMouse = true;
    }
    private void OnMouseExit()
    {
        m_EnterMouse = false;
        m_LeftArrow.SetActive(false);
        m_RightArrow.SetActive(false);
    }

    private void MouseRButtonDown()
    {
        m_IsMousePress = true;
        m_LeftArrow.SetActive(true);
        m_RightArrow.SetActive(true);
    }

    public void MouseRButtonUp()
    {
        m_IsMousePress = false;
        m_LeftArrow.SetActive(false);
        m_RightArrow.SetActive(false);
    }

    public void Rotation(int num)
    {
        _hitSource.clip = m_hitWallClip;
        _hitSource.Play();
        if (num > 0)
            this.gameObject.transform.Rotate(new Vector3(0, m_rotationAngle, 0));
        else
            this.gameObject.transform.Rotate(new Vector3(0, -m_rotationAngle, 0));
        if (m_NavMeshSurface != null)
        {
            m_NavMeshSurface.BuildNavMesh();
        }
    }

    //public IEnumerator MouseDownDelay()
    //{
    //   while (m_IsMousePress && m_EnterMouse)
    //   {
    //       Rotation();
    //       yield return new WaitForSeconds(0.15f);
    //   }
    //   m_IsMouseCoroutineActive = false;
    // }
}