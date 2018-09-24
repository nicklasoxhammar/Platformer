using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField] float time = 5.0f;
    [SerializeField] GameObject explosionParticles;

    private bool blownUp = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (blownUp) { return; }

        time -= Time.deltaTime;

        if(time <= 0) {
            Explode();
        }
		
	}

    void Explode() {
        blownUp = true;
        GameObject particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);

        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player" && blownUp) {
            Debug.Log("You dead boi");
            collision.gameObject.GetComponent<PlayerController>().Die();
        }
    }
}
