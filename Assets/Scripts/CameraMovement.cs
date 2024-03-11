using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public static event Action<Transform> CurrentCameraView;
    public Transform CurrentView {
        get { return _currentView; }
        set {
            _currentView = value;
            CurrentCameraView?.Invoke(value);
        }
    }

    public bool LockMovement { get; set; }

    private void OnDrawGizmos() {
        if (positionsInspector != null) {
            foreach (Transform child in positionsInspector) {
                Gizmos.color = Color.white;
                if(child.gameObject.activeInHierarchy)
                Gizmos.DrawCube(child.position, Vector3.one * 1f);
                foreach (Transform grandchild in child) {
                    Gizmos.color = Color.blue;
                    if(grandchild.GetComponent<LookView>().CanMoveHere != null) Gizmos.color = Color.green;
                    if (grandchild.GetComponent<LookView>().CanMoveHere != null ^ grandchild.GetComponent<LookView>().WillLookHere != null) Gizmos.color = Color.red;
                    if(grandchild.GetComponent<LookView>().CanMoveHere != null && grandchild.GetComponent<LookView>().WillLookHere != null) {
                        // Todo also red if two have the same transform
                        if (grandchild.GetComponent<LookView>().CanMoveHere != grandchild.GetComponent<LookView>().WillLookHere.transform.parent.gameObject) Gizmos.color = Color.red;
                    }
                    if (grandchild.GetComponent<LookView>().ConditionalMove) Gizmos.color = Color.magenta;
                    if (grandchild.gameObject.activeInHierarchy)
                    Gizmos.DrawCube(grandchild.position, Vector3.one * .5f);
                    foreach (Transform grandgrandchild in grandchild) {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(grandgrandchild.position, Vector3.one * .05f);
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
        //_currentPos = _positions[GameManager.Instance.GetCameraStartPosIndex()];
        _currentPos = _positions[_positions.Count-1];
        CurrentView = _currentPos.GetChild(_viewIndex);
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
        if (CurrentView != null) {
            var rotation = Quaternion.LookRotation(CurrentView.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,Time.deltaTime *_lookSpeed);
        }
    }

    public void MoveCameraRight() {
        if (_targetPos != null || _peeking) return;
        _viewIndex = (_viewIndex + 1) % _currentPos.childCount;
        CurrentView = _currentPos.GetChild(_viewIndex);
    }

    public void MoveCameraLeft() {
        if (_targetPos != null || _peeking) return;
        _viewIndex = (_viewIndex - 1 + _currentPos.childCount) % _currentPos.childCount;
        CurrentView = _currentPos.GetChild(_viewIndex);
    }

    public void MoveCameraForward() {
        if (_peeking) return;
        if (_targetPos != null) return; // Still moving
        if (LockMovement) return; // Used by elevator
        var lw = CurrentView.GetComponent<LookView>();
        if (CurrentView != null && lw.CanMoveHere != null && !lw.ConditionalMove) {

            _targetPos = lw.CanMoveHere.transform;
            CurrentView = lw.WillLookHere.transform;
            _viewIndex = CurrentView.GetSiblingIndex();
            PlayStepSound();
        }
    }

    private void PlayStepSound() {
        if (SceneHandler.Instance.IsOverWorld()) {
            var a = AudioController.Instance.PlaySound3D("wood", transform.position, 0.3f, randomization: new AudioParams.Randomization());
            a.AddComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.CarpetedHallway;
        }  else {
            AudioController.Instance.PlaySound3D("con", transform.position, 0.3f, distortion: new AudioParams.Distortion(false, true), randomization: new AudioParams.Randomization());
        }
      
    }

    public void CameraPeek() {
        if (CurrentView == null || _peeking) return;
        if (CurrentView.childCount <= 0) return;
        // Save current pos and view so we can go back to it later
        _prevPos = _currentPos;
        _prevView = CurrentView;
        _targetPos = CurrentView.GetChild(0); // View should always be on second child
        CurrentView = CurrentView.GetChild(1); // View should always be on second child
        // Set position and rotation the one of the peekView
        // Say that we are peeking, only way rotate, or move, is going back
        _peeking = true;
    }

    public void CameraPeekBack() {
        if (!_peeking) return;
        // Go back to prev view and pos
        _targetPos = _prevPos;
        CurrentView = _prevView;
        _prevPos = null;
        _prevView = null;
        _peeking = false;
    }

    public bool CanMoveFoward() {
        if (CurrentView != null && CurrentView.GetComponent<LookView>().CanMoveHere != null) {
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
        if (CurrentView == null || _peeking) return false;
        if (CurrentView.childCount <= 0) return false;
        return true;
    }
}