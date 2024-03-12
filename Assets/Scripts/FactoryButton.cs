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
        if(_connectedDevice.gameObject.GetComponent<Conveyor>() != null) {
            // Elevator button
            AudioController.Instance.PlaySound3D("buttonPushHeavy", transform.position, 0.6f, distortion: new AudioParams.Distortion(false, true));
        } else {
            AudioController.Instance.PlaySound3D("buttonPush", transform.position, 0.6f);
        }
        mButton.ActivateButton();
        _connectedDevice.ButtonClicked();
        _hasPressed = true;
    }

    private void Start() {
        mButton = GetComponentInChildren<MovingButton>();
        mButton.MoveDistance = 0.2f;
    }
}
