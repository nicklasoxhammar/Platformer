using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {

    private GameManager gameManager;



	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddFlower();
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gameManager.pickedFlower();
            Destroy(gameObject);
        }
    }

}
