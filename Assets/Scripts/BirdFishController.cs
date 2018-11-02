using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class BirdFishController : MonoBehaviour
{
    [SerializeField] bool dontJumpAtAll = false;
    [SerializeField] bool flyCrazyWhenWalking = false;
    [SerializeField] bool smallJumpWhenWalk = false;
    [SerializeField] private float minJumpTime = 5f;
    [SerializeField] private float maxJumpTime = 10f;
    [SerializeField] [Range(2000f, 7500f)] float minJumpForce = 3000f;
    [SerializeField] [Range(2000f, 7500f)] float maxJumpForce = 3000f;
    [SerializeField] float walkingSpeed = 300f;
    [SerializeField] ParticleSystem dieVFX;

    [SerializeField] GameObject BoundingBoxForBody;
    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;
    private bool isjumping = false;
    private float jumpForce = 2000f;
    private float jumpTimer = 0;
    private bool isWalking = false;

    [SerializeField] float timeBetweenChew = 2f;
    private float chewTimer = 2f;

    private bool isInWater = false;
    private float direction;
    const float groundedRadius = 0.4f;

    private bool isDead = false;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded = false;

    AudioSource audioSource;
    [SerializeField] AudioClip deathSound;

    //ANIMATION NAMES...
    private string chewAnimationName = "Chew";
    private string flyAnimationName = "Fly";
    private string flyCrazyAnimationName = "FlyCrazy";
    private string jumpDownAnimationName = "JumpDown";
    private string jumpUpAnimationName = "JumpUp";
    private string smallJumpAnimationName = "SmallJump";
    private string swimAnimationName = "Swim";
    private string wingsDownAnimationName = "WingsDown";
    private string wingsUpAnimationName = "WingsUp";
    private string walkAnimationName = "walk";
    private string dieAnimationName = "Die";

    Spine.Animation jumpUpAnimation;
    Spine.Animation chewAnimation;


    // Use this for initialization
    void Start()
    {
        chewTimer = timeBetweenChew;
        direction = transform.localScale.x * -1;
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        audioSource = GetComponent<AudioSource>();

        jumpUpAnimation = skeletonAnimation.skeleton.Data.FindAnimation(jumpUpAnimationName);

        skeletonAnimation.AnimationState.SetAnimation(0, swimAnimationName, true);
        GetRandomJumpTime();
        GetRandomJumpForce();
        skeletonAnimation.AnimationState.Complete += AnimationCompleteListener;
    }




    // Update is called once per frame
    void Update()
    {
        StartJumpTimer();
        StartChewTimer();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        if (isGrounded && !isWalking && !isDead)
        {
            isWalking = true;
            if(smallJumpWhenWalk)
            {
                skeletonAnimation.AnimationState.SetAnimation(4, smallJumpAnimationName, true);
            }
            else
            {
                skeletonAnimation.AnimationState.SetAnimation(4, walkAnimationName, true);
            }
            if(flyCrazyWhenWalking)
            {
                skeletonAnimation.AnimationState.SetAnimation(2, flyCrazyAnimationName, true);
            }
        }

    }

    private void FixedUpdate()
    {
        if (isGrounded && !isDead)
        {
            rb.velocity = (new Vector2(direction * walkingSpeed * Time.deltaTime, 0f));
        }
    }

    private void AnimationCompleteListener(TrackEntry trackEntry)
    {
        if (trackEntry.animation.Name == jumpUpAnimationName)
        {
            rb.AddForce(Vector2.up * jumpForce);
            isjumping = true;

        }
        else if (trackEntry.animation.name == jumpDownAnimationName)
        {
            isWalking = false;
        }

        else if (trackEntry.animation.Name == dieAnimationName)
        {
            //Dust Effect
            if (dieVFX != null)
            {
                ParticleSystem vfx = Instantiate(dieVFX, transform);
                Destroy(vfx, vfx.main.duration);
            }
            //Fade away and destroy
            LeanTween.value(1f, 0f, dieVFX.main.duration).setEaseOutCubic().setOnUpdate((float val) => {
                skeletonAnimation.skeleton.a = val;
            }).setOnComplete(() => {
                Destroy(gameObject);
            });
        }

    }

    //Called from Update
    private void StartJumpTimer()
    {
        if (!isjumping && !dontJumpAtAll && !isDead)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
            {
                GetRandomJumpTime();
                GetRandomJumpForce();
                skeletonAnimation.AnimationState.AddEmptyAnimation(4, 0f, 0f);
                skeletonAnimation.AnimationState.AddEmptyAnimation(2, 0f, 0f);
                skeletonAnimation.AnimationState.SetAnimation(1, jumpUpAnimation, false);
                skeletonAnimation.AnimationState.SetAnimation(2, flyAnimationName, true);
                skeletonAnimation.AnimationState.SetAnimation(3, chewAnimationName, true);

            }
        }
    }
    //Called from Update
    private void StartChewTimer()
    {
        chewTimer -= Time.deltaTime;
        if (chewTimer <= 0 && !isDead)
        {
            chewTimer = timeBetweenChew;
            skeletonAnimation.AnimationState.SetAnimation(3, chewAnimationName, false);
        }
    }

    private void GetRandomJumpTime()
    {
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
    }

    private void GetRandomJumpForce()
    {
        jumpForce = Random.Range(minJumpForce, maxJumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When falling in to water from jumping up..
        if (collision.gameObject.tag == "Water" && isjumping)
        {
            SetRightAnimationPostJump();
        }
        else if(collision.gameObject.tag == "Block")
        {
            gameObject.transform.localScale = new Vector3(direction, 1f, 1f);

            direction *= -1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("HE");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.isDashing && !isDead)
            {
                Die();
            }
        }
        else if (collision.gameObject.tag == "KillsEnemy" && !isDead)
        {
            Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGrounded && isjumping)
        {
            SetRightAnimationPostJump();
        }
    }


    private void SetRightAnimationPostJump()
    {
        skeletonAnimation.AnimationState.SetAnimation(1, jumpDownAnimationName, false);
        skeletonAnimation.AnimationState.AddEmptyAnimation(2, 0, 0);
        skeletonAnimation.AnimationState.AddEmptyAnimation(3, 0, 0);
        isjumping = false;
    }

    private void Die()
    {
        Debug.Log("DUE");
        if (!isDead)
        {
            isDead = true;
            //Ignore player layer.
            gameObject.layer = 11;
            BoundingBoxForBody.gameObject.layer = 11;
            //Die Animation and trigger when its done..
            rb.velocity = Vector2.zero;
            skeletonAnimation.AnimationState.ClearTracks();
            skeletonAnimation.AnimationState.SetAnimation(0, dieAnimationName, false);

            audioSource.clip = deathSound;
            audioSource.Play();
        }

    }
}
