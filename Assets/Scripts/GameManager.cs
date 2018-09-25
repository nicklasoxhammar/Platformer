using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    [SerializeField] GameObject levelCompleteScreen;
    [SerializeField] AudioClip levelCompleteSound;

    public PlayerController player;
    public int flowerCounter;
    public GameObject dashBar;

    private Color startDashButtonColor;
    private GameObject dashButton;

    AudioSource audioSource;

    private void Start() {
#if (UNITY_IOS || UNITY_ANDROID)
                dashButton = GameObject.Find("DashButton");
                startDashButtonColor = dashButton.GetComponent<Image>().color; 
#endif

        audioSource = GetComponent<AudioSource>();
    }


    void Update() {
        HandleDashBar();
        SetDashButtonColor();
    }

    public void LevelComplete() {
        audioSource.clip = levelCompleteSound;
        audioSource.Play();
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
        if (dashBar.transform.localScale.x < 0) {
            dashBar.transform.localScale = new Vector3(0, 1, 1);
        }

        if (player.canDash) {
            dashBar.GetComponent<Image>().color = Color.blue;
        }
        else {
            dashBar.GetComponent<Image>().color = Color.gray;
        }

    }

    public void SetDashButtonColor() {


        if (dashButton == null) { return; }

        if (player.isCarryingBox) {
            Color yellow = Color.yellow;
            yellow.a = 0.5f;
            dashButton.GetComponent<Image>().color = yellow;
        }
        else {
            dashButton.GetComponent<Image>().color = startDashButtonColor;
        }


    }
}

