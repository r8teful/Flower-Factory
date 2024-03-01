using System.Collections.Generic;
using UnityEngine;

public class CursorManager : StaticInstance<CursorManager>{
    public enum CursorType {
        Default,
        Interactable,
        Grab,
        Grabbing,
        PointLeft,
        PointRight,
        PointDown,
        PointUp,
        Look,
        FBlue,
        FRed,
        FYellow
    }

    [SerializeField] private List<Texture2D> cursorTextures;
    private CursorType _currentCursorType = CursorType.Default;


    public CursorType CurrentCursorType {
        get { return _currentCursorType; }
        set {
            if (_currentCursorType != value) {
                if (InventoryHandler.Instance.HoldingObject != null) return;
                _currentCursorType = value;
                UpdateCursor();
            }
        }
    }

    private void UpdateCursor() {
        int cursorIndex = (int)_currentCursorType;
        if (cursorIndex < cursorTextures.Count) {
            Cursor.SetCursor(cursorTextures[cursorIndex], Vector2.zero, CursorMode.ForceSoftware);
        } else {
            Debug.LogError("Cursor texture not available for type: " + _currentCursorType);
        }
    }

    // Need these for stupid UI, set in inspector
    public void SetPointLeftCursor() {
        CurrentCursorType = CursorType.PointLeft;
    }
    public void SetPointRightCursor() {
        CurrentCursorType = CursorType.PointRight;
    }
    public void SetDefaultCursor() {
        CurrentCursorType = CursorType.Default;
    }
}
