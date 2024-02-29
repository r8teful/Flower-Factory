using System.Collections;
using UnityEngine;

public class InventoryHandler : StaticInstance<InventoryHandler> {

    public GameObject HoldingObject{ get; set; }

    protected override void Awake() {
        HoldingObject = null;
        base.Awake();
    }

}