using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    //test
    Color[] colors = { Color.green, Color.blue, Color.red, Color.yellow, Color.magenta, Color.cyan };
    GameObject particles;

    void Start() {

        particles = transform.Find("Particle System").gameObject;

    }

    void OnCollisionEnter2D(Collision2D collision) {

        if(collision.gameObject.tag == "Player") {

            SpriteRenderer sprite = transform.parent.GetComponent<SpriteRenderer>();

            Color previousColor = sprite.color;

            while (true) {
                sprite.color = colors[Random.Range(0, colors.Length)];

                if(sprite.color != previousColor) {
                    break;
                }
            }

            particles.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision) {

        if (collision.gameObject.tag == "Player") {

            particles.SetActive(false);

        }

    }
}
