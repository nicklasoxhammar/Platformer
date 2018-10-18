using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDashButton : MonoBehaviour {

    GameManager GM;
    bool triggered = false;
    bool alreadyExit = false;

	void Start () {

        GM = FindObjectOfType<GameManager>();
		
	}
	
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && !triggered) {
            StopAllCoroutines();
            GM.dashButtonYellow = true;
            triggered = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && !triggered) {
            StopAllCoroutines();
            GM.dashButtonYellow = true;
            triggered = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && !alreadyExit) {
            Invoke("StopFlashing", 2.0f);
            alreadyExit = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && !alreadyExit) {
            Invoke("StopFlashing", 2.0f);
            alreadyExit = true;
        }
    }


    void StopFlashing() {
        GM.dashButtonYellow = false;
    }

}
