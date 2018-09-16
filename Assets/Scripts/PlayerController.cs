using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using Spine.Unity;
public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;

    public float speed = 15.0f;
    public float jumpForce = 1700.0f;
    public float dashForce = 50.0f;
    public float startDashTime = 0.5f;
    public float dashCooldownTime = 2.0f;
    public float cameraShakeTime = 1f;
    float dashDirection = 1.0f;



    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool freezeMovement = false;
    bool dashCooldown = false;

    private float timer = 0;
    private float dashTime;

    Transform groundCheck;
    const float groundedRadius = 0.4f;
    [SerializeField] private LayerMask whatIsGround;


    bool isGrounded = true;

    private SkeletonAnimation skeletonAnimation;
        



    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        dashTime = startDashTime;
        timer = dashCooldownTime;

    }

    void FixedUpdate()
    {
        
        MoveHorizontal();

    }

    void Update()
    {
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        Jump();



        if (transform.position.y < -50f)
        {
            Die();
        }


        DashTimers();

    }

    private void DashTimers()
    {
        if (CrossPlatformInputManager.GetButtonUp("Dash"))
        {
            isDashing = false;
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;

            if (dashTime <= 0)
            {
                isDashing = false;
                dashCooldown = true;
                dashTime = startDashTime;
            }
        }

        if (dashCooldown)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                dashCooldown = false;
                timer = dashCooldownTime;
            }
        }
    }



    private void MoveHorizontal()
    {
        if (freezeMovement){return;}


        //Right
        if (CrossPlatformInputManager.GetAxisRaw("Horizontal") > 0f)
        {
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
            skeletonAnimation.AnimationName = "RUN";
            Dash(dashForce);
        }
        //Left
        else if (CrossPlatformInputManager.GetAxisRaw("Horizontal") < 0f)
        {
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
            skeletonAnimation.AnimationName = "RUN";
            Dash(-dashForce);

        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            ShowRightAnimation();
            //Dash when no movement
            Dash(dashForce);
        }


    }

    private void Jump()
    {
        if (freezeMovement) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded)
        {
            
            rb.AddForce(new Vector2(0.0f, jumpForce));
        }
    }


    private void ShowRightAnimation()
    {
        if(rb.velocity.y < 0 && !isGrounded)
        {
            skeletonAnimation.AnimationName = "FALLING";

        }
        else if(rb.velocity.y > 0 && !isGrounded)
        {
            skeletonAnimation.AnimationName = "JUMP";

        }
        else
        {
            skeletonAnimation.AnimationName = "STANDING";
        }
    }



    private void Dash(float force)
    {

        if (CrossPlatformInputManager.GetButton("Dash") && !dashCooldown)
        {
            isDashing = true;
            rb.velocity = new Vector2(force, rb.velocity.y);
            skeletonAnimation.AnimationName = "RUN";
            StartCoroutine(StartCameraShake());

        }

    }


    IEnumerator StartCameraShake()
    {
        Camera.main.gameObject.GetComponent<CameraShake>().shake = true;
        yield return new WaitForSeconds(cameraShakeTime);
        Camera.main.gameObject.GetComponent<CameraShake>().shake = false;

    }




    public void Die()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
