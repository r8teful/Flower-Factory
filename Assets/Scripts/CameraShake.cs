using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;


            transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}