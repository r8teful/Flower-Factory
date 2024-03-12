using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : PersistentSingleton<SceneHandler> {

    public void LoadOverGroundAsync() {
        StartCoroutine(LoadYourAsyncScene(1));
    }
    public void LoadOverGround() {
        SceneManager.LoadScene(1);
    }

    public void LoadUnderGround() {
        SceneManager.LoadScene(2);
    }

    public void StartGame() {
        var o = GameObject.Find("LOADING");
        if (o != null) {
            var r = o.GetComponent<RawImage>();
            if (r !=null) r.enabled = true;
            var c = r.transform.GetChild(0);
            if (c!=null) c.gameObject.SetActive(true);
        }
        StartCoroutine(LoadYourAsyncScene(1));
    }

    public bool IsOverWorld() {
        return SceneManager.GetActiveScene().buildIndex == 1;
    }
    public bool IsUnderGround() {
        return SceneManager.GetActiveScene().buildIndex == 2;
    }

    IEnumerator LoadYourAsyncScene(int buildIndex) {
       
        var asyncLoad = SceneManager.LoadSceneAsync(buildIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
        // Use  asyncLoad.allowSceneActivation = false; to handle when you change the scene 
    }
    private void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            LoadUnderGround();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            LoadOverGroundAsync();
        }
# endif
    }

    internal void LoadEndMenu() {
        StartCoroutine(LoadYourAsyncScene(3));
    }
    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
    public void ApplicationQuit() {
        Application.Quit();
    }
    public void onOneSocketClicked() {
        Application.OpenURL("https://forms.gle/uv7yddLeVHcU8BLM7"); // todo
    }
    public void onRateClicked() {
        Application.OpenURL("https://forms.gle/uv7yddLeVHcU8BLM7"); // todo
    }
}