using System;
using Steamworks;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    public uint appID = 3748000;     //스팀 앱아이디
    
    private static SteamManager _instance;
    private static bool _initialized;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject); // 다른 인스턴스가 이미 존재하면 제거
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            SteamClient.Init(appID);
            _initialized = true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);

            if (!Application.isEditor && !Debug.isDebugBuild)
            {
                Application.Quit();
            }
        }
    }

    private void OnDestroy()
    {
        if (_instance == this && _initialized && SteamClient.IsValid)
        {
            SteamClient.Shutdown();
            _initialized = false;
        }
    }
}