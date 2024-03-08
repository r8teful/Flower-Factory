using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : StaticInstance<CameraMovement> {
    private List<Transform> _positions = new(); 
    [SerializeField] private Transform positionsInspector;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private float _moveSpeed;
    // Pos is a node, view is an edge
    private Transform _currentPos; 
    private Transform _targetPos; 
    private Transform _currentView; 
    // These two used for peeking, ie zooming 
    private Transform _prevView; 
    private Transform _prevPos; 
    private int _viewIndex = 0; 
    private bool _peeking;

    public static event Action<Transform> CurrentCameraPos;

    private void OnDrawGizmos() {
        if (positionsInspector != null) {
            foreach (Transform child in positionsInspector) {
                Gizmos.color = Color.white;
                if(child.gameObject.activeInHierarchy)
                Gizmos.DrawCube(child.position, Vector3.one * 1f);
                foreach (Transform grandchild in child) {
                    Gizmos.color = Color.blue;
                    if(grandchild.GetComponent<LookView>().CanMoveHere != null) Gizmos.color = Color.green;
                    if(grandchild.GetComponent<LookView>().CanMoveHere != null ^ grandchild.GetComponent<LookView>().WillLookHere != null) Gizmos.color = Color.red;
                    if (grandchild.GetComponent<LookView>().ConditionalMove) Gizmos.color = Color.magenta;
                    if (grandchild.gameObject.activeInHierarchy)
                    Gizmos.DrawCube(grandchild.position, Vector3.one * .5f);
                    foreach (Transform grandgrandchild in grandchild) {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(grandgrandchild.position, Vector3.one * .5f);
                    }
                }
            }
        }
    }
    private void PopulatePlayerPositions() {
        for (int i = 0; i < positionsInspector.childCount; i++) {
            _positions.Add(positionsInspector.GetChild(i));
        }
    }

    private void Start() {
        PopulatePlayerPositions();
        //_currentPos = _positions[_positions.Count-1];
        _currentPos = _positions[0];
        _currentView = _currentPos.GetChild(_viewIndex);
        transform.position = _currentPos.position;
    }

    private void FixedUpdate() {
        if (_targetPos != null) {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos.position, _moveSpeed * Time.deltaTime);
            // Check if reached the target node
            if (transform.position == _targetPos.position) {
                _currentPos = _targetPos;
                CurrentCameraPos?.Invoke(_currentPos);
                _targetPos = null;
            }
        }
        if (_currentView != null) {
            var rotation = Quaternion.LookRotation(_currentView.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,Time.deltaTime *_lookSpeed); 
        }
    }

    public void MoveCameraRight() {
        if (_targetPos != null || _peeking) return;
        _viewIndex = (_viewIndex + 1) % _currentPos.childCount;
        _currentView = _currentPos.GetChild(_viewIndex);
    }

    public void MoveCameraLeft() {
        if (_targetPos != null || _peeking) return;
        _viewIndex = (_viewIndex - 1 + _currentPos.childCount) % _currentPos.childCount;
        _currentView = _currentPos.GetChild(_viewIndex);
    }

    public void MoveCameraForward() {
        if (_peeking) return;
        var lw = _currentView.GetComponent<LookView>();
        if (_currentView != null && lw.CanMoveHere != null && !lw.ConditionalMove) {

            _targetPos = lw.CanMoveHere.transform;
            _currentView = lw.WillLookHere.transform;
            _viewIndex = _currentView.GetSiblingIndex();
        }
    }

    public void CameraPeek() {
        if (_currentView == null || _peeking) return;
        if (_currentView.childCount <= 0) return;
        // Save current pos and view so we can go back to it later
        _prevPos = _currentPos;
        _prevView = _currentView;
        Debug.Log("PEEKING");
        _targetPos = _currentView.GetChild(0); // View should always be on second child
        _currentView = _currentView.GetChild(1); // View should always be on second child
        // Set position and rotation the one of the peekView
        // Say that we are peeking, only way rotate, or move, is going back
        _peeking = true;
    }

    public void CameraPeekBack() {
        if (!_peeking) return;
        Debug.Log("PeekBack");
        // Go back to prev view and pos
        _targetPos = _prevPos;
        _currentView = _prevView;
        _prevPos = null;
        _prevView = null;
        _peeking = false;
    }

    public bool CanMoveFoward() {
        if (_currentView != null && _currentView.GetComponent<LookView>().CanMoveHere != null) {
            return true;
        }
        return false;
    }

    public bool CanTurnLeftOrRight() {
        // Should also actually check if there are children but rn we can always turn
        if(_peeking) return false;
        return true; 
    }

    public bool CanTurnBackward() {
        // Can only do this when we are peeking
        return _peeking;
    }

    public bool CanPeek() {
        if (_currentView == null || _peeking) return false;
        if (_currentView.childCount <= 0) return false;
        return true;
    }
}