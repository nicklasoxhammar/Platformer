using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class OnOffSwitchController : MonoBehaviour {

    [SerializeField] StoneController[] stoneElevators;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    [SerializeField] bool switchStatus = false;
    [SerializeField] bool lockSwitchWhenTurnedOn = false;


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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && CrossPlatformInputManager.GetButtonDown("Dash"))
        {
            TurnSwitch();
        }
    }

    private void TurnSwitch()
    {
        if (lockSwitchWhenTurnedOn && switchStatus) { return; }
            switchStatus = !switchStatus;
            changeSprite();

            foreach (StoneController stone in stoneElevators)
            {
                stone.SetElevatorStatusTo(switchStatus);
            }
    }
}
