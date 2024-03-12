using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    protected Button _playButton;
    private void Start() {
        _playButton = GameObject.FindWithTag("ButtonPlay").GetComponent<Button>();
        if (_playButton != null) _playButton.onClick.AddListener(OnButtonPlayClick);
        SceneHandler.Instance.PlayMainMenuLoop();
    }

    private void OnButtonPlayClick() {
        var o = GameObject.Find("LOADING");
        if (o != null) {
            var r = o.GetComponent<RawImage>();
            if (r != null) r.enabled = true;
            var c = r.transform.GetChild(0);
            if (c != null) c.gameObject.SetActive(true);
        }
        SceneHandler.Instance.StartGame();
    }
    public void onOneSocketClicked() {
        Application.OpenURL("https://r8teful.itch.io/onesocket"); // todo
    }
    public void onRateClicked() {
        Application.OpenURL(""); // todo
    }
}
