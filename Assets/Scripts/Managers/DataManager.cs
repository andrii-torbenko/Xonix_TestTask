using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataManager {

    private static string _localFilePath = "/playerData.dat";
    public static string appDataPath { get { return Application.persistentDataPath + _localFilePath; } }
    private static Database _currentDataBase = new Database();
    private static PlayerData _currentPlayer = new PlayerData();

    public static void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(appDataPath);

        Database playersDataBase = new Database();

        foreach (PlayerData pData in _currentDataBase.allPlayers) {
            playersDataBase.allPlayers.Add(pData);
        }

        bf.Serialize(file, playersDataBase);

        file.Close();
    }

    public static void AddStat(uint score, string name, uint level) {
        var playerData = new PlayerData();
        _currentDataBase.allPlayers.Add(playerData);
        playerData.score = score;
        playerData.name = name;
        playerData.level = level;
        UIManager.UpdateScoretab();
    }

    public static void Load() {
        if (File.Exists(appDataPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(appDataPath, FileMode.Open);

            Database data = (Database)bf.Deserialize(file);
            foreach (PlayerData pData in data.allPlayers) {
                _currentDataBase.allPlayers.Add(pData);
            }
            file.Close();
        }
        else {
            _currentDataBase = new Database();
            _currentPlayer = new PlayerData();
        }
    }

    public static bool IsBestScore(uint score) {
        foreach (PlayerData pd in _currentDataBase.allPlayers) {
            if (pd.score >= score) return false;
        }
        return true;
    }

    public static bool isRecord {
        get
        {
            foreach (PlayerData pd in _currentDataBase.allPlayers) {
                if (pd.score >= _currentPlayer.score) return false;
            }
            return true;
        }
    }

    public static uint CurrentScore {
        get { return _currentPlayer.score; }
    }

    public static uint CurrentLevel {
        get { return _currentPlayer.level; }
    }

    public static int DBCount { get { return _currentDataBase.allPlayers.Count; } }

    public static string GetNameByIndex(int index) {
        return _currentDataBase.allPlayers[index].name;
    }

    public static uint GetScoreByIndex(int index) {
        return _currentDataBase.allPlayers[index].score;
    }

    public static uint GetLevelByIndex(int index) {
        return _currentDataBase.allPlayers[index].level;
    }

    public static Database currentData { get { return _currentDataBase; } }

    public static void Init() {
        if (!StateManager.isAppStarted) {
            Load();
        }
        else {
            throw new System.NotImplementedException("Can't initialize date manager more than once");
        }
    }
}
