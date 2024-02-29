using System.Collections;
using UnityEngine;

public class FlowerBox : Interactable {
    [SerializeField] private Flower.FlowerType _flowerBoxType;
    [SerializeField] private Transform _placePosition;

    protected override void OnMouseDown() {
        var inventoryItem = InventoryHandler.Instance.HoldingObject;
        if (inventoryItem == null) return;
        var f = inventoryItem.GetComponent<Flower>();
        if (f == null) return; // Not holding flower
        if(f.type.Equals(_flowerBoxType)) {
            // Holding flower we want, place it down now
            f.SetMeshRender(true);
            f.gameObject.transform.position = _placePosition.position;
        }
    }
}