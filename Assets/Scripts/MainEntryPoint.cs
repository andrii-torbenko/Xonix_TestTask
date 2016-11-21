using UnityEngine;
using System.Collections;

public class MainEntryPoint : MonoBehaviour {
    

    void Awake() {
        StateManager.Init();
    }

    // Use this for initialization
    void Start() {
    }

    void Update() {
        TimeManager.Update();
        if (!GameManager.IsPaused) {
            SwipeInput.SetDirection();
            GameManager.Update();
        }
    }

    void OnApplicationQuit() {
        StateManager.OnCloseApp();
    }
}
