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
        _controlPannel.StartPannel();
        yield return new WaitUntil(() => _hasEnteredControlPannelDoor);
        AudioController.Instance.PlaySound2D("controlRoom", 0.5f);
        FindObjectOfType<CreatureSound>().StartCreatureSound();
        // TODO DEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUG
    
        yield return new WaitUntil(() => _hasEnteredControlPannel);
        yield return new WaitForEndOfFrame();
        // Lock movement going back until we have finished minigame
        _back.CanInteract = false;
        _controlPannel.MinigameStarted = true;
        _enterControlPannel.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false;
    
    
        yield return new WaitUntil(() => _controlPannel.SequenceComplete);
        _back.CanInteract = true;
        // I should get out of here
       

        // TODO DEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUGDEBUG

        _fans[0].Stop();
        _fans[1].Stop();
        yield return new WaitForSeconds(2);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        _hasBeenOnStairs = false;
        yield return new WaitUntil(() => _hasBeenOnStairs);
        // Close doors and have them stay closed
        _preventBackwards.ConditionalMove = true;
        _controlDoor.CloseDoors();
        _controlDoor.ConditionalOpen = true;
        // Activate endgame

    }

    private void Awake() => CameraMovement.CurrentCameraPos += CameraPosChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;
    private void CameraPosChanged(Transform t) {
        if (t == _enterControlPannel) {
            _hasEnteredControlPannel = true;
        }
        if (t == _enterControlPannelDoor) {
            _hasEnteredControlPannelDoor = true;
        }
        if (t == _onStairs) { // actually just ouside control room
            _hasBeenOnStairs = true;  
        }
    }
}