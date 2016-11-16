using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public static class GameManager {
    
    //public static void Init() {
    //    FieldManager.Init();
    //    AudioManager.Init();
    //}

    public static void Start() {
        FieldManager.GenerateField();
        FieldManager.AddEnemy();
        FieldManager.UpdateField();
    }

    public static void Update() {
        SwipeInput.SetDirection();
        FieldManager.UpdatePositions();
    }
}

