using System.Collections;
using UnityEngine;

public class OfficeSequence : Sequencer {

    [SerializeField] private Transform _exitLook;
    [SerializeField] private Transform _elevatorLook;
    [SerializeField] private Transform _monitorScreenLook;
    [SerializeField] private GameObject _officeWallsAndFloors;
    [SerializeField] private Elevator _elevator;
    private bool _lookedAtExit;
    private bool _seenCode;
    private bool _exitDialoguePlayed;
    private bool _codeDialoguePlayed;

    protected override IEnumerator Sequence() {
        Debug.Log("Office Sequence start");
        AudioController.Instance.PlaySound2D("theOffice", 0.5f);
        AudioController.Instance.SetLoopAndPlay("ambientOverground"); // Bit more creepy now!
        AudioController.Instance.SetLoopVolumeImmediate(0.5f);
        _elevatorLook.gameObject.SetActive(false);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);


        while (!_elevator.ElevatorMoving) {
            if (_lookedAtExit && !_exitDialoguePlayed) {
                if (_seenCode) {
                    // "I should not exit. I really should try to fix this"
                    _exitDialoguePlayed = true;
                    DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);
                } else if(!_codeDialoguePlayed) {
                    // "I should look for the code" dialogue;
                    _codeDialoguePlayed = true;
                    _lookedAtExit = false;
                    DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[2]);
                }
            }
            yield return null;
        }

        StartCoroutine(MoveOverAndWalls());
        // Going back down, quicker elevator now
        yield return new WaitForSeconds(3);
        //Spook stuff? Sounds, visual things to see that it is moving
        SceneHandler.Instance.LoadUnderGround();

    }
    private IEnumerator MoveOverAndWalls() {
        GameObject.Find("Grass").SetActive(false);
        while (true) {
            _officeWallsAndFloors.transform.position += Vector3.up * Time.deltaTime * 1.5f;
            yield return null;
        }
    }

    private void Awake() => CameraMovement.CurrentCameraView += CameraViewChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;
    private void CameraViewChanged(Transform t) {
        if (t == _exitLook) {
            _lookedAtExit = true;
        }
        if(t==_monitorScreenLook) {
            _seenCode = true;
            _elevator.AllowButtonClick(true);
        }
    }
}