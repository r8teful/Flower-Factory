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
    private Slider[] _puzzleSliders;
    private RawImage[] _sliderProgress;
    private MeshRenderer[] _buttonPuzzleMesh;
    private int _buttonPuzzleIndex;
    private void Awake() {
        // uggliest awake method in excistence
        _sliders = _controlSliders.GetComponentsInChildren<Slider>().OrderBy(t => t.transform.GetSiblingIndex()).ToArray();
        _puzzleSliders = _sliderPuzzle.GetComponentsInChildren<Slider>().OrderBy(t => t.transform.GetSiblingIndex()).ToArray();
        _sliderProgress = new RawImage[3];
        _sliderProgress[0] = _sliderPuzzle.transform.GetChild(3).GetChild(0).GetComponent<RawImage>();
        _sliderProgress[1] = _sliderPuzzle.transform.GetChild(4).GetChild(0).GetComponent<RawImage>();
        _sliderProgress[2] = _sliderPuzzle.transform.GetChild(5).GetChild(0).GetComponent<RawImage>();

        _buttonPuzzleMesh = new MeshRenderer[3];
        _buttonPuzzleMesh[0] = _sliderPuzzle.transform.GetChild(6).GetComponent<MeshRenderer>();
        _buttonPuzzleMesh[1] = _sliderPuzzle.transform.GetChild(7).GetComponent<MeshRenderer>();
        _buttonPuzzleMesh[2] = _sliderPuzzle.transform.GetChild(8).GetComponent<MeshRenderer>();
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
        GenerateNewSliderPuzzle();
    }

    private void GenerateNewSliderPuzzle() {
        // todo remove previos puzzle

        for (int i = 0; i < 3; i++) {
            _puzzleSliders[i].value = Random.value;
        }
        _buttonPuzzleIndex = Random.Range(0, 3);
        _buttonPuzzleMesh[_buttonPuzzleIndex].enabled = true;
    }

    internal void ButtonPressed(int buttonIndex) {
        if(buttonIndex == _buttonPuzzleIndex) {
            // Check if all the sliders are good
            for (int i = 0; i < 3; i++) if (_sliderProgress[i].enabled) return;
            Debug.Log("Stage complete!");
        }
    }
}
