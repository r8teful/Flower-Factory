using System.Collections;
using UnityEngine;

public class ControlRoomSequence : Sequencer {

    [SerializeField] private Transform _enterControlPannel;
    [SerializeField] private ClickAreaUI _back;
    private bool _hasEnteredControlPannel;


    protected override IEnumerator Sequence() {

        yield return new WaitUntil(() => _hasEnteredControlPannel);
        yield return new WaitForEndOfFrame();
        // Lock movement going back until we have finished minigame
        _back.CanInteract = false;
        _enterControlPannel.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;
        // Done, activate sortSequence
    }


    private void Awake() => CameraMovement.CurrentCameraPos += CameraPosChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;
    private void CameraPosChanged(Transform t) {
        if (t == _enterControlPannel) {
            _hasEnteredControlPannel = true;
        }
    }
}