using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSwitchController : MonoBehaviour {

    [SerializeField] StoneController[] stoneElevators;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    [SerializeField] bool switchStatus = false;

    private SpriteRenderer myRenderer;

	// Use this for initialization
	void Start () {

        myRenderer = GetComponent<SpriteRenderer>();

        changeSprite();

	}
	

    private void changeSprite()
    {
        if (switchStatus)
        {
            myRenderer.sprite = onSprite;
        }
        else
        {
            myRenderer.sprite = offSprite;
        }
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            TurnSwitch();
        }
    }





    private void TurnSwitch()
    {
        switchStatus = !switchStatus;
        changeSprite();

        foreach (StoneController stone in stoneElevators)
        {
            stone.SetElevatorStatusTo(switchStatus);
        }
    }




}
