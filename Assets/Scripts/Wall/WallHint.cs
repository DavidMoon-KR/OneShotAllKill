using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class HintInfo
{
    public int XPos;
    public int RotateDegree;
    public bool IsActive;
}

public class WallHint : MonoBehaviour
{
    [SerializeField] private GameObject m_HintSprite;   // 힌트 스프라이트
    [SerializeField] private Transform m_WallTrans;     // 이 스크립트와 관련있는 벽의 트랜스폼

    [SerializeField] private List<HintInfo> m_HintInfos;    // 힌트 번호에 따른 정보들
    private int m_NowHintCount;

    void Update()
    {
        m_NowHintCount = GameManager.Instance.NowHintCount - 1;

        // 힌트가 활성화 되었고 현재 힌트 번호의 활성화 여부가 true라면
        if (GameManager.Instance.IsActiveHint && m_HintInfos[m_NowHintCount].IsActive)
        {
            // 힌트 스프라이트가 비활성화 되어있으면 활성화 시키고 힌트 위치 설정
            if (!m_HintSprite.activeSelf)
            {
                m_HintSprite.SetActive(true);                

                // 현재 힌트 번호의 활성화 여부가 true라면
                if (m_HintInfos[m_NowHintCount].IsActive)
                {
                    Vector3 temppos = m_HintSprite.transform.localPosition;
                    temppos.x = m_HintInfos[m_NowHintCount].XPos;
                    m_HintSprite.transform.localPosition = temppos;

                    m_HintSprite.transform.rotation = Quaternion.Euler(90, m_HintInfos[m_NowHintCount].RotateDegree, 0);
                }
            }

            // 벽이랑 힌트를 비교 올바르게 위치하였는지 체크
            WallCompareHint();
        }
        else
        {
            // 힌트 스프라이트가 활성화 되어있으면 비활성화 시키기
            if (m_HintSprite.activeSelf)
            {
                m_HintSprite.SetActive(false);
            }
        }
    }

    // 벽이랑 힌트를 비교 올바르게 위치하였는지 체크
    private void WallCompareHint()
    {
        // 벽이 힌트 위치에 올바르게 위치 하였다면
        if (MathF.Abs(m_WallTrans.localPosition.x - m_HintSprite.transform.localPosition.x) <= 1)
        {
            // 만약 회전 벽이면 각도도 체크
            if (m_WallTrans.CompareTag("RotateWall")
                && MathF.Abs(m_WallTrans.eulerAngles.y - m_HintSprite.transform.eulerAngles.y) >= 10
                && MathF.Abs(m_WallTrans.eulerAngles.y - (m_HintSprite.transform.eulerAngles.y + 180)) >= 10)
            {
                return;
            }
            // 올바르게 위치한 수 1 증가
            GameManager.Instance.HintWallCount += 1;
        }
    }
}
