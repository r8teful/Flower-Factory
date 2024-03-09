using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : MonoBehaviour {
    private bool _moving;
    [SerializeField] private Transform _door1OpenPos;
    [SerializeField] private Transform _door2OpenPos;
    [SerializeField] private Transform _toOpenDoor;

    [SerializeField] private bool _conditionalOpen;
    public bool ConditionalOpen {
        get { return _conditionalOpen; }
        set { _conditionalOpen = value;
        }
    }


    private readonly Vector3 _door1ClosedPos = Vector3.zero;
    private readonly Vector3 _door2ClosedPos = new Vector3(0, -0.15f, -1.5f);
    private float _timeMoved;
    private void Start() {
        CameraMovement.CurrentCameraPos += OnCameraMoved;
        transform.GetChild(0).localPosition = _door1ClosedPos;
        transform.GetChild(1).localPosition = _door2ClosedPos;

    }
    private void OnDestroy() {
        CameraMovement.CurrentCameraPos -= OnCameraMoved;
    }

    private void OnCameraMoved(Transform t) {
        if (_conditionalOpen) return;
        if(t==_toOpenDoor) {
            OpenDoors();
        }
    }

    public void OpenDoors() {
        _moving = true;
        StartCoroutine(MoveDoorsToPositions(_door1OpenPos.localPosition, _door2OpenPos.localPosition));
    }

    public void CloseDoors() {
        _moving = true;
        StartCoroutine(MoveDoorsToPositions(_door1ClosedPos, _door2ClosedPos));
    }

    private IEnumerator MoveDoorsToPositions(Vector3 d1, Vector3 d2) {
        var startTime = Time.time;
        while (_moving) {
            _timeMoved = Time.time - startTime;
            if (_timeMoved > 6) _moving = false;
            transform.GetChild(0).localPosition = Vector3.MoveTowards(transform.GetChild(0).localPosition, d1,4.5f * Time.deltaTime);
            transform.GetChild(1).localPosition = Vector3.MoveTowards(transform.GetChild(1).localPosition, d2, 4 * Time.deltaTime);
            yield return null;
        }
    }

}
