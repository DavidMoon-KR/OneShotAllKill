using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Language
{
    English = 0,
    Korean = 1,
}

[System.Serializable]
public class GameData
{
    // 설정
    public double SfxVolume = 0.5;
    public double BgmVolume = 0.5;    
    public bool CameraShakeEnabled = true;
    public Language Language = Language.English;

    // 스테이지 클리어 여부
    public bool[] stageCleared = new bool[17];
}

public class GameDataManager : MonoBehaviour
{
    // GameDataManager 스크립트를 인스턴스화 한 것
    private static GameDataManager m_Instance;
    public static GameDataManager Instance => m_Instance;

    private GameData m_Data;
    public GameData Data { get => m_Data; }
    private string m_SavePath;

    void Awake()
    {
        var obj = FindObjectsOfType<GameDataManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        m_Instance = GetComponent<GameDataManager>();

        m_SavePath = Path.Combine(Application.persistentDataPath, "GameData.Json");
        Load();
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(m_Data, true);
        File.WriteAllText(m_SavePath, json);
    }

    public void Load()
    {
        if (File.Exists(m_SavePath))
        {
            string json = File.ReadAllText(m_SavePath);
            m_Data = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            m_Data = new GameData();
        }
    }
}
