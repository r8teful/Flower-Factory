using UnityEngine;

public class RotateAlways : MonoBehaviour{
    public float RotationSpeed;
    private int direction;
    private void Start() {
        direction = (Random.Range(0, 2) * 2 - 1); // Either -1 or 1
    }
    void FixedUpdate() {
        transform.localRotation *= Quaternion.Euler(0, RotationSpeed * direction, 0);
    }
}
