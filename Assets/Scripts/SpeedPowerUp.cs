using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float time;

    private float originalSpeed;
    private float timer = 0;
    private bool activated = false;
    PlayerController player;

    void Start() {
        StartCoroutine(Spin());
    }

    void Update() {
        if (activated) {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, playerPos.y + 4.0f);
            timer += Time.deltaTime;

            if (timer >= time) {
                Done();
            }
        }
    }

    IEnumerator Spin() {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < 1.0f) {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / 1.0f);
            transform.eulerAngles = new Vector3(0, yRotation, 0);
            yield return null;
        }
        StartCoroutine(Spin());
    }

    void Done() {
        activated = false;
        player.speed = originalSpeed;
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && !activated) {
            player = collision.gameObject.GetComponent<PlayerController>();
            originalSpeed = player.speed;
            player.speed = speed;
            activated = true;
        }

    }
}
