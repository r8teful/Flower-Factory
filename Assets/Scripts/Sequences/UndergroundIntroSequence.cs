using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundIntroSequence : Sequencer {

    [SerializeField] private Transform _enterFactory;
    [SerializeField] private GameObject _elevatorWall;
    private bool _hasEnteredFactory;
    protected override IEnumerator Sequence() {
        StartCoroutine(MoveWall());
        yield return new WaitForSeconds(10);
        // That elevator takes forever!
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => _hasEnteredFactory);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);


        // Done, activate sortSequence
    }
    private IEnumerator MoveWall() {
        float startTime = Time.time; 
        while (Time.time - startTime < 20f) {
            _elevatorWall.transform.position += Vector3.up * Time.deltaTime * 1.5f;
            yield return null;
        }
    }

    private void Awake() => CameraMovement.CurrentCameraPos += CameraPosChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;
    private void CameraPosChanged(Transform t) {
        if (t == _enterFactory) {
            _hasEnteredFactory = true;
        }
    }
}
