using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevelSkateboard : MonoBehaviour {

    private GameObject player = null;
    private Vector3 offset;

    [SerializeField] GameObject levelCompleteParticles;
    GameManager GM;

    private void Start() {
        GM = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    void OnCollisionEnter2D(Collision2D col) {

        if (col.gameObject.tag == "Player") {
            player = col.gameObject;

            //Make player a child of the skateboard, so they move together
            player.transform.parent = transform;
            //center the player on top of the skateboard
            player.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            CompleteLevel();
        }
    }

    void CompleteLevel() {
       
        player.GetComponent<PlayerController>().freezeMovement = true;
        Destroy(player.GetComponent<Rigidbody2D>());

        StartCoroutine(MoveAround());
    }

    IEnumerator MoveAround() {

        float duration = 2.0f;

        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration) {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(0, 0, zRotation);
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
            yield return null;
        }

        GameObject particles = Instantiate(levelCompleteParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);

        Destroy(gameObject);

        GM.LevelComplete();
    }
   
    
}
