using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : Interactable {
    protected override void OnMouseDown() {
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.FYellow; // Placeholder
        InventoryHandler.Instance.HoldingObject = gameObject;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
}
