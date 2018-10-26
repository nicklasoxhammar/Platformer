using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] Sprite doneFlower;

    GameObject LevelButtonsUI;
    GameObject StartingScreenUI;
    GameObject ScrollingCanvas;

    private void Awake() {
        ScrollingCanvas = GameObject.Find("Scrolling Canvas");
        LevelButtonsUI = GameObject.Find("Level Buttons UI");
    }

    void Start() {

        SetUpButtons();
    }

    public void LoadScene() {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int sceneToLoad = int.Parse(clickedButton.GetComponentInChildren<Text>().text);

        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadIntro() {
        SceneManager.LoadScene("Intro");
    }

    //Called when "Play" button is pressed
    public void SetUpButtons() {

        //If this is the first time the game is launched - load the intro scene.
        if(PlayerPrefs.GetInt("First Launch", 0) == 0) {
            PlayerPrefs.SetInt("First Launch", 1);
            LoadIntro();
            return;
        }

        GetComponent<SceneHandler>().fadeIn = true;
        ScrollingCanvas.SetActive(true);
        Camera.main.backgroundColor = Color.black;

        //get how many levels the player has completed
        int progress = PlayerPrefs.GetInt("Progress", 1);

        //show how many levels there are and unlock completed level buttons
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings - 2; i++) {
            LevelButtonsUI.transform.GetChild(i - 1).gameObject.SetActive(true);

            if (i <= progress) {
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

    }


}
