using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataManager {

    private static string _localFilePath = "/playerData.dat";
    public static string appDataPath { get { return Application.persistentDataPath + _localFilePath; } }
    private static Database _currentDataBase = new Database();

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
        }
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
