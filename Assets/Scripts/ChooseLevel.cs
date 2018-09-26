using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour {

    [SerializeField] Sprite doneFlower;
    [SerializeField] Sprite halfDoneFlower;
    [SerializeField] Sprite wiltedFlower;

    void Start() {

        //get how many levels the player has completed
        int progress = PlayerPrefs.GetInt("Progress", 1);

        //unlock completed level buttons
        for (int i = 1; i < progress + 1; i++) {
            transform.GetChild(i - 1).GetComponent<Button>().interactable = true;
            Image flowerImage = transform.GetChild(i - 1).GetChild(0).GetComponent<Image>();
            int flowersPicked = PlayerPrefs.GetInt("Level " + i + " flowers picked", 0);
            int totalFlowers = PlayerPrefs.GetInt("Level " + i + " total flowers", 1);

            //Set flower image sprite depending on how many flowers the player picked up
            if (flowersPicked < totalFlowers / 2 || flowersPicked == 0) {
                flowerImage.sprite = wiltedFlower;
            }
            else if (flowersPicked < totalFlowers) {
                flowerImage.sprite = halfDoneFlower;
            }
            else {
                flowerImage.sprite = doneFlower;
            }
        }
    }

    public void LoadScene() {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int sceneToLoad = int.Parse(clickedButton.GetComponentInChildren<Text>().text);

        SceneManager.LoadScene(sceneToLoad);
    }


}
