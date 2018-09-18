using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Enemy : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpForce = 500.0f;
    [SerializeField] float distance = 10.0f;
    [SerializeField] GameObject deathParticles;

    float direction = 1;

    float startingXPos;
    float startingYPos;

    private SkeletonAnimation skeletonAnimation;

    void Start () {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();

        startingXPos = transform.position.x;
        startingYPos = transform.position.y;

	}

	void Update () {

        Vector3 position = transform.position;
        position.x += speed * direction * Time.deltaTime;
        skeletonAnimation.AnimationName = "RUN";

        transform.position = position;


        if (position.x > startingXPos + distance){
            direction = -1;
        }

        if (position.x < startingXPos - distance) {
            direction = 1;
        }

        transform.localScale = new Vector3(direction, 1.0f, 1.0f);

    }

    private void FixedUpdate() {
        //limit velocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, jumpForce / 50);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if(collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player.isDashing) {
                Die();
            }
            else {
                player.Die();
            }
        }


        if(collision.gameObject.tag == "Ground") {

            rb.AddForce(new Vector2(0.0f, jumpForce));
        }
    }


    private void ShowRightAnimation() {
        if (rb.velocity.y < 0) {
            skeletonAnimation.AnimationName = "FALLING";

        }
        else if (rb.velocity.y > 0) {
            skeletonAnimation.AnimationName = "JUMP";

        }
        else {
            skeletonAnimation.AnimationName = "STANDING";
        }
    }

    void Die() {

        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);

        Destroy(gameObject);
    }


}
