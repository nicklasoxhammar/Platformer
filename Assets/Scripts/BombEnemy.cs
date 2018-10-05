using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : MonoBehaviour {

    Rigidbody2D rb;
    AudioSource audioSource;
    Animator animator;


    [SerializeField] float speed = 5.0f;
    [SerializeField] float bombDropRate = 5.0f;
    [SerializeField] float bombTime = 3.0f;
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject deathParticles;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip burpSound;

    float direction = 1;

    float startingXPos;
    float startingYPos;

    private float rotationSpeed = 5.0f;

    float bombTimer = 0.0f;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        startingXPos = transform.position.x;
        startingYPos = transform.position.y;

    }

    void Update() {
        Move();


        if (bombDropRate > 0) {
            bombTimer += Time.deltaTime;

            if (bombTimer > bombDropRate) {
                StartCoroutine(DropBomb());
                bombTimer = 0;
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player.isDashing) {
                Die();
            }
            else {
                player.Die();
            }
        }
    }

    void Die() {

        audioSource.clip = deathSound;
        audioSource.Play();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;

        bombDropRate = 0;

        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);

        Destroy(gameObject, 3.0f);
    }

    void Move() {

        //Right
        if (direction == 1) {
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);

        }
        //Left
        else if (direction == -1) {
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
  
        }

        transform.Rotate(0, 0, rotationSpeed * direction * -1);

    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Block") {
            direction *= -1;
        }
    }


    IEnumerator DropBomb() {
        audioSource.clip = burpSound;
        audioSource.Play();
        animator.SetTrigger("DropBombTrigger");

        yield return new WaitForSeconds(0.25f);
        GameObject bombObject = Instantiate(bomb, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        bombObject.GetComponent<Bomb>().time = bombTime;
    }

}
