using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Interactable {

    [SerializeField] public FlowerType type;
    private MeshRenderer[] _meshRenderer;
    public bool IsCrushMode;
    private bool _isCrushed;

    protected override void OnMouseDown() {
        if (IsCrushMode) {
            if (InventoryHandler.Instance.HoldingObject == null) {
                if (_isCrushed) return;
                // Turn into powder
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                SetMeshRender(false);
                FlowerSortManager.Instance.AddPointsCrushed();
                AudioController.Instance.PlaySound2D("crush", 0.2f);
                _isCrushed = true;
                return;
            }
      
        } else {
            if (InventoryHandler.Instance.HoldingObject != null) return; // Inventory full
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
            AudioController.Instance.PlaySound2D("pickup",0.3f);
            InventoryHandler.Instance.HoldingObject = gameObject;
        }
    }

    private void Start() {
        _meshRenderer = gameObject.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
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
