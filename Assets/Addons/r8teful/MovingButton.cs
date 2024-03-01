using System.Collections;
using UnityEngine;

public class MovingButton : MonoBehaviour {
    private float moveDistance = 0.16f; // Distance to move along the z-axis
    private float moveSpeed = 0.6f; // Speed of movement
    private bool isMoving = false;
    private Vector3 originalPosition;

    private void Start() {
        originalPosition = transform.position;
    }

    public void ActivateButton() {
        if (!isMoving) {
            isMoving = true;
            originalPosition = transform.position;
            StartCoroutine(MoveButton());
        }
    }

    private IEnumerator MoveButton() {
        Vector3 targetPosition = originalPosition + Vector3.forward * moveDistance;

        while (Vector3.Distance(transform.position, targetPosition) > 0.0001f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, originalPosition) > 0.0001f) {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}