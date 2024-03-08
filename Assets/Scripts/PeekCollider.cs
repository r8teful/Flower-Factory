using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekCollider : Interactable {
    protected override void OnMouseDown() {
        CameraMovement.Instance.CameraPeek();
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
    }

    // Todo show when we actialy will peak, when we enter we should see if we are still on it, to update it, incase anything overwrites it
    protected override void OnMouseEnter() {
        if(CameraMovement.Instance.CanPeek())
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Look;
    }
}