using System.Collections;
using UnityEngine;

public class ControlRoomSequence : Sequencer {

    [SerializeField] private Transform _enterControlPannel;
    [SerializeField] private Transform _onStairs;
    [SerializeField] private ClickAreaUI _back;
    [SerializeField] private ControlPannel _controlPannel;
    private bool _hasEnteredControlPannel;
    private bool _hasBeenOnStairs;


    protected override IEnumerator Sequence() {

        yield return new WaitUntil(() => _hasEnteredControlPannel);
        yield return new WaitForEndOfFrame();
        // Lock movement going back until we have finished minigame
        _back.CanInteract = false;
        _controlPannel.MinigameStarted = true;
        _enterControlPannel.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;


        yield return new WaitUntil(() => _controlPannel.SequenceComplete);
        _back.CanInteract = true;
        // I should get out of here
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        _hasBeenOnStairs = false;
        yield return new WaitUntil(() => _hasBeenOnStairs);
        // Activate endgame

    }

    private void Awake() => CameraMovement.CurrentCameraPos += CameraPosChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;
    private void CameraPosChanged(Transform t) {
        if (t == _enterControlPannel) {
            _hasEnteredControlPannel = true;
        }
        if (t == _onStairs) {
            _hasBeenOnStairs = true;  
        }
    }
}