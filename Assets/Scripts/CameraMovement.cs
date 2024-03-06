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
    private int _viewIndex = 0; 
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
        _currentPos = _positions[0];
        _currentView = _positions[0].GetChild(_viewIndex);
        transform.position = _currentPos.position;
    }

    private void FixedUpdate() {
        if (_targetPos != null) {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos.position, _moveSpeed * Time.deltaTime);
            // Check if reached the target node
            if (transform.position == _targetPos.position) {
                _currentPos = _targetPos;
                _targetPos = null;
            }
        }
        if (_currentView != null) {
            var rotation = Quaternion.LookRotation(_currentView.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,Time.deltaTime *_lookSpeed); 
        }
    }

    public void MoveCameraRight() {
        _viewIndex = (_viewIndex + 1) % _currentPos.childCount;
        _currentView = _currentPos.GetChild(_viewIndex);
        Debug.Log("view Index is: " + _viewIndex);
    }

    public void MoveCameraLeft() {
        _viewIndex = (_viewIndex - 1 + _currentPos.childCount) % _currentPos.childCount;
        _currentView = _currentPos.GetChild(_viewIndex);
        Debug.Log("view Index is: " + _viewIndex);
    }

    public void MoveCameraForward() {
        var lw = _currentView.GetComponent<LookView>();
        if (_currentView != null && lw.CanMoveHere != null && !lw.ConditionalMove) {

            _targetPos = lw.CanMoveHere.transform;
            _currentView = lw.WillLookHere.transform;
            _viewIndex = _currentView.GetSiblingIndex();
        Debug.Log("view Index is: " + _viewIndex);
        }
    }
    public bool CanMoveCameraFoward() {
        if (_currentView != null && _currentView.GetComponent<LookView>().CanMoveHere != null) {
            return true;
        }
        return false;
    }
}