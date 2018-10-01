using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class RobotEnemyController : MonoBehaviour
{
    [SerializeField] Transform playerToFollow;

    private SkeletonAnimation skeletonAnimation;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isDead = false;

    [SerializeField] private float speed = 2f;
    private int direction = 1;


    private string walkAnimationName = "WALK";
    private string dieAnimationName = "DIE";
    private string rungAnimationName = "RUN";


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        skeletonAnimation.AnimationState.SetAnimation(0, walkAnimationName, true);
    }

    // Update is called once per frame
    void Update()
    {

        CheckPlayerInsight();
        if(isDead != true)
        {
            Vector2 position = transform.position;
            position.x += speed * direction * Time.deltaTime;
            transform.position = position;
        }

    }

    //FÖLJA ÖGONEN

    private void CheckPlayerInsight()
    {
        //RaycastHit2D hit = Physics2D.Linecast(transform.position, playerToFollow.position);         //if (hit.collider.tag == "Player" && !isDead)
        //{
        //    skeletonAnimation.AnimationState.SetAnimation(0, rungAnimationName, true);

        //    //ÄNDRA RIKTNING
        //}
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EndOfGround")
        {
            Debug.Log("COLLISION!!!!!!!!!");
            direction *= -1;
            transform.localScale = new Vector3(direction, 1f, 1f);
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
            else
            {
                player.Die();
            }
        }
    }



    private void Die()
    {
        isDead = true;
        skeletonAnimation.AnimationState.SetAnimation(0, dieAnimationName, false);
        //KANSKE MOLN NÄR DEN FÖRRSVINNER?
    }

}
