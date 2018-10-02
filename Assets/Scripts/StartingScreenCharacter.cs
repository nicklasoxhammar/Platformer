using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScreenCharacter : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine(MoveAround());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator MoveAround() {

        float duration = 1.0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-5.0f, 3.0f), Random.Range(-4.0f, 10.0f));
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 180.0f;

        float t = 0.0f;
        while (t < duration) {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            Vector3 position = Vector3.Lerp(startPosition, endPosition, t / duration);
            transform.eulerAngles = new Vector3(0, 0, zRotation);
            transform.position = position;
            yield return null;
        }

        StartCoroutine(MoveAround());
    }
}
