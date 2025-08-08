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
    [SerializeField] private GameObject m_HintSprite;   // ��Ʈ ��������Ʈ
    [SerializeField] private Transform m_WallTrans;     // �� ��ũ��Ʈ�� �����ִ� ���� Ʈ������

    [SerializeField] private List<HintInfo> m_HintInfos;    // ��Ʈ ��ȣ�� ���� ������
    private int m_NowHintCount;

    void Update()
    {
        if (HintManager.Instance == null)
        {
            return;
        }

        m_NowHintCount = HintManager.Instance.NowHintCount;

        // ��Ʈ�� Ȱ��ȭ �Ǿ��� ���� ��Ʈ ��ȣ�� Ȱ��ȭ ���ΰ� true���
        if (HintManager.Instance.IsActiveHint && m_HintInfos[m_NowHintCount].IsActive)
        {
            // ��Ʈ ��������Ʈ�� ��Ȱ��ȭ �Ǿ������� Ȱ��ȭ ��Ű�� ��Ʈ ��ġ ����
            if (!m_HintSprite.activeSelf)
            {
                m_HintSprite.SetActive(true);                

                // ���� ��Ʈ ��ȣ�� Ȱ��ȭ ���ΰ� true���
                if (m_HintInfos[m_NowHintCount].IsActive)
                {
                    Vector3 temppos = m_HintSprite.transform.localPosition;
                    temppos.x = m_HintInfos[m_NowHintCount].XPos;
                    m_HintSprite.transform.localPosition = temppos;

                    m_HintSprite.transform.rotation = Quaternion.Euler(90, m_HintInfos[m_NowHintCount].RotateDegree, 0);
                }
            }

            // ���̶� ��Ʈ�� �� �ùٸ��� ��ġ�Ͽ����� üũ
            WallCompareHint();
        }
        else
        {
            // ��Ʈ ��������Ʈ�� Ȱ��ȭ �Ǿ������� ��Ȱ��ȭ ��Ű��
            if (m_HintSprite.activeSelf)
            {
                m_HintSprite.SetActive(false);
            }
        }
    }

    // ���̶� ��Ʈ�� �� �ùٸ��� ��ġ�Ͽ����� üũ
    private void WallCompareHint()
    {
        // ���� ��Ʈ ��ġ�� �ùٸ��� ��ġ �Ͽ��ٸ�
        if (MathF.Abs(m_WallTrans.localPosition.x - m_HintSprite.transform.localPosition.x) <= 1)
        {
            // ���� ȸ�� ���̸� ������ üũ
            if (m_WallTrans.CompareTag("RotateWall")
                && MathF.Abs(m_WallTrans.eulerAngles.y - m_HintSprite.transform.eulerAngles.y) >= 10
                && MathF.Abs(m_WallTrans.eulerAngles.y - (m_HintSprite.transform.eulerAngles.y + 180)) >= 10)
            {
                return;
            }
            // �ùٸ��� ��ġ�� �� 1 ����
            HintManager.Instance.HintWallCount += 1;
        }
    }
}
