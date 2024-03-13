using System;
using System.Collections;
using UnityEngine;

public class ControlRoomSequence : Sequencer {

    [SerializeField] private Transform _enterControlPannel;
    [SerializeField] private Transform _onStairs;
    [SerializeField] private LookView _preventBackwards;
    [SerializeField] private DoubleDoor _controlDoor;
    [SerializeField] private Transform _enterControlPannelDoor;
    [SerializeField] private ClickAreaUI _back;
    [SerializeField] private ControlPannel _controlPannel;
    [SerializeField] private ParticleSystem[] _fans;
    private bool _hasEnteredControlPannel;
    private bool _hasBeenOnStairs;
    private bool _hasEnteredControlPannelDoor;

    protected override IEnumerator Sequence() {
        _enterControlPannel.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;
        _controlPannel.StartPannel();
        yield return new WaitUntil(() => _hasEnteredControlPannelDoor);
        AudioController.Instance.PlaySound2D("controlRoom", 0.5f);

        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        FindObjectOfType<CreatureSound>().StartCreatureSound();
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);
        _enterControlPannel.parent.GetChild(1).GetComponent<BoxCollider>().enabled = true;
    
        yield return new WaitUntil(() => _hasEnteredControlPannel);
        yield return new WaitForEndOfFrame();
        _back.Lock = true;
        _back.CanInteract = false;
        // Lock movement going back until we have finished minigame
        CameraMovement.Instance.CurrentView = _enterControlPannel;
        _controlPannel.MinigameStarted = true;
        _enterControlPannel.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;
    
    
        yield return new WaitUntil(() => _controlPannel.SequenceComplete);
        _back.Lock = false;
        _back.CanInteract = true;
        // I should get out of here
        _enterControlPannel.parent.GetChild(1).GetComponent<BoxCollider>().enabled = true;


        _fans[0].Stop();
        _fans[1].Stop();
        yield return new WaitForSeconds(2);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        _hasBeenOnStairs = false;
        yield return new WaitUntil(() => _hasBeenOnStairs);
        // Close doors and have them stay closed
        _preventBackwards.ConditionalMove = true;
        _controlDoor.CloseDoors();
        _controlDoor.ConditionalOpen = true;
        // Activate endgame

    }

    private void Awake() {
        CameraMovement.CurrentCameraPos += CameraPosChanged;
        CameraMovement.CurrentCameraView += CameraViewChanged;
    
    } 
    private void OnDestroy() {
        CameraMovement.CurrentCameraPos -= CameraPosChanged;
        CameraMovement.CurrentCameraView -= CameraViewChanged;
    }

    private void CameraViewChanged(Transform t) {
        if (t == _enterControlPannel && !_hasEnteredControlPannel) {
            _hasEnteredControlPannel = true;
        }
    }

    private void CameraPosChanged(Transform t) {
        
        if (t == _enterControlPannelDoor) {
            _hasEnteredControlPannelDoor = true;
        }
        if (t == _onStairs) { // actually just ouside control room
            _hasBeenOnStairs = true;  
        }
    }
}