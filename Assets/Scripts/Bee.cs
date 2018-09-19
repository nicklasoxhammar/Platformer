using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour {

    [SerializeField] float speed = 5.0f;
    [SerializeField] float distance = 10.0f;
    [SerializeField] bool upAndDown = false;

    private GameObject target = null;
    private Vector3 offset;

    float direction = 1;

    Vector3 scale;
    Vector3 position;

    float startingXPos;
    float startingYPos;

    void Start() {

        startingXPos = transform.position.x;
        startingYPos = transform.position.y;

        scale = transform.localScale;
    }

    void FixedUpdate() {

        position = transform.position;

        if (upAndDown) {

            position.y += speed * direction * Time.deltaTime;

            if (position.y > startingYPos + distance) {
                direction = -1;
            }

            if (position.y < startingYPos - distance) {
                direction = 1;
            }

        }

        if (upAndDown == false) {

            position.x += speed * direction * Time.deltaTime;

            if (position.x > startingXPos + distance) {
                direction = -1;
            }

            if (position.x < startingXPos - distance) {
                direction = 1;
            }

        }

        transform.localScale = new Vector3(scale.x * direction, scale.y, scale.z);
        transform.position = position;

        if (target != null) {
            target.transform.position = transform.position + offset;
        }

    }


    void OnCollisionStay2D(Collision2D col) {

        if (col.gameObject.tag == "Player") {
            target = col.gameObject;
            offset = target.transform.position - transform.position;
        }
    }
    void OnCollisionExit2D(Collision2D col) {
        target = null;
    }


}
