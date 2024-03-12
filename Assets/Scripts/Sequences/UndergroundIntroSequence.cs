using System.Collections;
using UnityEngine;

public class UndergroundIntroSequence : Sequencer {

    [SerializeField] private Transform _enterFactory;
    [SerializeField] private FactoryButton _supplyButton;
    private bool _hasEnteredFactory;
    protected override IEnumerator Sequence() {
        if (GameManager.Instance.PlayerProgression > 2) yield break;
        yield return new WaitForSeconds(12);
        // That elevator takes forever!
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => _hasEnteredFactory);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        // Done, activate sortSequence
        _supplyButton.AllowPress = true;
    }


    private void Awake() => CameraMovement.CurrentCameraPos += CameraPosChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;
    private void CameraPosChanged(Transform t) {
        if (t == _enterFactory) {
            _hasEnteredFactory = true;
        }
    }
}
