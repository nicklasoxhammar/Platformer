using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    GameObject levelCompleteScreen;
    [SerializeField] AudioClip levelCompleteSound;
    Text flowerCounterText;

    [HideInInspector] public GameObject dashBar;

    private Color startDashButtonColor;
    private GameObject dashButton;
    private int flowersTotal = 0;
    private int pickedFlowers = 0;
    PlayerController player;

    AudioSource audioSource;

    private void Awake() {
        player = FindObjectOfType<PlayerController>();
#if (UNITY_IOS || UNITY_ANDROID)
            dashButton = GameObject.Find("DashButton");
            startDashButtonColor = dashButton.GetComponent<Image>().color; 
#endif

        audioSource = GetComponent<AudioSource>();

        levelCompleteScreen = GameObject.Find("Level Complete Screen");
        levelCompleteScreen.SetActive(false);
        flowerCounterText = GameObject.Find("Flower Counter Text").GetComponent<Text>();
        dashBar = GameObject.Find("Dash Bar Meter");

    }


    void Update() {
        HandleDashBar();
        SetDashButtonColor();
    }

    public void LevelComplete() {
        audioSource.clip = levelCompleteSound;
        audioSource.Play();
        levelCompleteScreen.SetActive(true);
        GameObject.Find("Mobile Input UI").SetActive(false);
        GameObject.Find("Dash Bar").SetActive(false);
    }

    public void pickedFlower() {
        pickedFlowers++;
        flowerCounterText.text = pickedFlowers + "/" + flowersTotal;

        if (pickedFlowers == flowersTotal) {
            //ALLA PLOCKADE.
            Debug.Log("ALLA PLOCKADE");
        }
    }



    public void AddFlower() {
        flowersTotal++;
        flowerCounterText.text = pickedFlowers + "/" + flowersTotal;
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

