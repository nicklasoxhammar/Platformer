using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;

    public float speed = 15.0f;
    public float jumpForce = 1700.0f;

    Transform groundCheck;
    const float groundedRadius = 0.4f;
    [SerializeField] private LayerMask whatIsGround;


    bool isGrounded = true;

	void Start () {

        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");

    }

	void FixedUpdate () {

        Move();


        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject)
                isGrounded = true;
        }
    }

    void Update() {

        if (transform.position.y < -50f) {
            Die();
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded) {

            rb.AddForce(new Vector2(0.0f, jumpForce));
        }

    }

    void Move() {

        float direction = CrossPlatformInputManager.GetAxis("Horizontal");
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

    }

    void Die() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
