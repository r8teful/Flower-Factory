using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ControlPannel : MonoBehaviour {
    private Slider[] _sliders;
    [SerializeField] private Transform _sliderPuzzle;
    [SerializeField] private Transform _controlSliders;
    [SerializeField] private Transform _progressionRectanglesParent;
    [SerializeField] private Transform _progressionSequence0Parent;
    [SerializeField] private Transform _progressionSequence1Parent;
    [SerializeField] private Transform _progressionSequence2Parent;
    [SerializeField] private GameObject _ButtonPuzzle0;
    [SerializeField] private GameObject _ButtonPuzzle1;
    [SerializeField] private GameObject _ButtonPuzzle2;
    [SerializeField] private GameObject _emergencyObject;
    [SerializeField] private GameObject _emergencyGuide;
    [SerializeField] private GameObject _varificationCompleteText;
    [SerializeField] private GameObject _sedativeLevelParent;

    private Slider[] _puzzleSliders;
    private RawImage[] _sliderProgress;
    private RawImage[] _progressionRectangles;
    private RawImage[] _progressionSquare0;
    private RawImage[] _progressionSquare1;
    private RawImage[] _progressionSquare2;
    private MeshRenderer[] _buttonPuzzleMesh;

    private readonly int[] sequence0 = { 0, 2, 1 }; // Red, Blue, Yellow
    private readonly int[] sequence1 = { 0, 1, 2, 1, 2 };  
    private readonly int[] sequence2 = { 1, 1, 0, 2, 0, 1,0 , 0};

    private bool[] _stages;
    private int _currentStageIndex;
    private int _buttonPuzzleIndex;
    private int _buttonsClicked;
    private bool _buttonPuzzle;
    private bool _passed;
    public bool MinigameStarted { get; set; }
    public bool SequenceComplete { get; private set; }

    private void Awake() {
        // uggliest awake method in excistence
        _sliders = _controlSliders.GetComponentsInChildren<Slider>().OrderBy(t => t.transform.GetSiblingIndex()).ToArray();
        _puzzleSliders = _sliderPuzzle.GetComponentsInChildren<Slider>().OrderBy(t => t.transform.GetSiblingIndex()).ToArray();
        _progressionSquare0 = _progressionSequence0Parent.GetComponentsInChildren<RawImage>().OrderBy(t => t.transform.GetSiblingIndex()).ToArray();
        _progressionSquare1 = _progressionSequence1Parent.GetComponentsInChildren<RawImage>().OrderBy(t => t.transform.GetSiblingIndex()).ToArray();
        _progressionSquare2 = _progressionSequence2Parent.GetComponentsInChildren<RawImage>().OrderBy(t => t.transform.GetSiblingIndex()).ToArray();

        _sliderProgress = new RawImage[3];
        _sliderProgress[0] = _sliderPuzzle.transform.GetChild(3).GetChild(0).GetComponent<RawImage>();
        _sliderProgress[1] = _sliderPuzzle.transform.GetChild(4).GetChild(0).GetComponent<RawImage>();
        _sliderProgress[2] = _sliderPuzzle.transform.GetChild(5).GetChild(0).GetComponent<RawImage>();


        _progressionRectangles = new RawImage[5];
        for (int i = 0; i < 5; i++) {
            _progressionRectangles[i] = _progressionRectanglesParent.GetChild(i).GetChild(0).GetComponent<RawImage>();
        }

        _progressionSquare0 = new RawImage[3];
        for (int i = 0; i < 3; i++) {
            _progressionSquare0[i] = _progressionSequence0Parent.GetChild(i).GetChild(0).GetComponent<RawImage>();
        }

        _progressionSquare1 = new RawImage[5];
        for (int i = 0; i < 5; i++) {
            _progressionSquare1[i] = _progressionSequence1Parent.GetChild(i).GetChild(0).GetComponent<RawImage>();
        }

        _progressionSquare2 = new RawImage[8];
        for (int i = 0; i < 8; i++) {
            _progressionSquare2[i] = _progressionSequence2Parent.GetChild(i).GetChild(0).GetComponent<RawImage>();
        }

        _buttonPuzzleMesh = new MeshRenderer[3];
        _buttonPuzzleMesh[0] = _sliderPuzzle.transform.GetChild(6).GetComponent<MeshRenderer>();
        _buttonPuzzleMesh[1] = _sliderPuzzle.transform.GetChild(7).GetComponent<MeshRenderer>();
        _buttonPuzzleMesh[2] = _sliderPuzzle.transform.GetChild(8).GetComponent<MeshRenderer>();
        _stages = new bool[5];
    }
    private void FixedUpdate() {
        for (int i = 0; i < 3; i++) {
            if(Mathf.Abs(_sliders[i].value - _puzzleSliders[i].value) <= 0.1f) {
                _sliderProgress[i].enabled = false;
            } else {
                _sliderProgress[i].enabled = true;
            }
        }
    }

    public void StartPannel() {
        StartCoroutine(MiniGameSequence());
        var _loopAudio = AudioController.Instance.PlaySound3D("AmbienceControlroom", transform.position, 0.1f, distortion: new AudioParams.Distortion(false, true), looping: true);
        _loopAudio.dopplerLevel = 0f;
        _loopAudio.minDistance = 2;
        _loopAudio.maxDistance = 5;
        _loopAudio.spatialBlend = 1;

    }

    private void GenerateNewSliderPuzzle() {
        // Active slider Gameobject and create new values
        ResetSliderScreen();

        for (int i = 0; i < 3; i++) {
            _puzzleSliders[i].value = Random.value;
        }
        _buttonPuzzleIndex = Random.Range(0, 3);
        _buttonPuzzleMesh[_buttonPuzzleIndex].enabled = true;
    }
    private void ResetSliderScreen() {
        // remove previos puzzles
        //if(_sliderPuzzle.gameObject.activeSelf) 
        _buttonPuzzleMesh[_buttonPuzzleIndex].enabled = false;
    }

    private void GenerateNewButtonPuzzle() {
        throw new NotImplementedException();
    }

    internal void ButtonPressed(int buttonIndex) {
        if(_currentStageIndex < 2) {
            if(buttonIndex == _buttonPuzzleIndex) {
                // Check if all the sliders are good
                for (int i = 0; i < 3; i++) if (_sliderProgress[i].enabled) {
                    AudioController.Instance.PlaySound2D("bad", 0.5f);
                    return;
                } 
                // Next stage
                _stages[_currentStageIndex] = true;
                AudioController.Instance.PlaySound2D("good", 0.5f);
            } else {
                AudioController.Instance.PlaySound2D("bad", 0.5f);
            }
        }

        if (_currentStageIndex==2) {
            // Button puzzle stage 
            _buttonsClicked++;
            if (buttonIndex == sequence0[_buttonsClicked - 1]) {
                Debug.Log("pass");
                _progressionSquare0[_buttonsClicked - 1].enabled = false;
                _passed = true;
            } else {
                Debug.Log("failed");
                _passed = false;
                ResetProgress();
            }
            if((_buttonsClicked == _progressionSquare0.Length) && (_passed == true)) {
                // next stage
                _stages[_currentStageIndex] = true;
                _buttonsClicked = 0;

                AudioController.Instance.PlaySound2D("good", 0.5f);
            }
        }
        if(_currentStageIndex==3) {
            _buttonsClicked++;
            if (buttonIndex == sequence1[_buttonsClicked - 1]) {
                _progressionSquare1[_buttonsClicked - 1].enabled = false;
                _passed = true;
            } else {
                _passed = false;
                ResetProgress();
            }
            if ((_buttonsClicked == _progressionSquare1.Length) && (_passed == true)) {
                // next stage
                AudioController.Instance.PlaySound2D("good", 0.5f);
                _stages[_currentStageIndex] = true;
                _buttonsClicked = 0;
            }
        }
        if (_currentStageIndex == 4) {
            _buttonsClicked++;
            if (buttonIndex == sequence2[_buttonsClicked - 1]) {
                _progressionSquare2[_buttonsClicked - 1].enabled = false;
                _passed = true;
            } else {
                _passed = false;
                ResetProgress();
            }
            if ((_buttonsClicked == _progressionSquare2.Length) && (_passed == true)) {
                // next stage
                AudioController.Instance.PlaySound2D("good", 0.5f);
                _stages[_currentStageIndex] = true;
                _buttonsClicked = 0;
            }
        }
    }

    private void ResetProgress() {
        AudioController.Instance.PlaySound2D("bad", 0.5f);
        _buttonsClicked = 0;
        if (_currentStageIndex == 2) {
            for (int i = 0; i < _progressionSquare0.Length; i++) {
                _progressionSquare0[i].enabled = true;
            }
        }
        if (_currentStageIndex == 3) {
            for (int i = 0; i < _progressionSquare1.Length; i++) {
                _progressionSquare1[i].enabled = true;
            }
        }
        if (_currentStageIndex == 4) {
            for (int i = 0; i < _progressionSquare2.Length; i++) {
                _progressionSquare2[i].enabled = true;
            }
        }
    }

    private IEnumerator MiniGameSequence() {
        // Ensure everything is hidden
        _sedativeLevelParent.SetActive(false);
        _progressionRectanglesParent.gameObject.SetActive(false);
        _sliderPuzzle.gameObject.SetActive(false);
        // blinking lights etc etc
        while (!MinigameStarted) {
            _emergencyObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            // todo beeping here
            _emergencyObject.SetActive(true);
            var a = AudioController.Instance.PlaySound3D("warning", transform.position, 0.6f);
            a.dopplerLevel = 0;
            a.maxDistance = 5;
            a.spatialBlend = 1;
            yield return new WaitForSeconds(1f);
        }
        // Start login sequence 
        _emergencyObject.SetActive(true);
        yield return new WaitForSeconds(2);
        // Hide blinking lights ui
        _emergencyObject.SetActive(false);
        // Complete tasks!!
        AudioController.Instance.PlaySound2D("neutral", 0.5f);
        _emergencyGuide.SetActive(true);
        yield return new WaitForSeconds(3);
        _emergencyGuide.SetActive(false);
        

        // Start puzzles, show Other ui
        _progressionRectanglesParent.gameObject.SetActive(true);
        _sliderPuzzle.gameObject.SetActive(true);


        GenerateNewSliderPuzzle();
        yield return new WaitUntil(() => _stages[_currentStageIndex]);
        _progressionRectangles[_currentStageIndex].enabled = false;

        // Puzzle slider 1
        _currentStageIndex = 1;
        GenerateNewSliderPuzzle();
        yield return new WaitUntil(() => _stages[_currentStageIndex]);
        _progressionRectangles[_currentStageIndex].enabled = false;
        _sliderPuzzle.gameObject.SetActive(false);

        // Next, button puzzle 0 
        _currentStageIndex = 2;
        _ButtonPuzzle0.SetActive(true);
        yield return new WaitUntil(() => _stages[_currentStageIndex]);
        _progressionRectangles[_currentStageIndex].enabled = false;
        _ButtonPuzzle0.SetActive(false);
        
        // Button Puzzle 1
        _currentStageIndex = 3;
        _ButtonPuzzle1.SetActive(true);
        yield return new WaitUntil(() => _stages[_currentStageIndex]);
        _progressionRectangles[_currentStageIndex].enabled = false;
        _ButtonPuzzle1.SetActive(false);
        
        // Button Puzzle 2
        _currentStageIndex = 4;
        _ButtonPuzzle2.SetActive(true);
        yield return new WaitUntil(() => _stages[_currentStageIndex]);
        _progressionRectangles[_currentStageIndex].enabled = false;
        _ButtonPuzzle2.SetActive(false);

        Debug.Log("YOU DID IT!");
        _progressionRectanglesParent.gameObject.SetActive(false);
        _sliderPuzzle.gameObject.SetActive(false);

        // verification complete!

        AudioController.Instance.PlaySound2D("neutral", 0.5f);
        _varificationCompleteText.SetActive(true);
        yield return new WaitForSeconds(3);
        _varificationCompleteText.SetActive(false);

        // OMG ITS SO LOW WE ARE ALL GOING TO DIE
        _sedativeLevelParent.SetActive(true); 
        SequenceComplete = true;
        StartCoroutine(SedativeProgressDown());
        while (true) {
            _sedativeLevelParent.transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            _sedativeLevelParent.transform.GetChild(0).gameObject.SetActive(true);
            var a = AudioController.Instance.PlaySound3D("warning", transform.position, 0.6f);
            a.dopplerLevel = 0;
            a.maxDistance = 5;
            a.spatialBlend = 1;
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator SedativeProgressDown() {
        var slider = _sedativeLevelParent.GetComponentInChildren<Slider>();
      
        while (slider.value >= 0) {
            slider.value -= 0.0001f;
            yield return null;
        }
    }
}
