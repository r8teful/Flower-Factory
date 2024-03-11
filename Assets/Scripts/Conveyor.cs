using Pixelplacement.TweenSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : ButtonDevice {

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _spawnPointHand;
    [SerializeField] private Transform _parentMovement;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private List<GameObject> _flowerPrefabs;
    [SerializeField] private GameObject _handPrefab;
    private AudioSource _loopAudio;
    public bool FlowerSupply { get; set; }

    private void Update() {
        _parentMovement.position += Vector3.right * Time.deltaTime * _movementSpeed;
    }


    private IEnumerator SpawnFlowers() {
        while (FlowerSupply) {
            int r = Random.Range(0, 3);
            var rotation = Quaternion.Euler(90, -180, Random.Range(-20, 20));
            var i = Instantiate(_flowerPrefabs[r], _spawnPoint.position, rotation, _parentMovement);
            Destroy(i, 20f);
            yield return new WaitForSeconds(_spawnDelay+ Random.value);
        }
    }

    public void SpawnHand() {
        var i = Instantiate(_handPrefab, _spawnPointHand.position, _handPrefab.transform.rotation, _parentMovement);
        Destroy(i, 20f);
    }

    public override void ButtonClicked() {
        var a = AudioController.Instance.PlaySound3D("ConveyorStart", transform.position, 0.1f, distortion: new AudioParams.Distortion(false, true));
        a.dopplerLevel = 0.5f;
        a.minDistance = 15;
        StartCoroutine(WaitForConveyorStartup(a));
    }

    private IEnumerator WaitForConveyorStartup(AudioSource a) {
        FlowerSortManager.Instance.GameStarted = true;
        yield return new WaitForSeconds(3);
        FlowerSupply = true;
        StartCoroutine(SpawnFlowers());
        yield return new WaitUntil(() => !a.isPlaying);

        _loopAudio = AudioController.Instance.PlaySound3D("ConveyorLoop", transform.position, 0.1f, distortion: new AudioParams.Distortion(false, true),looping:true);
        _loopAudio.dopplerLevel = 0.5f;
        _loopAudio.minDistance = 15;
        // while (FlowerSupply) {
        //     yield return new WaitUntil(() => _loopAudio.isPlaying);
        //     _loopAudio.Play();
        // }
    }
}
