using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Enemy : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField] float speed = 5.0f;
    //[SerializeField] float jumpHeight = 3.0f;
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

        /*if (jumpHeight > 0) {
            StartCoroutine(Jump());
        }*/
	}

	void Update () {

        Vector3 position = transform.position;
        position.x += speed * direction * Time.deltaTime;
        skeletonAnimation.AnimationName = "RUN";

        transform.position = position;


        if (position.y < startingYPos + 0.2f && position.x > startingXPos + distance){
            direction = -1;
        }

        if (position.y < startingYPos + 0.2f && position.x < startingXPos - distance) {
            direction = 1;
        }

        transform.localScale = new Vector3(direction, 1.0f, 1.0f);



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

    /*IEnumerator Jump() {


        float duration = 0.5f;

        while (true) {
            float startPos = transform.position.y;
            float endPos;

            if (startPos < startingYPos + jumpHeight) {
                endPos = startPos + jumpHeight;
            }
            else {
                endPos = startingYPos;
            }
            float t = 0.0f;
            while (t < duration) {
                t += Time.deltaTime;
                float yPos = Mathf.Lerp(startPos, endPos, t / duration);
                transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
                yield return null;
            }
        }

         while (true) {

            transform.position = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);

            yield return new WaitForSeconds(0.5f);

            transform.position = new Vector3(transform.position.x, transform.position.y - jumpHeight, transform.position.z);

            yield return new WaitForSeconds(0.5f);

        }
       

    }*/


}
