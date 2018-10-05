using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartingScene : MonoBehaviour {

    [SerializeField] Sprite doneFlower;

    GameObject LevelButtonsUI;
    GameObject StartingScreenUI;

    private void Awake() {
        LevelButtonsUI = GameObject.Find("Level Buttons UI");
        StartingScreenUI = GameObject.Find("Starting Screen UI");
    }

    void Start() {

        if (!StartingScreen.started) {
            StartingScreen.started = true;
            ShowStartingScreen();
        }
        else {
            SetUpButtons();
        }
    }

    public void LoadScene() {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int sceneToLoad = int.Parse(clickedButton.GetComponentInChildren<Text>().text);

        SceneManager.LoadScene(sceneToLoad);
    }

    public void SetUpButtons() {
        StartingScreenUI.SetActive(false);
        LevelButtonsUI.SetActive(true);
        Camera.main.backgroundColor = Color.black;

        //get how many levels the player has completed
        int progress = PlayerPrefs.GetInt("Progress", 1);

        //unlock completed level buttons
        for (int i = 1; i < progress + 1; i++) {
            int challengesCompleted = 0;
            LevelButtonsUI.transform.GetChild(i - 1).GetComponent<Button>().interactable = true;

            challengesCompleted += PlayerPrefs.GetInt("Level " + i + " challenge one", 0);
            challengesCompleted += PlayerPrefs.GetInt("Level " + i + " challenge two", 0);
            challengesCompleted += PlayerPrefs.GetInt("Level " + i + " challenge three", 0);

            for (int y = 0; y < challengesCompleted; y++) {
                LevelButtonsUI.transform.GetChild(i - 1).GetChild(y).GetComponent<Image>().sprite = doneFlower;
            }

        }


    }


    void ShowStartingScreen() {

        LevelButtonsUI.SetActive(false);

        Debug.Log("REACH!");

    }


}
