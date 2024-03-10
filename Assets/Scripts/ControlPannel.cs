using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start() {
        StartCoroutine(MiniGameSequence());
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
                for (int i = 0; i < 3; i++) if (_sliderProgress[i].enabled) return;
                Debug.Log("Stage complete!");
                _stages[_currentStageIndex] = true;
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
            }
        }
        if(_currentStageIndex==3) {
            _buttonsClicked++;
            if (buttonIndex == sequence1[_buttonsClicked - 1]) {
                Debug.Log("pass");
                _progressionSquare1[_buttonsClicked - 1].enabled = false;
                _passed = true;
            } else {
                Debug.Log("failed");
                _passed = false;
                ResetProgress();
            }
            if ((_buttonsClicked == _progressionSquare1.Length) && (_passed == true)) {
                // next stage
                _stages[_currentStageIndex] = true;
                _buttonsClicked = 0;
            }
        }
        if (_currentStageIndex == 4) {
            _buttonsClicked++;
            if (buttonIndex == sequence2[_buttonsClicked - 1]) {
                Debug.Log("pass");
                _progressionSquare2[_buttonsClicked - 1].enabled = false;
                _passed = true;
            } else {
                Debug.Log("failed");
                _passed = false;
                ResetProgress();
            }
            if ((_buttonsClicked == _progressionSquare2.Length) && (_passed == true)) {
                // next stage
                _stages[_currentStageIndex] = true;
                _buttonsClicked = 0;
            }
        }
    }

    private void ResetProgress() {
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

        // Puzzle slider 0
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

        Debug.Log("YOU DID IT!");
    }

   
}
