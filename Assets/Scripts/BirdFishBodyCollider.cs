using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFishBodyCollider : MonoBehaviour
{

    BirdFishController birdFishController;
    // Use this for initialization
    void Start()
    {
        birdFishController = GetComponentInParent<BirdFishController>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.isDashing && !birdFishController.GetIsDead())
            {
                //ignore player layer...
                gameObject.layer = 11;
                birdFishController.Die();
            }
        }
        else if (collision.gameObject.tag == "KillsEnemy" && !birdFishController.GetIsDead())
        {
            //ignore player layer...
            gameObject.layer = 11;
            birdFishController.Die();
        }
    }
}
