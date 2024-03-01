using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Interactable {

    [SerializeField] public FlowerType type;
    private MeshRenderer[] _meshRenderer;

    protected override void OnMouseDown() {
        if (InventoryHandler.Instance.HoldingObject != null) return;
        switch (type) {
            case FlowerType.Yellow:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.FYellow;
                break;
            case FlowerType.Blue:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.FBlue;
                break;
            case FlowerType.Red:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.FRed;
                break;
            default:
                break;
        }
        SetMeshRender(false);
        InventoryHandler.Instance.HoldingObject = gameObject;
    }

    private void Start() {
        _meshRenderer = gameObject.GetComponentsInChildren<MeshRenderer>();
    }

    public void SetMeshRender(bool b) {
        foreach (var renderer in _meshRenderer) {
            renderer.enabled = b;
        }
    }

    public enum FlowerType {
        Yellow,
        Blue,
        Red
    }
}
