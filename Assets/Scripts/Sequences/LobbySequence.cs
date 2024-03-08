using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySequence : Sequencer {
    [SerializeField] private Elevator _elevator;
    protected override IEnumerator Sequence() {
        // Dialogue stuff


        // End of lobby, load underground
        yield return new WaitUntil(() => _elevator.ElevatorMoving);
        yield return new WaitForSeconds(10);
        //Spook stuff? Sounds, visual things to see that it is moving
        SceneHandler.Instance.LoadUnderGround();
    }
}
