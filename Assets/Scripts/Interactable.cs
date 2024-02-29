using System.Collections;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    protected virtual void OnMouseEnter() {
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Interactable;
    }

    protected virtual void OnMouseExit() {
        CursorManager.Instance.CurrentCursorType = CursorManager.CursorType.Default;
    }

    protected abstract void OnMouseDown();
}