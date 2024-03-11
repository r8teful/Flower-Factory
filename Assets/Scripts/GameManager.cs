using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager> {
    // Handles flow of the game, considers different scene loads, sets objects states accordingly

#if UNITY_EDITOR
    [SerializeField] private bool debugStartFlowerSeq;
    [SerializeField] private GameObject _flowerSortSequencer;
    //private GameObject _officeSequence;
#endif
    [SerializeField] private int[] _code;
    public int[] Code { get { return _code; } private set { _code = value; } }

    public int PlayerProgression { get => _playerProgression; set => _playerProgression = value; }
    public AudioSource ElevatorSoundSource { get; set; }
    [SerializeField] private int _playerProgression;
    private Light[] _lamps;
    private readonly int[] _cameraPos = { 0, 0, 1, 0};

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
        if (SceneHandler.Instance.IsOverWorld()) {
            GameObject.Find("LobbySequence").GetComponent<LobbySequence>().enabled = true;
            AudioController.Instance.SetLoopAndPlay("ambientOverworldHappy");
            AudioController.Instance.SetLoopVolumeImmediate(0.5f);
        }

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        _playerProgression++;
        Debug.Log(_playerProgression);
#if UNITY_EDITOR
        AudioController.Instance.StopAllLoops();
#endif
        if (ElevatorSoundSource != null) {
            // Elevator move sound playing, stop it
            ElevatorSoundSource.Stop();
            Destroy(ElevatorSoundSource.gameObject);
        }

        if (_playerProgression == 1) {
            // Underground first time
            GameObject.Find("UndergroundIntroSequence").GetComponent<UndergroundIntroSequence>().enabled = true;

            AudioController.Instance.SetLoopAndPlay("ambientUnderground"); 
            AudioController.Instance.SetLoopVolumeImmediate(0);
            AudioController.Instance.FadeInLoop(5,0.5f,0);
        }
        if (_playerProgression == 2) {
            // Overground second time bug prone but I dont care
            GameObject.Find("OfficeSequence").GetComponent<OfficeSequence>().enabled = true;
            GameObject.Find("MiddleRoom").transform.GetChild(0).GetComponent<LookView>().ConditionalMove = false;
            GameObject.Find("Hallway1").transform.GetChild(3).GetComponent<LookView>().ConditionalMove = false;
            AdjustLamps();
            OpenOffice();
            AudioController.Instance.SetLoopAndPlay("ambientOverground"); // Bit more creepy now!
            AudioController.Instance.SetLoopVolumeImmediate(0.5f);
        }
        if (_playerProgression == 3) {
            // Underground second time
            // todo actiavte other sequence, etc etc.

            GameObject.Find("UndergroundIntroSequence").GetComponent<UndergroundIntroSequence>().enabled = false;
            GameObject.Find("ControlRoomSequence").GetComponent<ControlRoomSequence>().enabled = true;

            AudioController.Instance.SetLoopAndPlay("ambientUnderground"); 
            AudioController.Instance.SetLoopVolumeImmediate(0);
            AudioController.Instance.FadeInLoop(5, 0.5f, 0);
        }

            // Always overworld specific things 
        if (SceneHandler.Instance.IsOverWorld()) {
        }
    }

    private void OpenOffice() {
        GameObject.Find("OfficePivot").transform.localRotation = Quaternion.Euler(0,-70,0);
    }

    private void AdjustLamps() {
        RenderSettings.fogDensity = 0.05f;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fog = true;

        _lamps = FindObjectsOfType<Light>();
        foreach (Light light in _lamps) {
            light.intensity = 0.03f;
            var p = light.transform.parent;
            if (p != null) {
                var r = p.GetComponent<Renderer>();
                if (r != null && r.material != null) {
                    light.transform.parent.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.55f, 0.5f, 0.43f)); // FFEAC9 is default light, this is darker
                    // I hate color, all I want is 8C806E 
                }
            }    
        }
    }

    public int GetCameraStartPosIndex() {
        return _cameraPos[_playerProgression];
    }
}
