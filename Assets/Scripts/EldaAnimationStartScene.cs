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
    private float skateboardEndPosX;
    public GameObject saveTheWorldText;
    public Vector3 jumpOffset;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skateboardEndPosX = skateboard.transform.position.x;
        skateboard.position = new Vector3(skateboardEndPosX + 10f, skateboard.transform.position.y);

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
                LeanTween.move(gameObject, gameObject.transform.position + jumpOffset, 0.5f);
                moveCounter++;
            });
        }
         if(moveCounter == movingPoints.childCount-1)
            {
            LeanTween.moveX(skateboard.gameObject, skateboardEndPosX, 1f);
            }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            //SKATEBOARD
            transform.localScale = new Vector3(-1f, 1f, 1f);
            saveTheWorldText.transform.SetParent(saveTheWorldText.transform.root);
            skateboard.transform.SetParent(gameObject.transform);
            skateboard.transform.localPosition = Vector3.zero;

            gameObject.transform.parent = saveTheWorldText.transform;
            LeanTween.moveX(saveTheWorldText, 0f, 3f).setOnComplete(() => 
            {
                gameObject.transform.SetParent(transform.root);
                LeanTween.moveX(gameObject, -20, 2f);
            });
        }
        else
        {
            MoveAndJump();
        }
    }

    private void ShowRightAnimation()
    {
        if(rb!=null)
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
}
