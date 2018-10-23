using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepMovingRight : MonoBehaviour {

    float speed = 10.0f;

	void Update () {
        transform.position = new Vector3(transform.position.x + 1 * speed * Time.deltaTime, transform.position.y);

    }
}
