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
    [SerializeField] private int _playerProgression; // 0 is main menu, 1 is first scene, 2 is second, 3 is first 2nd time, 4 is 2nd 2nd time
    private Light[] _lamps;
    private readonly int[] _cameraPos = { 0, 0, 0, 1, 0};

    protected override void Awake() {
        base.Awake();
        if (_code == null || _code.Length != 3) {
            _code = new int[3];
            _code[0] = 6;
            _code[1] = 4;
            _code[2] = 9;
       //     _code[0] = Random.Range(0, 10);
       //     _code[1] = Random.Range(0, 10);
       //     _code[2] = Random.Range(0, 10);
        }
    }

    private void OnDestroy() {
        Debug.Log("OnDestroy");
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    private void Start() {

        SceneManager.sceneLoaded += OnSceneLoad;
        ChangeSceneLook(_playerProgression);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        var i = scene.buildIndex;
        if (i == 0) {
            // Main menu, reset player progression
            _playerProgression = 0;
        } 
        if (i == 1) {
            if(_playerProgression < 2) {
            // first time loading overworld
            _playerProgression = 1;
            } else {
                // second time
                _playerProgression = 3;
            }
        }
        if (i == 2) {
            if(_playerProgression < 2) {
                // first time loading underworld
                _playerProgression = 2;
            } else {
                _playerProgression = 4;
            }
        }
        if (i == 3) {
            // End menu, reset player progression
            _playerProgression = 0;

        }
        if (ElevatorSoundSource != null) {
            // Elevator move sound playing, stop it
            ElevatorSoundSource.Stop();
            Destroy(ElevatorSoundSource.gameObject);
        }
        Debug.Log(_playerProgression);
        ChangeSceneLook(_playerProgression);
    }

    private void ChangeSceneLook(int progress) {
        if (progress == 1) {
            GameObject.Find("LobbySequence").GetComponent<LobbySequence>().enabled = true;
            AudioController.Instance.SetLoopAndPlay("ambientOverworldHappy");
            AudioController.Instance.SetLoopVolumeImmediate(0.5f);
        }
        if (progress == 2) {
            // Underground first time
            GameObject.Find("UndergroundIntroSequence").GetComponent<UndergroundIntroSequence>().enabled = true;

            AudioController.Instance.SetLoopAndPlay("ambientUnderground");
            AudioController.Instance.SetLoopVolumeImmediate(0);
            AudioController.Instance.FadeInLoop(5, 0.5f, 0);
        }
        if (progress == 3) {
            // Overground second time bug prone but I dont care
            GameObject.Find("OfficeSequence").GetComponent<OfficeSequence>().enabled = true;
            GameObject.Find("MiddleRoom").transform.GetChild(0).GetComponent<LookView>().ConditionalMove = false;
            GameObject.Find("Hallway1").transform.GetChild(3).GetComponent<LookView>().ConditionalMove = false;
            AdjustLamps();
            OpenOffice();
            //AudioController.Instance.StopLoop(0);
        }
        if (progress == 4) {
            // Underground second time

            GameObject.Find("UndergroundIntroSequence").GetComponent<UndergroundIntroSequence>().enabled = false;
            GameObject.Find("ControlRoomSequence").GetComponent<ControlRoomSequence>().enabled = true;
            AdjustUndergroundAmbience();
            AudioController.Instance.SetLoopAndPlay("ambientUnderground");
            AudioController.Instance.SetLoopVolumeImmediate(0);
            AudioController.Instance.FadeInLoop(5, 0.5f, 0);
        }
    }

    private void AdjustUndergroundAmbience() {
        // Just add some fog, possibly also make it slighty darker
        RenderSettings.fogDensity = 0.025f;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fog = true;
        _lamps = FindObjectsOfType<Light>();
        foreach (Light light in _lamps) {
            if(!light.CompareTag("Findable"))
                light.intensity = 0.03f;
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
