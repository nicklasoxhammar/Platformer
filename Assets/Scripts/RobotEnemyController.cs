using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public class RobotEnemyController : MonoBehaviour
{
    [SerializeField] Collider2D playerToFollow;

    private SkeletonAnimation skeletonAnimation;
    private Animator animator;
    private Rigidbody2D rb;
    Bone eye;
    Bone playerHead;
    private string eyeBoneName = "Pupill";
    private string playerHeadBoneName = "Huvud";
    private bool isDead = false;
    public bool isFreezed = false;
    private bool playerInSight = false;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    private int direction = 1;


    private string walkAnimationName = "WALK";
    private string dieAnimationName = "DIE";
    private string runAnimationName = "RUN";

    private float secFadeAway = 0.5f;

    // Use this for initialization
    void Start()
    {
        if (playerToFollow == null) { return; }

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        eye = skeletonAnimation.skeleton.FindBone(eyeBoneName);
        playerHead = playerToFollow.GetComponent<SkeletonAnimation>().skeleton.FindBone(playerHeadBoneName);
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        skeletonAnimation.AnimationState.Complete += AnimationCompleteListener;

        skeletonAnimation.AnimationState.SetAnimation(0, walkAnimationName, true);


    }

    private void AnimationCompleteListener(TrackEntry trackEntry)
    {
        if(trackEntry.animation.Name == dieAnimationName)
        {
            //Fade away and destroy
            LeanTween.value(1f, 0f, secFadeAway).setEaseInCubic().setOnUpdate((float val) => {
                skeletonAnimation.skeleton.a = val;
            }).setOnComplete(() => {
                Destroy(gameObject);
            });



        }
    }


    // Update is called once per frame
    void Update()
    {

        CheckPlayerInsight();

        if (playerInSight)
        {
            MoveWithThisSpeed(runSpeed);
        }
        else
        {
            MoveWithThisSpeed(walkSpeed);
        }

        //Face the direction....but not at the edge.
        if (!isFreezed)
        { 
        transform.localScale = new Vector3(direction, 1f, 1f);
        }

    }

    private void MoveWithThisSpeed(float speed)
    {
        if (isDead != true && !isFreezed)
        {
            Vector2 position = transform.position;
            position.x += speed * direction * Time.deltaTime;
            transform.position = position;
        }

    }

    //FÖLJA ÖGONEN

    private void CheckPlayerInsight()
    {
        Vector3 eyePos = eye.GetWorldPosition(skeletonAnimation.transform);
        Vector3 targetPos = playerToFollow.bounds.center;
        RaycastHit2D hit = Physics2D.Linecast(eyePos, targetPos);

        //DEBUG THING:..
        Debug.DrawLine(eyePos, targetPos);
        if (hit != false){
            Debug.DrawLine(eyePos, hit.point, Color.red);

        }
       ////
         if (hit.collider.tag == "Player" && !isDead && !playerInSight)
        {
            playerInSight = true;
            skeletonAnimation.AnimationState.SetAnimation(0, runAnimationName, true);

            bool directionToLeft = hit.point.x < eyePos.x;
            if(directionToLeft)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
        }
        else if (hit.collider.tag != "Player" && !isDead && playerInSight)
        {
            playerInSight = false;
            isFreezed = false;
            skeletonAnimation.AnimationState.SetAnimation(0, walkAnimationName, true);
        }
            
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Block")
        {
            if (playerInSight)
            {
                isFreezed = true;
                skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0.5f, 0f);

            }
                direction *= -1;
                    


            //if(playerInSight)
            //{
            //    isFreezed = true;
            //    skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0.5f, 0f);
            //}
           //else
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player.isDashing)
            {
                Die();
            }
            else if(!isDead)
            {
                player.Die();
            }
        }
    }



    private void Die()
    {
        isDead = true;
        //Make it small så player can jump over it when die.
        BoxCollider2D thisCollider = GetComponent<BoxCollider2D>();
        Vector2 size = thisCollider.size;
        size.y *= 0.1f;
        thisCollider.size = size;
        Vector2 colliderOffset = thisCollider.offset;
        colliderOffset.y *= 0.1f;
        thisCollider.offset = colliderOffset;
        //Die Animation and trigger when its done..
        skeletonAnimation.AnimationState.SetAnimation(0, dieAnimationName, false);


        //KANSKE MOLN NÄR DEN FÖRRSVINNER?
    }

}
