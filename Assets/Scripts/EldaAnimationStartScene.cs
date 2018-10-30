using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EldaAnimationStartScene : MonoBehaviour
{

    private string fallingAnimationName = "FALLING";
    private string jumpAnimationName = "JUMP";
    private string idleAnimationName = "STANDING";
    private string runAnimationName = "RUN";
    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;
    public Transform skateboard;
    public GameObject saveTheWorldText;
    private bool moveSkateboardToElda = false;
    private float moveSkateboardSpeed = 7f;
    private float moveEldaSpeed = 6f;
    private bool MoveToTheRight = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowRightAnimation();
        if(moveSkateboardToElda && Vector3.Distance(skateboard.position,gameObject.transform.position) > 0.1f)
        {
            Vector3 newPos = Vector3.Lerp(skateboard.position, gameObject.transform.position, moveSkateboardSpeed * Time.deltaTime);
            newPos.y = skateboard.position.y;
            skateboard.position = newPos;
        }
    }

    private void FixedUpdate()
    {
        if (MoveToTheRight)
        {
            //rb.AddRelativeForce(Vector2.right * moveEldaSpeed);
            rb.AddForce(Vector2.right * moveEldaSpeed);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //SKATEBOARD
            transform.localScale = new Vector3(-1f, 1f, 1f);
            saveTheWorldText.transform.SetParent(saveTheWorldText.transform.root);
            skateboard.transform.SetParent(gameObject.transform);
            skateboard.transform.localPosition = Vector3.zero;
            gameObject.transform.parent = saveTheWorldText.transform;
            LeanTween.moveX(saveTheWorldText, 0f, 2f).setOnComplete(() =>
            {
                gameObject.transform.SetParent(transform.root);
                LeanTween.moveX(gameObject, -20, 2f);
            });
        }
        if(collision.gameObject.tag == "Letters")
        {
            MoveToTheRight = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EndOfGround")
        {
            //jump
            rb.AddRelativeForce(new Vector2(0f,1f) * 330);
        }
        else if(collision.gameObject.tag == "Finish")
        {
            moveSkateboardToElda = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Letters")
        {
            MoveToTheRight = false;
        }
    }

    //Called from Update
    private void ShowRightAnimation()
    {
        if (rb != null)
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
