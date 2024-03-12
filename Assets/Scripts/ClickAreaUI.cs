using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAreaUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler,IPointerMoveHandler {

    [SerializeField] MovementDirection movementDirection;
    private Image _image;
    private bool _canInteract = true;

    // Todo we have to set interactions true or false depening on the state we are on CameraMovement
    // For example, if we are peeking we only show back, only show forward if we can go forward etc etc.
    public bool CanInteract {
        get { return _canInteract; }
        set { _canInteract = value;
        if(_image == null) _image = GetComponent<Image>();
            _image.enabled = value;
        }
    }

    private void Awake() {
        _image = GetComponent<Image>();
        CameraMovement.CurrentCameraPeek += OnCameraPeekChanged;
    }
    private void OnDestroy() {
        CameraMovement.CurrentCameraPeek -= OnCameraPeekChanged;
    }

    private void OnCameraPeekChanged(bool b) {
        // We are on a peekpos, and we should enable down image
        if (movementDirection.Equals(MovementDirection.Down)) {
            CanInteract = b;
        } else {
            CanInteract = !b;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
       // if (!_canInteract) return;
        switch (movementDirection) {
            case MovementDirection.Left:
                CameraMovement.Instance.MoveCameraLeft();
                break;
            case MovementDirection.Right:
                CameraMovement.Instance.MoveCameraRight();
                break;
            case MovementDirection.Down:
                CameraMovement.Instance.CameraPeekBack();
                break;
            case MovementDirection.Forward:
                CameraMovement.Instance.MoveCameraForward();
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //if (!_canInteract) return; 
        UpdateCursor();
    }

    public void OnPointerExit(PointerEventData eventData) {
        switch (movementDirection) {
            case MovementDirection.Left:
                if (CursorManager.Instance.CurrentCursorType.Equals(CursorManager.CursorType.PointLeft))
                    CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
                break;
            case MovementDirection.Right:
                if (CursorManager.Instance.CurrentCursorType.Equals(CursorManager.CursorType.PointRight))
                    CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
                break;
            case MovementDirection.Down:
                if (CursorManager.Instance.CurrentCursorType.Equals(CursorManager.CursorType.PointDown))
                    CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
                break;
            case MovementDirection.Forward:
                if (CursorManager.Instance.CurrentCursorType.Equals(CursorManager.CursorType.PointUp))
                    CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
                break;
            default:
                // Todo add other?
                break;
        }
    }


public void OnPointerMove(PointerEventData eventData) {
        //if (!_canInteract) return;
        UpdateCursor();
    }

    // TODO Right now we just don't show the cursor update, but the collider itself is there, and we will still call the CameraMovement Methods BAD
    private void UpdateCursor() {
        switch (movementDirection) {
            case MovementDirection.Left:
                if (!CameraMovement.Instance.CanTurnLeftOrRight()) return;
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointLeft;
                break;
            case MovementDirection.Right:
                if (!CameraMovement.Instance.CanTurnLeftOrRight()) return;
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointRight;
                break;
            case MovementDirection.Down:
                if (!CameraMovement.Instance.CanTurnBackward()) return;
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointDown;
                break;
            case MovementDirection.Forward:
                if (!CameraMovement.Instance.CanMoveFoward()) return;
                if (CameraMovement.Instance.LockMovement) return; // Todo should also disable clickarea
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointUp;
                break;
            default:
                break;
        }
    }

    public enum MovementDirection{
        Left,
        Right,
        Down,
        Forward
    }
}
