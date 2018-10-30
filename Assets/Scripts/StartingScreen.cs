using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartingScreen : MonoBehaviour {

    public Image blackSprite;
    private bool fadeOut = false;
    float progress = 0;
    float fadeTime = 0.5f;
    Color color;

    private void Start() {
        Invoke("GoToMainMenu", 10.0f);
        color = blackSprite.color;
    }

    private void Update()
    {
        if (fadeOut)
        {
            progress += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, progress / fadeTime);
            blackSprite.color = color;
        }
        if (progress / fadeTime >= 1)
        {
            fadeOut = false;
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 2);
        }
    }

    public void GoToMainMenu()
    {
        fadeOut = true;
    }
}
