using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EldaAnimationStartScene : MonoBehaviour {

    private string fallingAnimationName = "FALLING";
    private string jumpAnimationName = "JUMP";
    private string idleAnimationName = "STANDING";
    private string runAnimationName = "RUN";
    public Transform movingPoints;
    public Transform startPosBeforeSkate;
    public Transform endPosSkate;
    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;
    private int moveCounter = 0;
    private float speed = 0.15f;
    public Transform skateboard;

    // Use this for initialization
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowRightAnimation();
    }

    private void MoveAndJump()
    {
        if (moveCounter < movingPoints.childCount)
        {
            float distance = Vector2.Distance(transform.position, movingPoints.GetChild(moveCounter).transform.position);
            float time = distance / speed * Time.deltaTime;
            LeanTween.moveX(gameObject, movingPoints.GetChild(moveCounter).transform.position.x, time).setOnComplete(() =>
            {
                //jump;
                rb.AddForce(new Vector2(150, 350));
                if (moveCounter == movingPoints.childCount - 1)
                {
                    skateboard.position = startPosBeforeSkate.position;
                    skateboard.gameObject.SetActive(true);
                    //rb.isKinematic = true;
                    //Invoke("startSkate", 1f);
                }
            });
            moveCounter++;
        }
    }

    private void startSkate()
    {
        gameObject.transform.position = startPosBeforeSkate.position;
        LeanTween.moveX(gameObject, endPosSkate.position.x, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MoveAndJump();
    }

    private void ShowRightAnimation()
    {
        if (rb.velocity.y < 0)
        {
            skeletonAnimation.AnimationName = fallingAnimationName;

        }
        else if (rb.velocity.y > 0)
        {
            skeletonAnimation.AnimationName = jumpAnimationName;
        }
        else
        {
            skeletonAnimation.AnimationName = runAnimationName;
        }
    }
}
