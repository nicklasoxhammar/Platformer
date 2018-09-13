using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;

    public float speed = 15.0f;
    public float jumpForce = 1700.0f;
    public float dashForce = 50.0f;
    public float startDashTime = 0.5f;
    public float dashCooldownTime = 2.0f;

    float dashDirection = 1.0f;



    [HideInInspector]public bool isDashing = false;
    [HideInInspector] public bool freezeMovement = false;
    bool dashCooldown = false;

    private float timer = 0;
    private float dashTime;

    Transform groundCheck;
    const float groundedRadius = 0.4f;
    [SerializeField] private LayerMask whatIsGround;


    bool isGrounded = true;

    void Start() {

        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        dashTime = startDashTime;

    }

    void FixedUpdate() {

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

        if (freezeMovement) {
            return;
        }


        if (transform.position.y < -50f) {
            Die();
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded) {

            rb.AddForce(new Vector2(0.0f, jumpForce));
        }

        if (CrossPlatformInputManager.GetButtonUp("Dash")) {
            isDashing = false;
        }

        if (isDashing) {
            dashTime -= Time.deltaTime;

            if (dashTime <= 0) {
                isDashing = false;
                dashCooldown = true;
                dashTime = startDashTime;
            }
        }

        if (dashCooldown) {
            timer -= Time.deltaTime;

            if (timer <= 0) {
                dashCooldown = false;
                timer = dashCooldownTime;
            }
        }

    }

    void Move() {

        if (freezeMovement) {
            return;
        }


        float direction = CrossPlatformInputManager.GetAxis("Horizontal");

        //set direction of dash
        if (!isDashing) {
            if(direction < 0.0f) {
                dashDirection = -1.0f;
            }
            else {
                dashDirection = 1.0f;
            }
        }

        //Dash
        if (CrossPlatformInputManager.GetButton("Dash") && !dashCooldown) {
            isDashing = true;

            rb.velocity = new Vector2(dashDirection * dashForce, rb.velocity.y);
            //Enable camera shake
            Camera.main.gameObject.GetComponent<CameraShake>().shake = true;
        }
        //Move at regular speed
        else {

            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            //Disable camera shake
            Camera.main.gameObject.GetComponent<CameraShake>().shake = false;
       }

    }

    public void Die() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
