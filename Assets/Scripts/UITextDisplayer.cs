using UnityEngine;

public class UITextDisplayer : StaticInstance<UITextDisplayer> {
    private GameObject _officeText;
    private GameObject _emailText;
    [SerializeField] private Transform _officeTextPosDisplay;
    [SerializeField] private Transform _officeMonitorPosDisplay;

    private void Start() {
        CameraMovement.CurrentCameraPos += CameraPosChanged;
        _officeText = transform.GetChild(0).gameObject;
        _emailText = transform.GetChild(1).gameObject;
        _officeText.SetActive(false);
        _emailText.SetActive(false);
    }

    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;

    private void CameraPosChanged(Transform t) {
        // If we are showing the text, and the cameraPos has changed, it means we moved away from the text thing
        if (_officeText.activeSelf) {
            _officeText.SetActive(false);
        }
        if (_emailText.activeSelf) {
            _emailText.SetActive(false);
        }
        if (t == _officeTextPosDisplay) {
            _officeText.SetActive(true);
        } else if (t== _officeMonitorPosDisplay) {
            _emailText.SetActive(true);
        }
    }
}