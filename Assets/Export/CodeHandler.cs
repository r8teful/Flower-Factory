using TMPro;
using UnityEngine;

public class CodeHandler : StaticInstance<CodeHandler> {
  
    [SerializeField] private TextMeshPro _codeText;
    [SerializeField] private DoubleDoor _doubleDoor;
    [SerializeField] private LookView _lookViewOnDoor;
    private string _currentCode = "";


    private void UpdateCodeDisplay() {
        _codeText.text = _currentCode;
       
    }

    public void OnNumberButtonClick(int number) {
        AudioController.Instance.PlaySound3D("click", transform.position, distortion: new AudioParams.Distortion(false, true));
        if(_currentCode.Length < 3) {
            _currentCode += number.ToString();
            UpdateCodeDisplay();

        }
    }

    public void OnBackButtonClick() {
        AudioController.Instance.PlaySound3D("click", transform.position, distortion: new AudioParams.Distortion(false, true));
        if (_currentCode.Length > 0) {
            _currentCode = _currentCode.Substring(0, _currentCode.Length - 1);
            UpdateCodeDisplay();
        }
    }

    public void OnEnterButtonClick() {
        var code = "";

        foreach (int num in GameManager.Instance.Code) {
            code += num.ToString();
        }
        if (_currentCode == code) {
            // You did it!
            //AudioController.Instance.PlaySound3D("Correct", transform.position, distortion: new AudioParams.Distortion(false, true));
            GameEnd();
        } else {
            // Error sound!
            // Reset the code input
            //AudioController.Instance.PlaySound3D("Incorrect", transform.position, distortion: new AudioParams.Distortion(false, true));
            _currentCode = "";
            UpdateCodeDisplay();
        }
    }

    private void GameEnd() {
        _doubleDoor.OpenDoors();
        _lookViewOnDoor.ConditionalMove = false;
    }

    // Index until 9 is the same as the number, 10, is back, 11 confirm
    internal void ButtonPressed(int v) {
        if (v >= 0 && v < 10) {
            OnNumberButtonClick(v);
        } else if(v == 10) {
            OnBackButtonClick();
        } else if(v == 11) {
            OnEnterButtonClick();
        }
    }
}