using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// It's 23PM and I just want this to work 
public class MovingButtonLocal : MonoBehaviour {
    private float moveDistance = 0.3f; // Distance to move along the z-axis
    private float moveSpeed = 2f; // Speed of movement
    private bool isMoving = false;
    private Vector3 originalPosition;
    [SerializeField] private bool _isRight;
    private void Start() {
        originalPosition = transform.localPosition;
    }

    public void ActivateButton() {
        if (!isMoving) {
            isMoving = true;
            originalPosition = transform.localPosition;
            StartCoroutine(MoveButton());
        }
    }

    private IEnumerator MoveButton() {
        Vector3 targetPosition = originalPosition + transform.localRotation * Vector3.down * moveDistance;
     

        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.0001f) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.0001f) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}