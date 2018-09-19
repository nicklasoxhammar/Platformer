using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Box : MonoBehaviour {

    PlayerController player;
    bool beingCarried = false;
    bool canPickUp = true;
    bool canDrop = false;

    SpriteRenderer sprite;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update() {

        if (beingCarried) {
            MoveWithPlayer();
        }

    }



    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.isCarryingBox = true;

            if (CrossPlatformInputManager.GetButtonDown("Dash") && canPickUp) {
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

        if (!beingCarried) {
            player.isCarryingBox = false;
        }

    }


    void MoveWithPlayer() {

        if (CrossPlatformInputManager.GetButtonUp("Dash") && !canDrop) {
            canDrop = true;
            return;
        }

        if (CrossPlatformInputManager.GetButtonUp("Dash") && canDrop) {
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
