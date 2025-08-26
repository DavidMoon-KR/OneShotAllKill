using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_StageMessage;
    [SerializeField] private GameObject m_StartMessage;
    [SerializeField] private GameObject m_WaitSecondsImg;

    void OnEnable()
    {
        GameManager.Instance.IsGamePause = true;
        m_StageMessage.text = $"Stage {GameManager.Instance.SceneNumber}";

        Invoke("StartMsg", 1.3f);
        Invoke("Hide", 2.9f);
    }

    private void StartMsg()
    {
        m_StartMessage.SetActive(true);
    }

    private void Hide()
    {
        Color TempColor = this.GetComponent<Image>().color;
        TempColor.a = 0;
        this.GetComponent<Image>().color = TempColor;
        m_StartMessage.SetActive(false);
        GameManager.Instance.IsGamePause = false;
        GameManager.Instance.RemoveBlur();

        StartCoroutine(WaitSeconds());
    }

    private IEnumerator WaitSeconds()
    {
        m_WaitSecondsImg.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
        m_WaitSecondsImg.SetActive(false);

        Color TempColor = this.GetComponent<Image>().color;
        TempColor.a = 1;
        this.GetComponent<Image>().color = TempColor;
    }
}
