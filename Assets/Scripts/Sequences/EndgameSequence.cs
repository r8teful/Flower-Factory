using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameSequence : Sequencer {
    [SerializeField] private Transform _inElevator;
    protected override IEnumerator Sequence() {
        Debug.Log("Endgame Start");
        return base.Sequence();
    }
}