using UnityEngine;

public class DoorKey : Interactable {
    private bool _pickedUp;
    protected override void OnMouseDown() {
        if (_pickedUp) return;
        _pickedUp = true;
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Key; // Placeholder
        InventoryHandler.Instance.HoldingObject = gameObject;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        AudioController.Instance.PlaySound3D("key", transform.position, 0.5f);
    }
}
