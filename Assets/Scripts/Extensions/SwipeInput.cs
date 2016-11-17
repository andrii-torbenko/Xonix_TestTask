using UnityEngine;
using System.Collections;

public static class SwipeInput {
    
    public static void SetDirection() {
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //        if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved) {
        //            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

        //            if (Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y)) {
        //                if (touchDeltaPosition.x > 0) {
        //                    Field.player.PlayerDirection = Enumerators.PlayerDirection.R;
        //                }
        //                else {
        //                    Field.player.PlayerDirection = Enumerators.PlayerDirection.L;
        //                }
        //            }
        //            else {
        //                if (touchDeltaPosition.y > 0) {
        //                    Field.player.PlayerDirection = Enumerators.PlayerDirection.U;
        //                }
        //                else {
        //                    Field.player.PlayerDirection = Enumerators.PlayerDirection.D;
        //                }

        //            }
        //        }
        //#elif UNITY_EDITOR
        //        if (Input.GetKeyDown(KeyCode.LeftArrow)) FieldManager.player.PlayerDirection = Enumerators.PlayerDirection.L;
        //        else if (Input.GetKeyDown(KeyCode.RightArrow)) FieldManager.player.PlayerDirection = Enumerators.PlayerDirection.R;
        //        else if (Input.GetKeyDown(KeyCode.UpArrow)) FieldManager.player.PlayerDirection = Enumerators.PlayerDirection.U;
        //        else if (Input.GetKeyDown(KeyCode.DownArrow)) FieldManager.player.PlayerDirection = Enumerators.PlayerDirection.D;
        //#endif
    }
}
