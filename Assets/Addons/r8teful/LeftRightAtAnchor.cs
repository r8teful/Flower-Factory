using UnityEngine;

public class LeftRightAtAnchor : MonoBehaviour {
    [SerializeField] private float _rockingSpeed;
    [SerializeField] private float _angle;
    [SerializeField] private GameObject _anchorPoint;
    private float _timeCounter;

    private Quaternion initialRotation;


    private void Start() {
        initialRotation = transform.rotation;
    }

    private void Update() {
        _timeCounter += Time.deltaTime * _rockingSpeed;
        float currentAngle =  Mathf.Sin(_timeCounter) * _angle;
        transform.RotateAround(_anchorPoint.transform.position,Vector3.forward,currentAngle);
        //Quaternion currentRotation = Quaternion.Euler(0, 0, currentAngle) * initialRotation;
        //transform.rotation = currentRotation;
    }
}
