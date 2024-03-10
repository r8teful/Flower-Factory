using System.Collections;
using UnityEngine;

public class InventoryHandler : StaticInstance<InventoryHandler> {
    private GameObject _holdingObject; 

    public GameObject HoldingObject {
        get { return _holdingObject; }
        set { _holdingObject = value;
            if (value == null) CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
        }
    }
    protected override void Awake() {
        HoldingObject = null;
        base.Awake();
    }

}