using UnityEngine;

public class MenuBackground : MonoBehaviour {
    [SerializeField] private GameObject[] _prefabs; 
    [SerializeField] private float _spawnInterval = 0.2f;
    [SerializeField] private float _fallSpeed = 2f;
    [SerializeField] BoxCollider _box;

    private void Start() {
        SpawnObjects();
    }

    void SpawnObjects() {
        Vector2 colliderSize = _box.size;
        Vector2 startPoint = (Vector2)_box.transform.position - (colliderSize / 2);
        var distanceBetweenObjects =1.2f;
        int objectsPerRow = Mathf.FloorToInt(colliderSize.x / distanceBetweenObjects);
        Vector2 spawnPoint = startPoint;

        for (int i = 0; i < 100; i++) {
            Instantiate(_prefabs[Random.Range(0, 3)], spawnPoint, Quaternion.Euler(0f, Random.Range(-60, -80), Random.Range(70, 110))).AddComponent<RotateAlways>().RotationSpeed = 0.2f;

            if ((i + 1) % objectsPerRow == 0) {
                spawnPoint.x = startPoint.x;
                spawnPoint.y += distanceBetweenObjects;
            } else {
                spawnPoint.x += distanceBetweenObjects;
            }
        }
    }
}