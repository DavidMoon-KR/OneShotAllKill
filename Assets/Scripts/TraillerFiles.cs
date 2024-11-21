using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TraillerFiles : MonoBehaviour
{
    [SerializeField] private GameObject m_SoundManager;

    void Start()
    {
        if (TraillerController.Instance.IsPlayTrailler == false)
        {
            this.gameObject.SetActive(false);
            m_SoundManager.SetActive(true);
        }
    }
}
