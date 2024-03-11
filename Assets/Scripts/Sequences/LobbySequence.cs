using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySequence : Sequencer {
    [SerializeField] private Elevator _elevator;
    [SerializeField] private Transform _doorLook;
    [SerializeField] private Transform _officeLook;
    [SerializeField] private Transform _elevatorLook;
    [SerializeField] private GameObject _officeWallsAndFloors;
    [SerializeField] private float _movementSpeed;
    private bool _lookedAtDoor;
    private bool _lookedAtOffice;
    private bool _lookedAtOfficeDialoguePlayed;
    private bool _doorUnlock;
    protected override IEnumerator Sequence() {
        _elevatorLook.gameObject.SetActive(false);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);
        yield return new WaitUntil(() => _lookedAtDoor);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);

        // Wait untill we unlocked the door
        yield return new WaitUntil(() => !_doorLook.GetComponent<LookView>().ConditionalMove);   
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[2]);


        while (!_elevator.ElevatorMoving) { 
            if(_lookedAtOffice && !_lookedAtOfficeDialoguePlayed) {
                DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[3]);
                _lookedAtOfficeDialoguePlayed = true;
            }
            yield return null;
        }
        // End of lobby, load underground
        _elevatorLook.gameObject.SetActive(true);
        //_elevatorLook.GetComponent<PeekCollider>().enabled = true;
        Debug.Log("fade out lop");
        AudioController.Instance.FadeOutLoop(3);
        StartCoroutine(MoveOverAndWalls());
        yield return new WaitForSeconds(10);
        //Spook stuff? Sounds, visual things to see that it is moving

        //6B7AA0 normal real time shadow light


        RenderSettings.fogDensity = 0.05f;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fog = true;

        SceneHandler.Instance.LoadUnderGround();
    }

    private IEnumerator MoveOverAndWalls() {
        GameObject.Find("Grass").SetActive(false);
        while (true) {
            _officeWallsAndFloors.transform.position += Vector3.up * Time.deltaTime * 1.5f ;
            yield return null;
        }
    }

    private void Awake() => CameraMovement.CurrentCameraView += CameraViewChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;
    private void CameraViewChanged(Transform t) {
        if(t == _doorLook) {
            _lookedAtDoor = true;
        } 
        if(t == _officeLook) {
            _lookedAtOffice = true;
        }
    }
}
