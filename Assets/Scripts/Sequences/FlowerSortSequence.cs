using System.Collections;
using UnityEngine;

public class FlowerSortSequence : Sequencer {
    [SerializeField] private ConveyorThree _conveyorThree;
    [SerializeField] private Conveyor _conveyorSort;
    [SerializeField] private Elevator _elevator;
    [SerializeField] private ClickAreaUI _clickAreaLeft;
    [SerializeField] private ClickAreaUI _clickAreaRight;
    [SerializeField] private Transform _sortPosView;
    [SerializeField] private Transform _crushPosView;
    private bool _isLookingAtSort;
    private bool _isLookingAtCrush;

    protected override IEnumerator Sequence() {
        // Enter, wait for the lights to be pressed
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[0]);
        // Go to the machine and press start
        yield return new WaitUntil(() => FlowerSortManager.Instance.GameStarted);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[1]);
        _isLookingAtSort = false;
        yield return new WaitUntil(() => _isLookingAtSort);
        _clickAreaLeft.CanInteract = false;
        _clickAreaRight.CanInteract = false;
        yield return new WaitForEndOfFrame(); // im not having anyone getting softlocked 
        CameraMovement.Instance.CurrentView = _sortPosView;


        // START SORTING FLOWERS -------------
        yield return new WaitForSeconds(15f);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[2]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(5f);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[8]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);

        yield return new WaitForSeconds(4f);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[9]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);

        yield return new WaitForSeconds(10f);
        _conveyorSort.SpawnHand();
        yield return new WaitForSeconds(1.5f);
        AudioController.Instance.PlaySound2D("stingerDark", 0.4f);
        yield return new WaitForSeconds(1.4f);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[3]);
        yield return new WaitForSeconds(9);
        //yield return new WaitUntil(() => FlowerSortManager.Instance.AmountSorted > 2);
        // BANG, stop supply

        AudioController.Instance.PlaySound3D("explosionDistantBetter",new Vector3(
            _conveyorSort.transform.position.x, _conveyorSort.transform.position.y + 2, _conveyorSort.transform.position.z), 1f);

        var s = FindObjectOfType<CameraShake>();
        StartCoroutine(s.Shake(1.4f, 0.2f));
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(s.Shake(1f, 0.1f));
        yield return new WaitForSeconds(1f);
        StartCoroutine(s.Shake(0.5f, 0.04f));
        // wait before we say anything
        yield return new WaitForSeconds(2.42f);
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[4]); // That did not sound good
        yield return new WaitForSeconds(10f); // Play for another little bit
        _conveyorSort.FlowerSupply = false;


        yield return new WaitForSeconds(2f); 
        _clickAreaLeft.CanInteract = true;
        _clickAreaRight.CanInteract = true;
        yield return new WaitForSeconds(3f); // Wait untill we notice

        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[5]); // Huh, look like the supply stopped, lets start with next

        // Start next task
        _conveyorThree.ButtonClicked();
        // Wait for x amount of flowers to be crushed
        _isLookingAtCrush = false;
        yield return new WaitUntil(() => _isLookingAtCrush);
        _clickAreaLeft.CanInteract = false;
        _clickAreaRight.CanInteract = false;
        yield return new WaitForEndOfFrame();
        CameraMovement.Instance.CurrentView = _crushPosView;

        // CRUSHING FLOWERS ------------------

        yield return new WaitForSeconds(6f); // crush a lil bit and then start rambling again
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[6]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(8f);

        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[10]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(6f);

        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[11]);
        yield return new WaitUntil(() => DialogueManager.Instance.NoDialoguePlaying);
        yield return new WaitForSeconds(10f);


        _conveyorThree.FlowerSupply = false;

        yield return new WaitForSeconds(2f); // Wait untill we notice

        _clickAreaLeft.CanInteract = true;
        _clickAreaRight.CanInteract = true;
        yield return new WaitForSeconds(1f); // Wait untill we notice

        // Huh, it stoped. Checkup on boss
        DialogueManager.Instance.AddDialogueEventToStack(dialogueEvents[7]);

        // DONE
        _elevator.AllowButtonClick(true);
        yield return new WaitUntil(() => _elevator.ElevatorMoving);
        yield return new WaitForSeconds(10);
        SceneHandler.Instance.LoadOverGroundAsync();
    }


    private void Awake() => CameraMovement.CurrentCameraView += CameraViewChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraView -= CameraViewChanged;
    private void CameraViewChanged(Transform t) {
        if (t == _sortPosView) {
            _isLookingAtSort = true;
        }
        if(t == _crushPosView) {
            _isLookingAtCrush = true;
        }
    }
}