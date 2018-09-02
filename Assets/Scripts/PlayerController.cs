using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;

    public float speed = 15.0f;
    public float jumpForce = 1700.0f;

    bool isGrounded = true;

	void Start () {

        rb = GetComponent<Rigidbody2D>();
		
	}

	void FixedUpdate () {

        Move();
		
	}

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {

            isGrounded = false;

            rb.AddForce(new Vector2(0.0f, jumpForce));
        }

    }

    void Move() {

        float direction = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag == "Ground") { 
        isGrounded = true;
        }
    }
}
