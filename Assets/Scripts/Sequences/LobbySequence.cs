using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySequence : Sequencer {
    [SerializeField] private Elevator _elevator;
    [SerializeField] private Transform _doorLook;
    private bool _lookedAtDoor;
    private bool _doorUnlock;
    protected override IEnumerator Sequence() {

        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);
        yield return new WaitUntil(() => _lookedAtDoor);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);

        // Wait untill we unlocked the door
        yield return new WaitUntil(() => !_doorLook.GetComponent<LookView>().ConditionalMove);   
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[2]);
        
        // End of lobby, load underground


        yield return new WaitUntil(() => _elevator.ElevatorMoving);
        yield return new WaitForSeconds(10);
        //Spook stuff? Sounds, visual things to see that it is moving
        SceneHandler.Instance.LoadUnderGround();
    }

    private void Awake() => CameraMovement.CurrentCameraView += CameraViewChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;
    private void CameraViewChanged(Transform t) {
        if(t == _doorLook) {
            _lookedAtDoor = true;
        }
    }
}
