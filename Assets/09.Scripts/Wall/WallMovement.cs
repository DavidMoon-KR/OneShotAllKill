using System.Collections;
using DG.Tweening;
using Unity.AI.Navigation;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    // 최대 +이동할 수 있는 거리
    [SerializeField] private float m_MaxDirection;

    // 최소 -이동할 수 있는 거리
    [SerializeField] private float m_MinDirection;

    // 사운드
    [SerializeField] private AudioClip m_HitArrowClip;
    private AudioSource m_HitSource;

    private bool m_EnterMouse = false;
    private bool m_IsClick = false;
    private bool m_IsMoveable = false;
    private Vector3 m_MouseP;
    private float m_Walldistance = 3.9f; //두 벽간의 거리
    private NavMeshSurface m_NavMeshSurface;
    RaycastHit m_RaycastHit;
    private float m_tempF;

    // 거울이 자동으로 움직이는 오토모드 추가
    [Header("AutoMode Settings")]
    public bool autoMode = false;
    public bool moveStartFromMin = true;
    public float moveDuration = 3f;

    private void Update()
    {
        if (autoMode) return;

        if (Input.GetMouseButtonUp(1))
        {
            m_IsClick = false;
            m_IsMoveable = false;
            PrintArrow();
        }

        // 튜토리얼이 진행중이라면 상호작용 불가
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) && m_EnterMouse)
        {
            MouseLDown();
        }

        if (m_IsMoveable)
            MoveUpdate();
    }

    void Start()
    {
        if (null != GameObject.Find("Navigation"))
            m_NavMeshSurface = GameObject.Find("Navigation").GetComponent<NavMeshSurface>();
        m_HitSource = GetComponent<AudioSource>();

        m_HitSource.volume = (float)GameDataManager.Instance.Data.SfxVolume;

        // 오토모드 구현 추가
        if (!autoMode) return;
        
        var firstDest = moveStartFromMin ? m_MaxDirection : m_MinDirection;
        var secondDest = moveStartFromMin ? m_MinDirection : m_MaxDirection;
        
        transform.localPosition = new Vector3(secondDest, transform.localPosition.y);
        
        var tween = DOTween.Sequence();
        tween.Append(gameObject.transform.DOLocalMoveX(firstDest, moveDuration).SetEase(Ease.Linear));
        tween.Append(gameObject.transform.DOLocalMoveX(secondDest, moveDuration).SetEase(Ease.Linear));
        tween.SetLoops(-1, LoopType.Restart);
    }

    private void OnMouseEnter()
    {
        m_EnterMouse = true;
    }

    private void OnMouseExit()
    {
        m_EnterMouse = false;
        if (m_EnterMouse == false && m_IsClick == false)
            m_IsMoveable = false;
    }

    private void MouseLDown()
    {
        // 튜토리얼이 진행중이라면 상호작용 불가
        // 게임이 일시정지인 상황에서는 행동 불가
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        m_IsClick = true;
        if (m_IsClick && m_EnterMouse)
            m_IsMoveable = true;
        PrintArrow();
    }

    // 벽 움직이기
    private void MoveUpdate()
    {
        if (GameManager.Instance.IsGamePause)
        {
            return;
        }

        m_MouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            -Camera.main.transform.position.z));
        if ((gameObject.transform.eulerAngles.y == 0 && this.gameObject.transform.position.x + 2 < m_MouseP.x) ||
            (gameObject.transform.eulerAngles.y == 270 && this.gameObject.transform.position.z + 2 < m_MouseP.z))
        {
            Debug.DrawRay(transform.position, transform.right * m_Walldistance * 2, Color.clear, 0.3f);
            if (Physics.Raycast(transform.position, transform.right, out m_RaycastHit, m_Walldistance * 2))
            {
                if (m_RaycastHit.transform.CompareTag("RotateWall"))
                {
                    if (Vector3.Distance(m_RaycastHit.transform.position, transform.position) < m_Walldistance * 2)
                        return;
                }
            }

            Debug.DrawRay(transform.position, transform.right * m_Walldistance, Color.clear, 0.3f);
            if (!Physics.Raycast(transform.position, transform.right, out m_RaycastHit, m_Walldistance) ||
                m_RaycastHit.transform.tag == "Cone")
            {
                m_HitSource.clip = m_HitArrowClip;
                m_HitSource.Play();
                gameObject.transform.Translate(Vector3.right * 2, Space.Self);
            }
        }
        else if ((gameObject.transform.eulerAngles.y == 0 && this.gameObject.transform.position.x - 2 > m_MouseP.x) ||
                 (gameObject.transform.eulerAngles.y == 270 && this.gameObject.transform.position.z - 2 > m_MouseP.z))
        {
            Debug.DrawRay(transform.position, transform.right * -1 * m_Walldistance * 2, Color.clear, 0.3f);
            if (Physics.Raycast(transform.position, transform.right * -1, out m_RaycastHit, m_Walldistance * 2))
            {
                if (m_RaycastHit.transform.CompareTag("RotateWall"))
                {
                    if (Vector3.Distance(m_RaycastHit.transform.position, transform.position) < m_Walldistance * 2)
                        return;
                }
            }

            Debug.DrawRay(transform.position, transform.right * -1 * m_Walldistance, Color.clear, 0.3f);
            if (!Physics.Raycast(transform.position, transform.right * -1, out m_RaycastHit, m_Walldistance) ||
                m_RaycastHit.transform.tag == "Cone")
            {
                m_HitSource.clip = m_HitArrowClip;
                m_HitSource.Play();
                gameObject.transform.Translate(Vector3.left * 2, Space.Self);
            }
        }

        if (m_NavMeshSurface != null)
        {
            m_NavMeshSurface.BuildNavMesh();
        }
    }

    private void PrintArrow()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject wall = hitInfo.collider.gameObject;
            if (m_IsClick == true)
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(m_IsClick);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(m_IsClick);
            }
            else
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(m_IsClick);
                this.gameObject.transform.GetChild(1).gameObject.SetActive(m_IsClick);
            }
        }
    }
}