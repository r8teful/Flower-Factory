using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : Interactable {
    protected override void OnMouseDown() {
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Key; // Placeholder
        InventoryHandler.Instance.HoldingObject = gameObject;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        AudioController.Instance.PlaySound3D("key", transform.position, 0.5f);
    }
}
