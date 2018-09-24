using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField] float time = 5.0f;
    [SerializeField] GameObject explosionParticles;

    private bool blownUp = false;

    void Update() {

        if (blownUp) { return; }

        time -= Time.deltaTime;

        if (time <= 0) {
            Explode();
        }

        StartCoroutine(Spin());

    }

    IEnumerator Spin() {

        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < time) {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / time);
            transform.eulerAngles = new Vector3(0, 0, zRotation);
            yield return null;
        }
    }

    void Explode() {
        blownUp = true;
        GameObject particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerStay2D(Collider2D collision) {

        if (collision.gameObject.tag == "Player" && blownUp) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.Die();
            player.rb.AddForceAtPosition(Vector2.up * 100, transform.position, ForceMode2D.Force);

        }
    }
}
