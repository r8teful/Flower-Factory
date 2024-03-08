using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextDisplayer : StaticInstance<UITextDisplayer> {
    private GameObject _officeText;
    [SerializeField] private Transform _officeTextPosDisplay;

    private void Start() {
        CameraMovement.CurrentCameraPos += CameraPosChanged;
        _officeText = transform.GetChild(0).gameObject;
        _officeText.SetActive(false);
    }

    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;

    private void CameraPosChanged(Transform t) {
        // If we are showing the text, and the cameraPos has changed, it means we moved away from the text thing
        if (_officeText.activeSelf) {
            _officeText.SetActive(false);
        }
        if (t == _officeTextPosDisplay) {
            _officeText.SetActive(true);
        }
    }

}
