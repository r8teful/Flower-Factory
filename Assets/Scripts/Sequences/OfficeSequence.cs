using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSequence : Sequencer {

    [SerializeField] private Transform _exitLook;
    [SerializeField] private Elevator _elevator;
    private bool _lookedAtExit;
    private bool _lookedAtExitDialoguePlayed;

    protected override IEnumerator Sequence() {
        _elevator.ElevatorMoving = false;
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);

        while(!_elevator.ElevatorMoving) {
            if (_lookedAtExit && !_lookedAtExitDialoguePlayed) {
                // "I should really try to fix this" blah blah
                DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);
                _lookedAtExitDialoguePlayed = true;
            }


            yield return null;
        }
        // Going back down, quicker elevator now
        yield return new WaitForSeconds(3);
        //Spook stuff? Sounds, visual things to see that it is moving

        SceneHandler.Instance.LoadUnderGround();

    }

    private void Awake() => CameraMovement.CurrentCameraView += CameraViewChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;
    private void CameraViewChanged(Transform t) {
        if (t == _exitLook) {
            _lookedAtExit = true;
        }
    }
}