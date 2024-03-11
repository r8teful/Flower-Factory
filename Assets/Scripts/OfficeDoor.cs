using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OfficeDoor : Interactable {

    private Transform _door;
    [SerializeField] private LookView _conditionCanMove;
    private bool _canOpen;

    private void Start() {
        CameraMovement.CurrentCameraView+= CameraViewChanged;
        _door = transform.GetChild(1);
        if(GameManager.Instance.PlayerProgression < 2) {
            _door.localRotation = Quaternion.identity;
        }
    }


    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;

    private void CameraViewChanged(Transform t) {
        if(t == _conditionCanMove.gameObject.transform) {
            // We are looking at the door and we can open it
            _canOpen = true;
        } else {
            _canOpen = false;
        }
    }

    protected override void OnMouseDown() {
        if (InventoryHandler.Instance.HoldingObject == null || !_canOpen) return;
        // Can move forward now
        var a = AudioController.Instance.PlaySound3D("DoorOpenOffice", transform.position, 0.4f);
        a.AddComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.Hallway;
        
        InventoryHandler.Instance.HoldingObject = null;
        _door.localRotation = Quaternion.Euler(0, -70,0);
        _conditionCanMove.ConditionalMove = false;
    }
}
