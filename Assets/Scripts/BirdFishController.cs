using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class BirdFishController : MonoBehaviour
{

    [SerializeField] private float minJumpTime = 5f;
    [SerializeField] private float maxJumpTime = 10f;
    [SerializeField] [Range(2000f, 7500f)] float minJumpForce = 3000f;
    [SerializeField] [Range(2000f, 7500f)] float maxJumpForce = 3000f;
    [SerializeField] float minTimeFlaxOrChew = 2f;
    [SerializeField] float maxTimeFlaxOrChew = 5f;

    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;
    private bool isjumping = false;
    private float jumpForce = 2000f;
    public float jumpTimer = 0;

    private float chewOrFlaxTimer = 2f;

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
    private string walkAnimationName = "Walk";

    Spine.Animation jumpUpAnimation;
    Spine.Animation chewAnimation;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        jumpUpAnimation = skeletonAnimation.skeleton.Data.FindAnimation(jumpUpAnimationName);

        skeletonAnimation.AnimationState.SetAnimation(0, swimAnimationName, true);
        GetRandomJumpTime();
        GetRandomJumpForce();
        GetRandomFlaxChewTime();
        skeletonAnimation.AnimationState.Complete += AnimationCompleteListener;



    }




    // Update is called once per frame
    void Update()
    {
        StartJumpTimer();
        StartChewTimer();
    }

    private void AnimationCompleteListener(TrackEntry trackEntry)
    {
        if (trackEntry.animation.Name == jumpUpAnimationName)
        {
            rb.AddForce(Vector2.up * jumpForce);
            //skeletonAnimation.AnimationState.SetAnimation(1, jumpDownAnimation, false);

        }
    }

    //Called from Update
    private void StartJumpTimer()
    {
        if (!isjumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0 && !isjumping)
            {
                isjumping = true;
                GetRandomJumpTime();
                GetRandomJumpForce();
                skeletonAnimation.AnimationState.SetAnimation(1, jumpUpAnimation, false);
                skeletonAnimation.AnimationState.SetAnimation(2, flyAnimationName, true);
                skeletonAnimation.AnimationState.SetAnimation(3, chewAnimationName, true);

            }
        }
    }
    //Called from Update
    private void StartChewTimer()
    {
        chewOrFlaxTimer -= Time.deltaTime;
        if (chewOrFlaxTimer <= 0)
        {
            GetRandomFlaxChewTime();

            if (Random.Range(0, 2) == 0)
            {
                skeletonAnimation.AnimationState.SetAnimation(3, chewAnimationName, false);
            }
            else
            {
                skeletonAnimation.AnimationState.SetAnimation(3, flyAnimationName, false);
            }

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

    private void GetRandomFlaxChewTime()
    {
        chewOrFlaxTimer = Random.Range(minTimeFlaxOrChew, maxTimeFlaxOrChew);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When falling in to water from jumping up..
        if (collision.gameObject.tag == "Water" &&
            skeletonAnimation.AnimationState.GetCurrent(1) != null
            && skeletonAnimation.AnimationState.GetCurrent(1).animation == jumpUpAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(1, jumpDownAnimationName, false);
            skeletonAnimation.AnimationState.AddEmptyAnimation(2, 0, 0);
            skeletonAnimation.AnimationState.AddEmptyAnimation(3, 0, 0);
            isjumping = false;
        }
    }

}
