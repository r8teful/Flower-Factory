using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : ButtonDevice {

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _parentMovement;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private List<GameObject> _flowerPrefabs;


    private void Update() {
        _parentMovement.position += Vector3.right * Time.deltaTime * _movementSpeed;
    }


    private IEnumerator SpawnFlowers() {
        while (true) {
            int r = Random.Range(0, 3);
            var rotation = Quaternion.Euler(90, -180, Random.Range(-20, 20));
            var i = Instantiate(_flowerPrefabs[r], _spawnPoint.position, rotation, _parentMovement);
            Destroy(i, 20f);
            yield return new WaitForSeconds(_spawnDelay+ Random.Range(0,2));
        }
    }

    public override void ButtonClicked() {
        StartCoroutine(SpawnFlowers());
        FlowerSortManager.Instance.GameStarted = true;
    }
}
