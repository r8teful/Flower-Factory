using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour {
    private Light _targetLight;

    [SerializeField] private float _minIntensity = 1f;
    [SerializeField] private float _maxIntensity = 2f;
    [SerializeField] private float _flickerSpeed = 2f;

    private float baseIntensity;
    private float flickerTimer;

    void Start() {
        if (_targetLight == null) {
            _targetLight = GetComponent<Light>();
        }

        baseIntensity = _targetLight.intensity;
    }

    void Update() {
        flickerTimer -= Time.deltaTime;
        if (flickerTimer <= 0f) {
            float newIntensity = Random.Range(_minIntensity, _maxIntensity);
            _targetLight.intensity = newIntensity;
            flickerTimer = 1f / _flickerSpeed;
        }
    }

    //void OnDisable() {
    //    _targetLight.intensity = baseIntensity;
    //}
}
