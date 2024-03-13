using System.Collections;
using UnityEngine;

// so bad but this is last day and who is going to care?
public class UITextDisplayerUnderground : MonoBehaviour {
    private GameObject _noteText;
    [SerializeField] private Transform _noteTextPosDisplay;
    private void Start() {
        CameraMovement.CurrentCameraPos += CameraPosChanged;
        _noteText = transform.GetChild(0).gameObject;
        _noteText.SetActive(false);
    }

    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;

    private void CameraPosChanged(Transform t) {
        // If we are showing the text, and the cameraPos has changed, it means we moved away from the text thing
        if (_noteText.activeSelf) {
            _noteText.SetActive(false);
        }
        if (t == _noteTextPosDisplay) {
            _noteText.SetActive(true);
        } 
    }
}