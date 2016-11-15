using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataManager {

    private static string _localFilePath = "/playerData.dat";
    public static string appDataPath { get { return Application.persistentDataPath + _localFilePath; } }
    private static PlayerData _currentData;

    public static void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(appDataPath);

        PlayerData data = new PlayerData();
        PlayerData.WriteData(data, _currentData);
        bf.Serialize(file, data);

        file.Close();
    }

    public static void Load() {
        if (File.Exists(appDataPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(appDataPath, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            PlayerData.WriteData(_currentData, data);
            file.Close();
        }
    }

    public static PlayerData currentData { get { return _currentData; } }

    public static void Init() {
        if (!StateManager.isAppStarted) {
            Load();
        }
        else {
            throw new System.NotImplementedException("Can't initialize date manager more than once");
        }
    }
}
