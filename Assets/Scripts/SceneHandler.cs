using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : PersistentSingleton<SceneHandler> {

    public void LoadOverGround() {
        StartCoroutine(LoadYourAsyncScene(0));

    }

    public void LoadUnderGround() {
        SceneManager.LoadScene(1);
    }

    public bool IsOverWorld() {
        return SceneManager.GetActiveScene().buildIndex == 0;
    }

    IEnumerator LoadYourAsyncScene(int buildIndex) {
       
        var asyncLoad = SceneManager.LoadSceneAsync(buildIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
        // Use  asyncLoad.allowSceneActivation = false; to handle when you change the scene 
    }

}
