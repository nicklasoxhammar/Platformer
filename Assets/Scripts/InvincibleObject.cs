using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleObject : MonoBehaviour {

    [SerializeField] private int invincibleTime = 5;
    private bool isUsed = false;

    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }


    public int GetInvincibleTime()
    {
        if(isUsed)
        {
            invincibleTime = 0;
        }
        isUsed = true;
        return invincibleTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            audioSource.Play();
        }
    }
}
