using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndgameSequence : Sequencer {
    [SerializeField] private Transform _powerOffSource;
    [SerializeField] private Transform _explosionPos;
    [SerializeField] private RawImage _blackScreen;
    private bool _hasEnteredexplosionPos;

    protected override IEnumerator Sequence() {
        var monster = FindObjectOfType<CreatureSound>();
        Debug.Log("Endgame Start");
        // Todo posibly lock movement
        CameraMovement.Instance.LockMovement = true; 
        CameraMovement.Instance.SetMoveSpeed(6); // half the move speed

        AudioController.Instance.PlaySound3D("powerOff", _powerOffSource.position, distortion: new AudioParams.Distortion(false, true));
        RenderSettings.fogDensity = 0.07f;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fog = true;
        yield return new WaitForSeconds(4);

        monster.Angry = true;
        monster.PlaySoundAngry(5);// angry sound
        yield return new WaitForSeconds(5);
        AudioController.Instance.PlaySound3D("itsEscaping", _powerOffSource.position, distortion: new AudioParams.Distortion(false, true));
        CameraMovement.Instance.LockMovement = false; 
        yield return new WaitForSeconds(6);
        monster.PlaySoundAngry(1);// angry sound
        monster.StartHitSound();
        yield return new WaitForSeconds(4);
        monster.PlaySoundAngry(4);// angry sound
        monster.StartCreatureSoundAngry();
        yield return new WaitUntil(() => _hasEnteredexplosionPos);
        AudioController.Instance.PlaySound3D("wallBreak", _powerOffSource.position, distortion: new AudioParams.Distortion(false, true));
        _blackScreen.enabled = true;
        yield return new WaitForSeconds(4);
        SceneHandler.Instance.LoadEndMenu();
    }

    private void Awake() => CameraMovement.CurrentCameraPos += CameraPosChanged;
    private void OnDestroy() => CameraMovement.CurrentCameraPos -= CameraPosChanged;
    private void CameraPosChanged(Transform t) {
        if (t == _explosionPos) {
            _hasEnteredexplosionPos = true;
        }
    }
}