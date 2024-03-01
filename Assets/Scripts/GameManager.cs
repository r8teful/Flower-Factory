using Palmmedia.ReportGenerator.Core;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

#if UNITY_EDITOR
    [SerializeField] private bool debugStartFlowerSeq;
    [SerializeField] private GameObject _flowerSortSequencer;
#endif

    private void Start() {
#if UNITY_EDITOR

        if (debugStartFlowerSeq) {
            _flowerSortSequencer.SetActive(true);
        }

#endif
        // TODO add startup calls here
    }

}
