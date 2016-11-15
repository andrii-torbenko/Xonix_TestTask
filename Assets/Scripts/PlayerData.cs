﻿using UnityEngine;

[System.Serializable]
public class PlayerData {
    private uint _score;
    private string _name;
    private uint _level;

    public void ResetScore() {
        _score = 0;
    }

    public void ResetName() {
        _name = "";
    }

    public void ResetLevel() {
        _level = 0;
    }

    public void ResetAll() {
        _score = 0;
        _name = "";
        _level = 0;
    }

    public uint Score { get { return _score; } }
    public string Name { get { return _name; } }
    public uint Level { get { return _level; } }

    public void AddScore(uint score) {
        _score += score;
    }

    public void SetName(string name) {
        _name = name;
    }

    public void SetLevel(uint level) {
        _level = level;
    }

    public static void WriteData(PlayerData left, PlayerData right) {
        left._score = right._score;
        left._name = right._name;
        left._level = right._level;
    }

    public PlayerData(uint score, string name, uint level) {
        _score = score;
        _name = name;
        _level = level;
    }

    public PlayerData() {
        _score = 0;
        _name = "";
        _level = 0;
    }
}