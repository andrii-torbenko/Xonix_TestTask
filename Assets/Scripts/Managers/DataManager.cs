using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PlayerData {
    public uint score = 0;
    public string name = "";
    public uint level = 0;

    public PlayerData() {
        score = 0;
        name = "";
        level = 0;
    }
}

[System.Serializable]
public class Database {
    public List<PlayerData> players;

    public Database() {
        players = new List<PlayerData>();
    }
}

public static class DataManager {

    private static string _localFilePath = "/playerData.dat";
    public static string appDataPath { get { return Application.persistentDataPath + _localFilePath; } }

    private static Database _currentDataBase = new Database();
    public static Database currentData { get { return _currentDataBase; } }
    public static int DBCount { get { return _currentDataBase.players.Count; } }

    public static void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(appDataPath);

        Database playersDataBase = new Database();

        foreach (PlayerData pData in _currentDataBase.players) {
            playersDataBase.players.Add(pData);
        }

        bf.Serialize(file, playersDataBase);

        file.Close();
    }

    public static void Load() {
        if (File.Exists(appDataPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(appDataPath, FileMode.Open);

            Database data = (Database)bf.Deserialize(file);
            foreach (PlayerData pData in data.players) {
                _currentDataBase.players.Add(pData);
            }
            file.Close();
        }
        else {
            _currentDataBase = new Database();
        }
    }

    private static void SortStats() {
        PlayerData temp;

        for (int i = 0; i < _currentDataBase.players.Count - 1; i++) {
            for (int j = 0; j < _currentDataBase.players.Count - i - 1; j++) {
                if (_currentDataBase.players[j].score < _currentDataBase.players[j + 1].score) {
                    temp = _currentDataBase.players[j];
                    _currentDataBase.players[j] = _currentDataBase.players[j + 1];
                    _currentDataBase.players[j + 1] = temp;
                }
            }
        }
    }

    public static void AddStat(uint score, string name, uint level) {
        var playerData = new PlayerData();
        _currentDataBase.players.Add(playerData);
        playerData.score = score;
        playerData.name = name;
        playerData.level = level;
        SortStats();
        UIManager.UpdateScoretab();
    }

    public static bool IsBestScore(uint score) {
        foreach (PlayerData pd in _currentDataBase.players) {
            if (pd.score >= score) return false;
        }
        return true;
    }

    public static string GetNameByIndex(int index) {
        return _currentDataBase.players[index].name;
    }

    public static uint GetScoreByIndex(int index) {
        return _currentDataBase.players[index].score;
    }

    public static uint GetLevelByIndex(int index) {
        return _currentDataBase.players[index].level;
    }


    public static void Init() {
        if (!StateManager.isAppStarted) {
            Load();
        }
        else {
            throw new System.NotImplementedException("Can't initialize date manager more than once");
        }
    }
}
