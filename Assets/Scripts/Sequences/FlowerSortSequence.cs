using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSortSequence : Sequencer {
    [SerializeField] private ConveyorThree _conveyorThree;
    [SerializeField] private Conveyor _conveyorSort;
    [SerializeField] private Elevator _elevator;
    [SerializeField] private FactoryButton _supplyButton;
    protected override IEnumerator Sequence() {
        // Enter, wait for the lights to be pressed
        _supplyButton.AllowPress = true;
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        // Go to the machine and press start
        yield return new WaitUntil(() => FlowerSortManager.Instance.GameStarted);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);

        // Sorting flowers
        yield return new WaitUntil(() => FlowerSortManager.Instance.AmountSorted > 2);

        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[2]);
        // wait before we say anything

        // BANG, stop supply
        _conveyorSort.FlowerSupply = false;

        // Start next task
        _conveyorThree.ButtonClicked();
        // Wait for x amount of flowers to be crushed

        yield return new WaitUntil(() => FlowerSortManager.Instance.AmountCrushed > 2);
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
}
