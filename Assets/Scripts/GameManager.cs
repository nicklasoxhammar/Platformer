using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    GameObject levelCompleteScreen;
    GameObject deathScreen;
    [SerializeField] AudioClip levelCompleteSound;
    Text flowerCounterText;

    [HideInInspector] public GameObject dashBar;

    private Color startDashButtonColor;
    private Color startDashBarColor;
    private GameObject dashButton;
    private int flowersTotal = 0;
    private int pickedFlowers = 0;
    PlayerController player;

    AudioSource audioSource;

    private void Awake() {
        player = FindObjectOfType<PlayerController>();
#if (UNITY_IOS || UNITY_ANDROID)
        dashButton = GameObject.Find("Dash Button");
        startDashButtonColor = dashButton.GetComponent<Image>().color;
#endif
        audioSource = GetComponent<AudioSource>();

        levelCompleteScreen = GameObject.Find("Level Complete Screen");
        levelCompleteScreen.SetActive(false);

        deathScreen = GameObject.Find("Death Screen");
        deathScreen.SetActive(false);


        flowerCounterText = GameObject.Find("Flower Counter Text").GetComponent<Text>();

        dashBar = GameObject.Find("Dash Bar Meter");
        startDashBarColor = dashBar.GetComponent<Image>().color;

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
        SetPlayerPrefs();
    }

    public void PlayerDied() {
        deathScreen.SetActive(true);
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
            dashBar.GetComponent<Image>().color = startDashBarColor;
        }
        else {
            Color gray = Color.gray;
            gray.a = 0.5f;
            dashBar.GetComponent<Image>().color = gray;
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

    void SetPlayerPrefs() {
        int progress = PlayerPrefs.GetInt("Progress", 1);
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        int previousScore = PlayerPrefs.GetInt("Level " + currentLevel + " flowers picked", 0);

        if (currentLevel >= progress) {
            PlayerPrefs.SetInt("Progress", currentLevel + 1);
        }

        if (pickedFlowers > previousScore) {
            PlayerPrefs.SetInt("Level " + currentLevel + " flowers picked", pickedFlowers);
        }

        PlayerPrefs.SetInt("Level " + currentLevel + " total flowers", flowersTotal);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void NextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TryAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

