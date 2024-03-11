using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMoveObject : MonoBehaviour {

    private Vector3 _startPos;
    private Vector3 _targetPos;

    private void Start() {
        _targetPos = transform.localPosition;
        _startPos = transform.localPosition -new Vector3(0,8,0);
        transform.localPosition = _startPos;
        // Move elevator down
        StartCoroutine(Anim(_startPos, _targetPos,4));
    }

    public void MoveElevatorUp() {
        // Gives the illusion we are going up

        StartCoroutine(Anim(_targetPos, _startPos, 4));
    }


    IEnumerator Anim(Vector3 startPosition, Vector3 targetPosition, float animationDuration, bool applyEasing = true, float delay = 0f) {
        // Wait for the delay
        yield return new WaitForSeconds(delay);

        float time = 0;
        while (time < animationDuration) {
            float t = time / animationDuration;
            if (applyEasing) {
                t = Mathf.SmoothStep(0f, 1f, t); // Apply easing if needed
            }
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
    }
}
