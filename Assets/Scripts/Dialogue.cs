using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Dialogue : MonoBehaviour {

    [SerializeField] [TextArea] string dialogue;
    [SerializeField] float time = 2.0f;
    [SerializeField] bool unlimitedTriggers = false;

    Text text;
    bool triggered = false;
    bool fade = false;
    float progress = 0.0f;

    Color textColor;

    void Start() {
        text = GameObject.Find("Dialogue Text").GetComponent<Text>();
        textColor = text.color;
    }

    private void Update() {

        if (fade && text.text == dialogue) {
            progress += Time.deltaTime;
            Color color = text.color;
            color.a = Mathf.Lerp(1, 0, progress);
            text.color = color;

            if (progress >= 1) {
                text.text = "";
                fade = false;
                text.color = textColor;
            }
        }

        if (unlimitedTriggers && text.text != dialogue) { triggered = false; }

    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag == "Player" && !triggered) {
            transform.parent = null; //here so that the dialogue doesnt get destroyed if the parent gets destroyed.
            CancelInvoke();
            fade = false;
            triggered = true;
            text.text = dialogue;
            text.color = textColor;

            Invoke("RemoveDialogue", time);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "Player" && !triggered) {
            transform.parent = null; //here so that the dialogue doesnt get destroyed if the parent gets destroyed.
            CancelInvoke();
            fade = false;
            triggered = true;
            text.text = dialogue;
            text.color = textColor;

            Invoke("RemoveDialogue", time);
        }
    }

    void RemoveDialogue() {
        progress = 0;
        fade = true;
    }

}
