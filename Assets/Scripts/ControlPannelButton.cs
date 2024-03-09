using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPannelButton : Interactable {
    protected override void OnMouseDown() {
        gameObject.GetComponent<MovingButtonLocal>().ActivateButton();
        gameObject.GetComponentInParent<ControlPannel>().ButtonPressed(transform.GetSiblingIndex());
    }

}
