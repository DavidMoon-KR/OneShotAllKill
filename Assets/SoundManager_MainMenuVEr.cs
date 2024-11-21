using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_MainMenuVEr : MonoBehaviour
{
    void Start()
    {
        if (TraillerController.Instance.IsPlayTrailler == false)
        {
            this.gameObject.SetActive(true);
        }
    }
}
