using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : ButtonDevice {
    private bool _doorsMoving;
    public bool ElevatorMoving { get; private set; }

    private FactoryButton _fb;
    private bool _actived;
    private float _timeMoved;
    [SerializeField] private Transform _door1OpenPos;
    [SerializeField] private Transform _door2OpenPos;
    private Vector3 _door1ClosedPos;
    private Vector3 _door2ClosedPos;
    private AudioSource _elevatorMovingStart;
    private void Start() {
        _door1ClosedPos = transform.GetChild(0).localPosition;
        _door2ClosedPos = transform.GetChild(1).localPosition;
        _fb = GetComponentInChildren<FactoryButton>();
        // Start open only first scene
        if (GameManager.Instance.PlayerProgression == 0) {
            transform.GetChild(0).localPosition = _door1OpenPos.localPosition;
            transform.GetChild(1).localPosition = _door2OpenPos.localPosition;
        } else if (GameManager.Instance.PlayerProgression == 2) {
            AllowButtonClick(false); // Don't allow to move up again before some other sequence script says we can
            AudioController.Instance.PlaySound3D("ElevatorMovingStop", transform.position, 0.5f);
            OpenDoors(1);
        } else {
            // We are in the elevator, and moving, play the sound

            AllowButtonClick(false); // Don't allow to move up again before some other sequence script says we can
            AudioController.Instance.PlaySound3D("ElevatorMovingStop", transform.position, 0.5f);
            OpenDoors(8);
        }
    }
    public void OpenDoors(int delay) {
        CameraMovement.Instance.LockMovement = true;
        _doorsMoving = true;
        StartCoroutine(MoveDoorsToPositions(_door1OpenPos.localPosition, _door2OpenPos.localPosition, false,delay));
    }

    public void CloseDoors() {
        CameraMovement.Instance.LockMovement = true;
        _doorsMoving = true;
        StartCoroutine(MoveDoorsToPositions(_door1ClosedPos, _door2ClosedPos,true));
    }

    private IEnumerator MoveDoorsToPositions(Vector3 d1, Vector3 d2, bool isSceneLoad = false, int delay = 2) {
        yield return new WaitForSeconds(delay);
        // Play door move sound
        AudioController.Instance.PlaySound3D("ElevatorDoorMove",transform.position,0.5f);
        while (_doorsMoving) {
            transform.GetChild(0).localPosition = Vector3.MoveTowards(transform.GetChild(0).localPosition, d1, 1.25f * Time.deltaTime);
            transform.GetChild(1).localPosition = Vector3.MoveTowards(transform.GetChild(1).localPosition, d2, 1f * Time.deltaTime);

            bool door1Reached = Vector3.Distance(transform.GetChild(0).localPosition, d1) < 0.01f;
            bool door2Reached = Vector3.Distance(transform.GetChild(1).localPosition, d2) < 0.01f;
            if (door1Reached && door2Reached)
                _doorsMoving = false;
            if (door1Reached && door2Reached)
                _doorsMoving = false;

            yield return null;
        }
        if(isSceneLoad) {
            yield return new WaitForSeconds(1.5f);
            // We have closed the doors, packed our bags, are we are ready to head off!
            ElevatorMoving = true;
            if(SceneHandler.Instance.IsUnderGround()) {
                FindObjectOfType<ElevatorMoveObject>().MoveElevatorUp();
            }
            _elevatorMovingStart = AudioController.Instance.PlaySound3D("ElevatorMovingStart", transform.position, 0.5f);
            AudioController.Instance.FadeOutLoop(4,0); // We are going away from whereever we are stop the sound!
            StartCoroutine(WaitForElevatorMove());
        }
        // You are free to leave and go about your day
        CameraMovement.Instance.LockMovement = false;
    }

    private IEnumerator WaitForElevatorMove() {
        yield return new WaitUntil(() => !_elevatorMovingStart.isPlaying);
        var v = AudioController.Instance.PlaySound3D("ElevatorMoving", transform.position, 0.5f, looping: true);
        DontDestroyOnLoad(v);
        GameManager.Instance.ElevatorSoundSource = v;
    }

    public override void ButtonClicked() {
        if (_actived) return;
        CloseDoors();
        _actived = true;
    }

    public void AllowButtonClick(bool b) {
        _fb.AllowPress = b;
    }
}
