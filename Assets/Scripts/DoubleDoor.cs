using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : MonoBehaviour {
    private bool _moving;
    [SerializeField] private Transform _door1OpenPos;
    [SerializeField] private Transform _door2OpenPos;
    [SerializeField] private Transform _toOpenDoor;

    private Vector3 _door1ClosedPos;
    private Vector3 _door2ClosedPos;
    private float _timeMoved;
    private void Start() {
        OpenDoors();
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
            transform.GetChild(0).localPosition = Vector3.MoveTowards(transform.GetChild(0).localPosition, d1, 1.5f * Time.deltaTime);
            transform.GetChild(1).localPosition = Vector3.MoveTowards(transform.GetChild(1).localPosition, d2, 2 * Time.deltaTime);
            yield return null;
        }
    }

}
