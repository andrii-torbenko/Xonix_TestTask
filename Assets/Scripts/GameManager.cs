using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
    
    void Awake() {
        Field.Init();
        AudioManager.Init();
    }

    void Start() {
        Field.GenerateField();
        Field.AddEnemy();
        Field.UpdateField();
    }

    void Update() {
        SwipeInput.SetDirection();
        Field.UpdatePositions();
    }
}

