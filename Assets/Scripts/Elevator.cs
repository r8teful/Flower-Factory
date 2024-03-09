using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : ButtonDevice {
    private bool _doorsMoving;
    public bool ElevatorMoving { get; set; }

    private FactoryButton _fb;
    private bool _actived;
    private float _timeMoved;
    [SerializeField] private bool _startOpen;
    [SerializeField] private Transform _door1OpenPos;
    [SerializeField] private Transform _door2OpenPos;
    private Vector3 _door1ClosedPos;
    private Vector3 _door2ClosedPos;

    private void Start() {
        _door1ClosedPos = transform.GetChild(0).localPosition;
        _door2ClosedPos = transform.GetChild(1).localPosition;
        _fb = GetComponentInChildren<FactoryButton>();
        if(_startOpen) {
            transform.GetChild(0).localPosition = _door1OpenPos.localPosition;
            transform.GetChild(1).localPosition = _door2OpenPos.localPosition;
        }
    }
    public void OpenDoors() {
        _doorsMoving = true;
        StartCoroutine(MoveDoorsToPositions(_door1OpenPos.localPosition, _door2OpenPos.localPosition));
    }

    public void CloseDoors() {
        CameraMovement.Instance.LockMovement = true;
        _doorsMoving = true;
        StartCoroutine(MoveDoorsToPositions(_door1ClosedPos, _door2ClosedPos));
    }

    private IEnumerator MoveDoorsToPositions(Vector3 d1, Vector3 d2) {
        yield return new WaitForSeconds(5);
        var startTime = Time.time;
        while (_doorsMoving) {
            _timeMoved = Time.time-startTime;
            if(_timeMoved>6) _doorsMoving = false;
            transform.GetChild(0).localPosition = Vector3.MoveTowards(transform.GetChild(0).localPosition, d1, 1.5f * Time.deltaTime);
            transform.GetChild(1).localPosition = Vector3.MoveTowards(transform.GetChild(1).localPosition, d2, 2 * Time.deltaTime);
            yield return null;
        }
        // We have closed the doors, packed our bags, are we are ready to head off!
        ElevatorMoving = true;
    }

    public override void ButtonClicked() {
        // TODO add functionality that prevents button presses
        if (_actived) return;

        if(_startOpen) {
            CloseDoors();
            _actived = true;
        } else {
            OpenDoors();
            _actived = true;
            // Load overground again
        }
    }

    public void AllowButtonClick(bool b) {
        _fb.AllowPress = b;
    }
}
