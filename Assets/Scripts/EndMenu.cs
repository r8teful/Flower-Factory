using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour {

    protected Button _playButton;
    private void Start() {
        _playButton = GameObject.FindWithTag("ButtonPlay").GetComponent<Button>();
        if (_playButton != null) _playButton.onClick.AddListener(OnButtonMenuClick);
        SceneHandler.Instance.PlayMainMenuLoop();
    }

    private void OnButtonMenuClick() {
        SceneHandler.Instance.LoadMainMenu();
    }

    public void onOneSocketClicked() {
        Application.OpenURL("https://r8teful.itch.io/onesocket"); 
    }
    public void onRateClicked() {
        Application.OpenURL("https://r8teful.itch.io/flower-factory/rate"); 
    }
}
