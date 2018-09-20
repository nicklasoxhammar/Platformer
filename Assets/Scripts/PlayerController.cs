using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using Spine.Unity;
public class PlayerController : MonoBehaviour {

    public float speed = 400.0f;
    public float jumpForce = 500.0f;
    public float dashForce = 50.0f;
    public float startDashTime = 0.5f;
    public float dashRefreshTime = 0.05f;

    float dashDirection = 1.0f;
    bool isGrounded = true;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool freezeMovement = false;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public float dashTime;
    [HideInInspector] public bool isCarryingBox = false;

    public float direction = 1.0f;

    Transform groundCheck;
    const float groundedRadius = 0.4f;

    [SerializeField] private LayerMask whatIsGround;


    private SkeletonAnimation skeletonAnimation;




    void Start() {
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        dashTime = startDashTime;
    }

    void FixedUpdate() {

        MoveHorizontal();
        //limit player velocity
        if (rb != null) {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, dashForce);
        }

    }

    void Update() {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        Jump();



        if (transform.position.y < -50f) {
            Die();
        }


        DashTimers();

    }

    private void DashTimers() {

        if (isDashing) {
            dashTime -= Time.deltaTime;

            if (dashTime <= 0) {
                isDashing = false;
                canDash = false;
            }
        }
        else {
            if (dashTime < startDashTime) {
                dashTime += Time.deltaTime * dashRefreshTime;
            }

            if (dashTime > startDashTime * 0.25) {
                canDash = true;
            }
        }

    }




    private void MoveHorizontal() {
        if (freezeMovement) { return; }

        //Right
        if (CrossPlatformInputManager.GetAxisRaw("Horizontal") > 0f) {
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
            skeletonAnimation.AnimationName = "RUN";
            Dash(dashForce);

            direction = 1.0f;
        }
        //Left
        else if (CrossPlatformInputManager.GetAxisRaw("Horizontal") < 0f) {
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            skeletonAnimation.AnimationName = "RUN";
            Dash(-dashForce);

            direction = -1.0f;

        }
        else {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            ShowRightAnimation();
            //Dash when no movement
            Dash(dashForce);
        }


    }

    private void Jump() {
        if (freezeMovement) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded) {

            rb.AddForce(new Vector2(0.0f, jumpForce));
        }
    }


    private void ShowRightAnimation() {
        if (rb.velocity.y < 0 && !isGrounded) {
            skeletonAnimation.AnimationName = "FALLING";

        }
        else if (rb.velocity.y > 0 && !isGrounded) {
            skeletonAnimation.AnimationName = "JUMP";

        }
        else {
            skeletonAnimation.AnimationName = "STANDING";
        }
    }



    private void Dash(float force) {

        if (CrossPlatformInputManager.GetButton("Dash") && canDash && !isCarryingBox) {
            isDashing = true;
            rb.velocity = new Vector2(force, rb.velocity.y);
            skeletonAnimation.AnimationName = "RUN";
            Camera.main.gameObject.GetComponent<CameraShake>().shake = true;

        }
        else {
            isDashing = false;
            Camera.main.gameObject.GetComponent<CameraShake>().shake = false;
        }

    }


    public void Die() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
