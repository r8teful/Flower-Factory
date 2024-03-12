using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorThree : ButtonDevice {
    [SerializeField] private List<Transform> _spawnPoints = new();
    [SerializeField] private Transform _parentMovement;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private List<GameObject> _flowerPrefabs;
    public bool FlowerSupply { get; set; }
    private void Update() {
        _parentMovement.position += Vector3.back * Time.deltaTime * _movementSpeed;
    }

    private void Start() {

       // StartCoroutine(SpawnFlowers());
    }
    private IEnumerator SpawnFlowers() {
        while (FlowerSupply) {
            for (int i = 0; i < _flowerPrefabs.Count; i++) {        
                var rotation = Quaternion.Euler(150, -90, 180 +Random.Range(-20, 20));
                var p = Instantiate(_flowerPrefabs[i], _spawnPoints[i].position, rotation, _parentMovement);
                p.GetComponent<Flower>().IsCrushMode = true;
                Destroy(p,7f);
                yield return new WaitForSeconds(_spawnDelay + Random.value);
            }
        }
    }

    public override void ButtonClicked() {
        FlowerSupply = true;
        // Check first if we have done sorting first
        if (!FlowerSortManager.Instance.GameStarted) return;
        StartCoroutine(SpawnFlowers());
    }

}