using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using Spine.Unity;
public class PlayerController : MonoBehaviour {



    private string fallingAnimationName = "FALLING";
    private string jumpAnimationName = "JUMP";
    private string idleAnimationName = "STANDING";


    public float speed = 400.0f;
    public float jumpForce = 500.0f;
    public float dashForce = 50.0f;
    public float startDashTime = 0.5f;
    public float dashRefreshTime = 0.05f;

    bool isGrounded = true;
    bool dead = false;
    public bool collidingWithInteractableThing;    

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool freezeMovement = false;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public float dashTime;
    [HideInInspector] public bool isCarryingBox = false;
    [HideInInspector] public float direction = 1.0f;

    Transform groundCheck;
    const float groundedRadius = 0.4f;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] AudioClip walkingSound;
    [SerializeField] AudioClip jumpingSound;
    [SerializeField] AudioClip dashingSound;
    [SerializeField] AudioClip deathSound;

    AudioSource audioSource;
    GameManager GM;


    private SkeletonAnimation skeletonAnimation;




    void Start() {
        audioSource = GetComponent<AudioSource>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        GM = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        dashTime = startDashTime;
    }

    void FixedUpdate() {

        MoveHorizontal();
        //limit player velocity
        if (rb != null && !freezeMovement) {
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
            PlayAudio("Walk");
        }
        //Left
        else if (CrossPlatformInputManager.GetAxisRaw("Horizontal") < 0f) {
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            skeletonAnimation.AnimationName = "RUN";
            Dash(-dashForce);

            direction = -1.0f;
            PlayAudio("Walk");

        }
        else {
            if (audioSource.clip == walkingSound) {
                audioSource.Stop();
            }
            rb.velocity = new Vector2(0f, rb.velocity.y);
            ShowRightAnimation();
            //Dash when no movement
            Dash(dashForce);
        }


    }


    void PlayAudio(string action) {

        switch (action) {
            case "Walk":
                if (isGrounded && !audioSource.isPlaying) {
                    audioSource.clip = walkingSound;
                    audioSource.Play();
                }
                break;

            case "Jump":
                if (isGrounded) {
                    audioSource.clip = jumpingSound;
                    audioSource.Play();
                }
                break;

            case "Dash":
                audioSource.clip = dashingSound;
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
                break;

            case "Die":
                audioSource.clip = deathSound;
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
                break;

            default:
                break;
        }


    }

    private void Jump() {
        if (freezeMovement) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded) {

            rb.AddForce(new Vector2(0.0f, jumpForce));
            PlayAudio("Jump");
        }
    }


    private void ShowRightAnimation() {
        if (rb.velocity.y < 0 && !isGrounded) {
            skeletonAnimation.AnimationName = fallingAnimationName;

        }
        else if (rb.velocity.y > 0 && !isGrounded) {
            skeletonAnimation.AnimationName = jumpAnimationName;

        }
        else {
            skeletonAnimation.AnimationName = idleAnimationName;
        }
    }



    private void Dash(float force) {

        if (CrossPlatformInputManager.GetButton("Dash") && canDash && !isCarryingBox && !collidingWithInteractableThing) {
            isDashing = true;
            rb.velocity = new Vector2(force, rb.velocity.y);
            skeletonAnimation.AnimationName = "RUN";
            Camera.main.gameObject.GetComponent<CameraShake>().shake = true;
            PlayAudio("Dash");

        }
        else {
            isDashing = false;
            Camera.main.gameObject.GetComponent<CameraShake>().shake = false;
        }

    }


    public void Die() {

        if (dead) { return; }

        dead = true;

        PlayAudio("Die");
        freezeMovement = true;
        rb.velocity = Vector3.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        skeletonAnimation.AnimationName = fallingAnimationName;

        GM.PlayerDied();
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractableThing")
        {
            DashButtonIsInteractable(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractableThing")
        {
            DashButtonIsInteractable(false);
        }
    }


    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "InteractableThing") {
            DashButtonIsInteractable(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "InteractableThing") {
            DashButtonIsInteractable(false);
        }
    }


    private void DashButtonIsInteractable(bool status)
    {
        collidingWithInteractableThing = status;
        GM.dashButtonYellow = status;
    }








}
