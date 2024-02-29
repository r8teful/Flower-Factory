using System.Collections;
using UnityEngine;

public class Flower : Interactable {

    [SerializeField] public FlowerType type;
    private MeshRenderer _meshRenderer;

    protected override void OnMouseDown() {
        // TODO Add rigid body
        if (InventoryHandler.Instance.HoldingObject != null) return;
        InventoryHandler.Instance.HoldingObject = gameObject;
        // TODO change cursor
        _meshRenderer.enabled = false;
    }

    private void Start() {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void SetMeshRender(bool b) {
        _meshRenderer.enabled = b;
    }

    public enum FlowerType {
        Yellow,
        Blue,
        Red
    }
}
