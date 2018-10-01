using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Box : MonoBehaviour {

    [SerializeField] AudioClip pickUpSound;
    [SerializeField] AudioClip dropSound;

    PlayerController player;
    bool beingCarried = false;
    bool canPickUp = true;
    bool canDrop = false;

    GameManager GM;
    SpriteRenderer sprite;
    AudioSource audioSource;

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {

        if (beingCarried) {
            player.isCarryingBox = true;
            MoveWithPlayer();
        }

    }



    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            player = collision.gameObject.GetComponent<PlayerController>();
            if (player.isCarryingBox) { return; }

            if (CrossPlatformInputManager.GetButtonDown("Dash") && canPickUp) {
                player.isCarryingBox = true;
                audioSource.clip = pickUpSound;
                audioSource.Play();
                Color newAlpha = sprite.color;
                newAlpha.a = 0.5f;
                sprite.color = newAlpha;
                beingCarried = true;
                canPickUp = false;
                canDrop = false;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision) {

        if (!beingCarried && player != null) {
            player.isCarryingBox = false;
        }

    }


    void MoveWithPlayer() {

        if (CrossPlatformInputManager.GetButtonUp("Dash") && !canDrop) {
            canDrop = true;
            return;
        }

        if (CrossPlatformInputManager.GetButtonUp("Dash") && canDrop) {
            audioSource.clip = dropSound;
            audioSource.Play();
            beingCarried = false;
            canPickUp = true;
            player.isCarryingBox = false;
            Color newAlpha = sprite.color;
            newAlpha.a = 1.0f;
            sprite.color = newAlpha;
        }

        float xPos = player.transform.position.x + 2f;
        if (player.direction == -1.0f) {
            xPos = player.transform.position.x - 2f;
        }

        transform.position = new Vector3(xPos, player.transform.position.y + 1.0f, transform.position.z);
    }




}
