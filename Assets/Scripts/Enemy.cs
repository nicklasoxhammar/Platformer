using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Enemy : MonoBehaviour {

    Rigidbody2D rb;
    AudioSource audioSource;


    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpForce = 500.0f;
    [SerializeField] float distance = 10.0f;
    [SerializeField] float bombDropRate = 5.0f;
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject deathParticles;
    [SerializeField] AudioClip deathSound;

    float direction = 1;

    float startingXPos;
    //float startingYPos;

    float bombTimer = 0.0f;

    private SkeletonAnimation skeletonAnimation;

    void Start() {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        startingXPos = transform.position.x;
        //startingYPos = transform.position.y;

    }

    void Update() {

        Vector3 position = transform.position;
        position.x += speed * direction * Time.deltaTime;
        skeletonAnimation.AnimationName = "RUN";

        transform.position = position;


        if (position.x > startingXPos + distance) {
            direction = -1;
        }

        if (position.x < startingXPos - distance) {
            direction = 1;
        }

        transform.localScale = new Vector3(direction, 1.0f, 1.0f);


        if (bombDropRate > 0) {
            bombTimer += Time.deltaTime;

            if (bombTimer > bombDropRate) {
                DropBomb();
                bombTimer = 0;
            }
        }

    }

    private void FixedUpdate() {
        //limit velocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, jumpForce / 50);
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player.isDashing) {
                Die();
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {

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

        audioSource.clip = deathSound;
        audioSource.Play();

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;

        bombDropRate = 0;

        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);

        Destroy(gameObject, 3.0f);
    }


    void DropBomb() {
        Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), transform.rotation);
    }


}
