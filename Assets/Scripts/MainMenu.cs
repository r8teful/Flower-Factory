using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    private AlphaLerp _fadeOut;
    protected Button _playButton;
    private void Start() {
        _fadeOut = GameObject.FindWithTag("FadeOut").GetComponent<AlphaLerp>();
        _playButton = GameObject.FindWithTag("ButtonPlay").GetComponent<Button>();
        if (_playButton != null) _playButton.onClick.AddListener(OnButtonPlayClick);
        if (_fadeOut != null) _fadeOut.gameObject.SetActive(true);
        SceneHandler.Instance.PlayMainMenuLoop();
        StartCoroutine(_fadeOut.Fade(true));
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
        Application.OpenURL("https://r8teful.itch.io/onesocket"); 
    }
    public void onRateClicked() {
        Application.OpenURL("https://r8teful.itch.io/flower-factory/rate");
    }
    public IEnumerator FadeOut() {
        _fadeOut = GameObject.FindWithTag("FadeOut").GetComponent<AlphaLerp>();
        if (_fadeOut == null) yield break;
        _fadeOut.gameObject.SetActive(true);
        yield return StartCoroutine(_fadeOut.GetComponent<AlphaLerp>().Fade(false));
    }
}
