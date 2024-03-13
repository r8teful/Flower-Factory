using System.Collections;
using UnityEngine;

public class OfficeSequence : Sequencer {

    [SerializeField] private Transform _exitLook;
    [SerializeField] private Transform _elevatorLook;
    [SerializeField] private Transform _monitorScreenLook;
    [SerializeField] private Transform _seenBlood;
    [SerializeField] private GameObject _officeWallsAndFloors;
    [SerializeField] private Elevator _elevator;
    [SerializeField] private GameObject _blood;
    private bool _seenCode;
    private bool _dialoge1played;
    private bool _dialoge2played;
    private bool _dialoge3played;
    private bool _hasSeenBlood;

    protected override IEnumerator Sequence() {
        Debug.Log("Office Sequence start");
        Instantiate(_blood);
        AudioController.Instance.PlaySound2D("theOffice", 0.5f);
        AudioController.Instance.SetLoopAndPlay("ambientOverground"); // Bit more creepy now!
        AudioController.Instance.SetLoopVolumeImmediate(0.5f);
        _elevatorLook.gameObject.SetActive(false);

        yield return new WaitUntil(() => _hasSeenBlood);
        yield return new WaitForSeconds(2.35f);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);


        yield return new WaitUntil(() => _elevator.ElevatorMoving);

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

    private void Awake() {
        if (GameManager.Instance.PlayerProgression < 2) return;
        CameraMovement.CurrentCameraView += CameraViewChanged;
    
    }
    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;
    private void CameraViewChanged(Transform t) {
        // todo do the dialogue calls here
        if (t == _exitLook) {
            if (_seenCode && !_dialoge1played) {
                // "I should not exit. I really should try to fix this"
                _dialoge1played = true;
                DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);
            } else if (!_seenCode && !_dialoge2played) {
                _dialoge2played = true;
                DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[2]);
            }
        }
        if(t==_monitorScreenLook && !_dialoge3played) {
            _dialoge3played = true;
            DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[3]);
            _elevator.AllowButtonClick(true);
            _seenCode = true;
        }
        if (t == _seenBlood) {
            _hasSeenBlood = true;
        }
    }
}