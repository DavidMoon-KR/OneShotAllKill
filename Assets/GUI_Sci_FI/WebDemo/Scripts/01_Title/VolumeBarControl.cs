using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Rendering;

public enum SoundType
{
    SfxSound = 0,
    BgmSound = 1,
}

public class VolumeBarControl : MonoBehaviour, IPointerUpHandler{

    public Slider slider;
    public Scrollbar scrollbar;

    public SoundType m_SoundType;

    void Start()
    {
        switch (m_SoundType)
        {
            case SoundType.SfxSound:
                slider.value = (float)GameDataManager.Instance.Data.SfxVolume;
                scrollbar.value = (float)GameDataManager.Instance.Data.SfxVolume;
                break;
            case SoundType.BgmSound:
                slider.value = (float)GameDataManager.Instance.Data.BgmVolume;
                scrollbar.value = (float)GameDataManager.Instance.Data.BgmVolume;
                break;
            default:
                break;
        }        
    }

    public void ScrollBarValueChange()
    {
        slider.value = scrollbar.value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        double volume = Math.Round(slider.value, 1);

        // 데이터 저장
        switch (m_SoundType)
        {
            case SoundType.SfxSound:
                GameDataManager.Instance.Data.SfxVolume = volume;
                break;
            case SoundType.BgmSound:
                GameDataManager.Instance.Data.BgmVolume = volume;
                SoundManager.Instance.ChagneBgmSound((float)volume);
                break;
            default:
                break;
        }
        GameDataManager.Instance.Save();

        
    }
}
