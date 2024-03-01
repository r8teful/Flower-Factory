using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSortSequence : Sequencer {
    protected override IEnumerator Sequence() {
        // Enter, wait for the lights to be pressed
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        // Go to the machine and press start
        yield return new WaitUntil(() => FlowerSortManager.Instance.GameStarted);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);

        // Wait for x amount of flowers to be sorted correctly
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);

        // BANG

        // Wait for press button again

        // Go to next task

        // Wait for x amount of flowers to be crushed

        // Checkup on mates

        // DONE
    }
}
