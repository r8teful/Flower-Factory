using System.Collections;
using UnityEngine;

public class CreatureSound : MonoBehaviour {
    [SerializeField] private Transform _outOfCagePos;

    public bool Angry { get; set; }

    public void StartCreatureSound() {
        StartCoroutine(RandomSound());
    }

    public void StartHitSound() {
        transform.position = _outOfCagePos.position;
        StartCoroutine(LoopHitSound());
    }
    public void StopAllSound() {
        StopAllCoroutines();
    }
    public void PlaySoundAngry(int i) {
        if (i == 1) {
            AudioController.Instance.PlaySound3D("monsterAngry#1", transform.position, distortion: new AudioParams.Distortion(false, true));
        } else if (i == 2) {
            AudioController.Instance.PlaySound3D("monsterAngry#2", transform.position, distortion: new AudioParams.Distortion(false, true));
        } else if (i == 3) {
            AudioController.Instance.PlaySound3D("monsterAngry#3", transform.position, distortion: new AudioParams.Distortion(false, true));
        } else if (i == 4) {
            AudioController.Instance.PlaySound3D("monsterAngry#4", transform.position, distortion: new AudioParams.Distortion(false, true));
        } else if (i == 5) {
            AudioController.Instance.PlaySound3D("monsterAngry#5", transform.position, distortion: new AudioParams.Distortion(false, true));
        }
    }

    public void StartCreatureSoundAngry() {
        StartCoroutine(RandomAngrySound());
    }


    private IEnumerator RandomSound() {
        PlayCreatureSound(); // Start with a sound
        while (!Angry) {
            yield return new WaitForSeconds(Random.Range(10, 20));
            PlayCreatureSound();
        }
    }
    private IEnumerator RandomAngrySound() {
        while (Angry) {
            yield return new WaitForSeconds(Random.Range(5, 7));
            PlayCreatureSoundAngry();
        }
    }

    private IEnumerator LoopHitSound() {
        var s = FindObjectOfType<CameraShake>();
        AudioController.Instance.PlaySound3D("wallHit", transform.position, distortion: new AudioParams.Distortion(false, true));
        StartCoroutine(s.Shake(0.4f, 0.2f));
        while (true) {
            yield return new WaitForSeconds(Random.Range(2, 3));
            StartCoroutine(s.Shake(0.4f, 0.2f));
            AudioController.Instance.PlaySound3D("wallHit", transform.position, distortion: new AudioParams.Distortion(false, true));
        }
    }

    private void PlayCreatureSound() {
        AudioController.Instance.PlaySound3D("monster", transform.position,
        randomization: new AudioParams.Randomization(), distortion: new AudioParams.Distortion(false, true));
    }
    private void PlayCreatureSoundAngry() {
        AudioController.Instance.PlaySound3D("monsterAngry", transform.position,
        randomization: new AudioParams.Randomization(), distortion: new AudioParams.Distortion(false, true));
    }
}