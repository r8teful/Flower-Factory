using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSortSequence : Sequencer {
    [SerializeField] private ConveyorThree _conveyorThree;
    [SerializeField] private Conveyor _conveyorSort;
    [SerializeField] private Elevator _elevator;
    [SerializeField] private ClickAreaUI _clickAreaLeft;
    [SerializeField] private ClickAreaUI _clickAreaRight;
    [SerializeField] private Transform _sortPosView;
    [SerializeField] private Transform _crushPosView;
    private bool _isLookingAtSort;
    private bool _isLookingAtCrush;

    protected override IEnumerator Sequence() {
        // Enter, wait for the lights to be pressed
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        // Go to the machine and press start
        yield return new WaitUntil(() => FlowerSortManager.Instance.GameStarted);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        _isLookingAtSort = false;
        yield return new WaitUntil(() => _isLookingAtSort);
        Debug.Log("Looking at sort");
        _clickAreaLeft.CanInteract = false;
        _clickAreaRight.CanInteract = false;
        // todo Sorting flowers maybe add dialogue here? 
        yield return new WaitUntil(() => FlowerSortManager.Instance.AmountSorted > 2);
        _clickAreaLeft.CanInteract = true;
        _clickAreaRight.CanInteract = true;
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[2]);
        // wait before we say anything

        // BANG, stop supply
        _conveyorSort.FlowerSupply = false;

        // Start next task
        _conveyorThree.ButtonClicked();
        // Wait for x amount of flowers to be crushed
        yield return new WaitUntil(() => _isLookingAtCrush);
        _clickAreaLeft.CanInteract = false;
        _clickAreaRight.CanInteract = false;
        yield return new WaitUntil(() => FlowerSortManager.Instance.AmountCrushed > 2);
        _clickAreaLeft.CanInteract = true;
        _clickAreaRight.CanInteract = true;
        _conveyorThree.FlowerSupply = false;
        // todo Probably wait a few seconds here

        // Huh, it stoped. Checkup on boss
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[3]);

        // DONE
        _elevator.AllowButtonClick(true);
        yield return new WaitUntil(() => _elevator.ElevatorMoving);
        yield return new WaitForSeconds(10);
        SceneHandler.Instance.LoadOverGround();
    }


    private void Awake() => CameraMovement.CurrentCameraView += CameraViewChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;
    private void CameraViewChanged(Transform t) {
        if (t == _sortPosView) {
            Debug.Log("LOOKING AT SORT");
            _isLookingAtSort = true;
        }
        if(t == _crushPosView) {
            _isLookingAtCrush = true;
        }
    }

}
