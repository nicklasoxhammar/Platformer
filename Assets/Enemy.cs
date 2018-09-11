using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] float speed = 5.0f;
    [SerializeField] float distance = 10.0f;

    float direction = 1;

    float startingXPos;

	void Start () {

        startingXPos = transform.position.x;
	}

	void Update () {

        Vector3 position = transform.position;
        position.x += speed * direction * Time.deltaTime;
        transform.position = position;


        if (position.x >= startingXPos + distance || position.x <= startingXPos - distance) {
            ChangeDirection();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if(collision.gameObject.tag == "Player") {
            Debug.Log("DIE!");

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player.isDashing) {
                Debug.Log("kill enemy");
                Die();
            }
            else {
                player.Die();
            }
        }
    }

    void ChangeDirection() {

        direction *= -1;

    }

    void Die() {

        Destroy(gameObject);
    }


}
