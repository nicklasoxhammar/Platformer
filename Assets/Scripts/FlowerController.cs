using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {

    private GameManager gameManager;

    private Animator animator;
    private bool picked = false;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddFlower();

        animator = GetComponent<Animator>();
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !picked)
        {
            picked = true;
            gameManager.pickedFlower();
            animator.SetBool("run", true);
        }
    }

}
