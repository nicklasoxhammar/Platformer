using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen : MonoBehaviour {

    public GameObject blackSprite;

    private void Start() {
        Invoke("GoToMainMenu", 10.0f);
    }

    public void GoToMainMenu()
    {
        blackSprite.SetActive(true);
        LeanTween.alpha(blackSprite, 1, 0.5f).setOnComplete(() => 
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 2);
        });
        }
}
