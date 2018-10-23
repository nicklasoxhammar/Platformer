using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {

    private GameManager gameManager;

    private Animator animator;
    private bool picked = false;

    [SerializeField] AudioClip flowerSound;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        gameManager.AddFlower();

        animator = GetComponent<Animator>();
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !picked)
        {
            audioSource.clip = flowerSound;
            audioSource.Play();
            picked = true;
            gameManager.pickedFlower(this);
            animator.SetBool("run", true);
        }
    }

}
