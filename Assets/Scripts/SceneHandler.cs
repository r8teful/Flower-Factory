using System.Collections;
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
    public void PlayMainMenuLoop() {
        if (!AudioController.Instance.IsLoopPlaying("mainMenu")) {
            Debug.Log("PlayingLoop");
            AudioController.Instance.SetLoopAndPlay("mainMenu", 0);
        } else {
            Debug.Log("Loop already playing!");
            // Assume we already started the loop, so just resume
            AudioController.Instance.FadeInLoop(4, 1);
        }
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
}