using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] float minShakeValue = 0.1f;
    [SerializeField] float maxShakeValue = 1.0f;

    [HideInInspector] public bool shake = false;



    void Update() {

        if (shake) {
            transform.rotation = Quaternion.Euler(0.0f, Random.Range(minShakeValue, maxShakeValue), Random.Range(minShakeValue, maxShakeValue));
        }
        else {
            transform.rotation = Quaternion.identity;
        }

    }
}
