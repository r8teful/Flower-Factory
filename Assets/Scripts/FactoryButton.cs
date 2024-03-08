using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryButton : Interactable {
    private MovingButton mButton;
    [SerializeField] private ButtonDevice _connectedDevice;
    private bool _hasPressed;

    [SerializeField] private bool _allowPress;
    public bool AllowPress { 
        get { return _allowPress; }
        set { _allowPress = value; }
    }

    protected override void OnMouseDown() {
        if (_hasPressed || !AllowPress) return;
        if (_connectedDevice == null) return;
        mButton.ActivateButton();
        _connectedDevice.ButtonClicked();
        _hasPressed = true;
    }

    private void Start() {
        mButton = GetComponentInChildren<MovingButton>();
    }
}
