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
        if (t == _officeTextPosDisplay) {
            // Camera pos matches display pos
            Debug.Log("IT WORKED!");
            _officeText.SetActive(true);
            StartCoroutine(WaitUntilMoved());
        }
    }

    private IEnumerator WaitUntilMoved() {
        // TODO write it here and done!
        yield return null;
    }

    public void DisplayOfficeText(bool b) {
        _officeText.SetActive(b);
    }


}
