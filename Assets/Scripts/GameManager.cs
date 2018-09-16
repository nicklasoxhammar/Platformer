using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject levelCompleteScreen;
    public PlayerController player;
    public int flowerCounter;
    public GameObject dashBar;

    void Update() {

        HandleDashBar();
        
    }

    public void LevelComplete() {
        levelCompleteScreen.SetActive(true);
    }

    public void pickedFlower() {
        flowerCounter--;
        Debug.Log("FLOWER PICKED");

        if (flowerCounter <= 0) {
            //ALLA PLOCKADE.
            Debug.Log("ALLA PLOCKADE");
        }
    }



    public void AddFlower() {
        flowerCounter++;
    }

    private void HandleDashBar() {

        //Scale dashbar with current dashtime
        dashBar.transform.localScale = new Vector3(player.dashTime / player.startDashTime, 1, 1);

        //Just here so the dashbar doesnt scale below zero
        if(dashBar.transform.localScale.x < 0) {
            dashBar.transform.localScale = new Vector3(0, 1, 1);
        }
 
        if (player.canDash) {
            dashBar.GetComponent<Image>().color = Color.blue;
        }
        else {
            dashBar.GetComponent<Image>().color = Color.gray;
        }

    }
}
