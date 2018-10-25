using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SharkEnemyController : MonoBehaviour {

    [SerializeField] Transform player;
    [SerializeField] GameObject deathParticles;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float followDistance = 10f;
    [SerializeField] AudioClip biteSound;
    [SerializeField] AudioClip deathSound;

    private int direction = 1;
    private float chewingDistance = 10.0f;
    private bool followPlayer = false;
    private bool chewing = false;

    SkeletonAnimation skeletonAnimation;
    AudioSource audioSource;

    //Flying animations... Använd en i taget av flying.
    private string flying1AnimationName = "Flying1";
    private string flying2AnimationName = "Flying2";
    private string glidInAirAnimationName = "Glidning";
    private string SetWingsDownAnimationName = "WingsDowm";
    private string SetWingsUpAnimationName = "WingsUp";

    //Mouth...använd en i taget.
    private string chewingAnimationName = "Chewing";
    private string closeMouthAnimationName = "CloseMouth";
    private string openMouthAnimationName = "OpenMouth";

    //Other
    private string sniffAnimationName = "Sniff";
    private string swimAnimationName = "Swim";


    //Annars kan alla kombineras och köras samtidigt.

    void Start() {
        audioSource = GetComponent<AudioSource>();

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(0, flying2AnimationName, true);
        skeletonAnimation.AnimationState.SetAnimation(1, sniffAnimationName, true);
        skeletonAnimation.AnimationState.SetAnimation(4, swimAnimationName, true);
        skeletonAnimation.AnimationState.SetAnimation(3, closeMouthAnimationName, false);
    }

    void Update() {
        CheckDistanceToPlayer();
        SetDirectionToFollowPlayer();

        if (followPlayer) {

            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (direction == 1) {
                transform.right = player.position - transform.position;
            }
            else {
                transform.right = -(player.position - transform.position);
            }
        }


        transform.localScale = new Vector3(direction * -1, 1f, 1f);
    }



    void CheckDistanceToPlayer() {

        if (transform.position.x - player.position.x < chewingDistance && transform.position.x - player.position.x > -chewingDistance) {
            if (!chewing) {
                skeletonAnimation.AnimationState.SetAnimation(3, chewingAnimationName, true);
                chewing = true;
            }
        }
        else {
            if (chewing) {
                skeletonAnimation.AnimationState.SetAnimation(3, closeMouthAnimationName, false);
                chewing = false;
            }
        }


        if (followPlayer) { return; }

        if (transform.position.x - player.position.x < followDistance && transform.position.x - player.position.x > -followDistance) { followPlayer = true; }
    }

    //Called from Update
    private void SetDirectionToFollowPlayer() {
        //Direction follow the player...
        float offset = 3f;

        if (player.position.x < transform.position.x - offset) {
            direction = -1;
        }
        else if (player.position.x > transform.position.x + offset) {
            direction = 1;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.isDashing) {
                Die();
            }
            else {
                audioSource.clip = biteSound;
                audioSource.Play();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.isDashing)
            {
                Die();
            }
        }
    }



    private void Die() {
        audioSource.clip = deathSound;
        audioSource.Play();

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);
        Destroy(this.gameObject, 3.0f);
    }



}
