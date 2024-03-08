using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundIntroSequence : Sequencer {

    [SerializeField] private Transform _enterFactory;
    private bool _hasEnteredFactory;
    protected override IEnumerator Sequence() {

        yield return new WaitForSeconds(2);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => _hasEnteredFactory);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);


        // Done, activate sortSequence
    }


    private void Awake() => CameraMovement.CurrentCameraPos += CameraPosChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;
    private void CameraPosChanged(Transform t) {
        if (t == _enterFactory) {
            _hasEnteredFactory = true;
        }
    }
}
