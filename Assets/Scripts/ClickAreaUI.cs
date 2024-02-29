using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAreaUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler,IPointerMoveHandler {

    [SerializeField] MovementDirection movementDirection;
    private Image _image;
    private bool _canInteract = true;

    public bool CanInteract {
        get { return _canInteract; }
        set { _canInteract = value;
        _image.enabled = value;
        }
    }

    private void Awake() {
        _image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!_canInteract) return;
        switch (movementDirection) {
            case MovementDirection.Left:
                CameraMovement.Instance.MoveCameraLeft();
                break;
            case MovementDirection.Right:
                CameraMovement.Instance.MoveCameraRight();
                break;
            case MovementDirection.Down:
                CanInteract = false;
                break;
            case MovementDirection.Forward:
                CameraMovement.Instance.MoveCameraForward();
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!_canInteract) return; 
        switch (movementDirection) {
            case MovementDirection.Left:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointLeft;
                break;
            case MovementDirection.Right:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointRight;
                break;
            case MovementDirection.Down:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointDown;
                break;
            case MovementDirection.Forward:
                if(!CameraMovement.Instance.CanMoveCameraFoward()) return;
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointUp;
                break;
            default:
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
    }

    public void OnPointerMove(PointerEventData eventData) {
        if (!_canInteract) return;
        switch (movementDirection) {
            case MovementDirection.Left:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointLeft;
                break;
            case MovementDirection.Right:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointRight;
                break;
            case MovementDirection.Down:
                CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.PointDown;
                break;
            case MovementDirection.Forward:
                if (!CameraMovement.Instance.CanMoveCameraFoward()) return;
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
