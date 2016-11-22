using UnityEngine;
using System.Collections;

public static class SwipeInput {

    public static void SetDirection() {
#if !UNITY_ANDROID && !UNITY_EDITOR
                if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved) {
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                    if (Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y)) {
                        if (touchDeltaPosition.x < 0) {
                             BaseSetDir(Enumerators.PlayerDirection.R);
                }
                        else {
                    BaseSetDir(Enumerators.PlayerDirection.L);
                }
                    }
                    else {
                        if (touchDeltaPosition.y < 0) {
                    BaseSetDir(Enumerators.PlayerDirection.U);
                }
                        else {
                    BaseSetDir(Enumerators.PlayerDirection.D);
                }

                    }
                }
#elif UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            BaseSetDir(Enumerators.PlayerDirection.R);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            BaseSetDir(Enumerators.PlayerDirection.L);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            BaseSetDir(Enumerators.PlayerDirection.D);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            BaseSetDir(Enumerators.PlayerDirection.U);
        }
#endif
    }

    private static void BaseSetDir(Enumerators.PlayerDirection playerDirection) {
        Enumerators.PlayerDirection reversePlayerDir;

        switch (playerDirection) {
            case Enumerators.PlayerDirection.R:
                reversePlayerDir = Enumerators.PlayerDirection.L;
                break;
            case Enumerators.PlayerDirection.L:
                reversePlayerDir = Enumerators.PlayerDirection.R;
                break;
            case Enumerators.PlayerDirection.D:
                reversePlayerDir = Enumerators.PlayerDirection.U;
                break;
            case Enumerators.PlayerDirection.U:
                reversePlayerDir = Enumerators.PlayerDirection.D;
                break;
            default:
                throw new System.Exception();
        }

        if (GameManager.GetPlayerDirection() == playerDirection && GameManager.IsPlayerMooving) {
            if (GameManager.IsPlayerLeavingTrack) {
                GameManager.Die();
            }
            else {
                GameManager.SetPlayerDirection(reversePlayerDir);
            }
        }
        else {
            GameManager.IsPlayerMooving = true;
            GameManager.SetPlayerDirection(reversePlayerDir);
        }
    }
}
