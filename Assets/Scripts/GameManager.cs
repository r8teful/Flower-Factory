using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager> {
    // Handles flow of the game, considers different scene loads, sets objects states accordingly

#if UNITY_EDITOR
    [SerializeField] private bool debugStartFlowerSeq;
    [SerializeField] private GameObject _flowerSortSequencer;
#endif
    [SerializeField] private int[] _code;
    public int[] Code { get { return _code; } private set { _code = value; } }

    public int PlayerProgression { get => _playerProgression; set => _playerProgression = value; }

    private int _playerProgression;
    private Light[] _lamps;
    private readonly int[] _cameraPos = { 0, 0, 11, 0};

    protected override void Awake() {
        base.Awake();
        if (_code == null || _code.Length != 3) {
            _code = new int[3];
            _code[0] = 0;
            _code[1] = 0;
            _code[2] = 0;
       //     _code[0] = Random.Range(0, 10);
       //     _code[1] = Random.Range(0, 10);
       //     _code[2] = Random.Range(0, 10);
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    private void Start() {
#if UNITY_EDITOR
        if (debugStartFlowerSeq) {
            _flowerSortSequencer.SetActive(true);
        }
#endif
        //OnSceneLoad(SceneManager.GetActiveScene(),LoadSceneMode.Single);
        

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        _playerProgression++;
        Debug.Log(_playerProgression);
        if (_playerProgression == 1) {
            // Underground first time
        }
        if (_playerProgression == 2) {
            // Overground second time
            AdjustLamps();
            OpenOffice();
        }
        if (_playerProgression == 3) {
            // Underground second time

            // Always overworld specific things 
            if (SceneHandler.Instance.IsOverWorld()) {
            }
        }
    }

    private void OpenOffice() {
        GameObject.Find("OfficePivot").transform.localRotation = Quaternion.Euler(0,-70,0);
    }

    private void AdjustLamps() {
        _lamps = FindObjectsOfType<Light>();
        foreach (Light light in _lamps) {
            light.intensity = 0.1f;
        }
    }

    public int GetCameraStartPosIndex() {
        return _cameraPos[_playerProgression];
    }
}
