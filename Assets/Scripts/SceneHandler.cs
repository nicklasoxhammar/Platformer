﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    Image fadeImage;
    bool fadeIn = true;
    bool fadeOut = false;
    float fadeTime = 0.5f;
    Color color;
    float progress = 0;


    void Awake() {
        fadeImage = GameObject.Find("Fade Image").GetComponent<Image>();
        color = fadeImage.color;

    }
	
	void Update () {

        if (fadeIn) {
            progress += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, progress / fadeTime);
            fadeImage.color = color;

            if (progress / fadeTime >= 1) {
                fadeIn = false;
                fadeImage.gameObject.SetActive(false);
                progress = 0;
            }
        }


        if (fadeOut) {
            progress += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, progress / fadeTime);
            fadeImage.color = color;
        }



    }

    IEnumerator FadeOut(int sceneIndex) {
        fadeImage.gameObject.SetActive(true);
        fadeOut = true;
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneIndex);
    }

    public void MainMenu() {
        StartCoroutine(FadeOut(0));
    }

    public void NextLevel() {
        StartCoroutine(FadeOut(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void TryAgain() {
        StartCoroutine(FadeOut(SceneManager.GetActiveScene().buildIndex));
    }
}