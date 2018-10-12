using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float time = 5.0f;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] AudioClip tickSound;
    [SerializeField] AudioClip explosionSound;

    AudioSource audioSource;

    private bool blownUp = false;

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Tick());

    }

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

    IEnumerator Tick() {

        audioSource.clip = tickSound;
        audioSource.Play();

        yield return new WaitForSeconds(time / 5);

        if (time > 0) {
            StartCoroutine(Tick());
        }
    }



    void Explode() {
        audioSource.clip = explosionSound;
        audioSource.Play();
        blownUp = true;
        GameObject particles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(particles, 3.0f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponentInChildren<ParticleSystem>());
        StartCoroutine(Done());
    }

    private void OnTriggerStay2D(Collider2D collision) {

        if (collision.gameObject.tag == "Player" && blownUp) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null && !player.IsWearingShield()) {
                player.rb.AddForceAtPosition(Vector2.up * 100, transform.position, ForceMode2D.Force);
            }
        }
    }

    IEnumerator Done() {

        yield return new WaitForSeconds(0.2f);

        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        yield return new WaitForSeconds(3.0f);

        Destroy(gameObject);


    }
}
