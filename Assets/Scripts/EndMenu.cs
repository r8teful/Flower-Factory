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
        Application.OpenURL("https://forms.gle/uv7yddLeVHcU8BLM7"); // todo
    }
    public void onRateClicked() {
        Application.OpenURL("https://forms.gle/uv7yddLeVHcU8BLM7"); // todo
    }

}
