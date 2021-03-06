﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DelayLetters : MonoBehaviour
{

    private Text text;
    private Color color;
    private IntroScene introScene;
    private float fadeOutTime = 0.6f;


    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        color = text.color;
        introScene = FindObjectOfType<IntroScene>();
    }


    public void SetTextTo(string newText, float timeBetweenLetters, float secToKeepText, bool fadeOut)
    {
        text.text = "";
        StartCoroutine(DelayPrintFade(newText, timeBetweenLetters, secToKeepText, fadeOut));
    }


    IEnumerator DelayPrintFade(string newText, float timeBetweenLetters, float secToKeepText, bool fadeOut)
    {
        for (int i = 0; i < newText.Length; i++)
        {
            yield return new WaitForSeconds(timeBetweenLetters);
            if (newText[i].Equals('@'))
            {
                text.text += System.Environment.NewLine;
            }
            else
            {
                text.text += newText[i];
            }
        }
        FadeOut(secToKeepText, fadeOut);
    }

    private void FadeOut(float secToKeepText, bool fadeOut)
    {
        StartCoroutine(FadeOutDelay(secToKeepText, fadeOut));
    }
    IEnumerator FadeOutDelay(float secToKeepText, bool fadeOut)
    {
        yield return new WaitForSeconds(secToKeepText);
        //Tell the text is done...
        introScene.SetIsPrintingTo(false);
        if (fadeOut)
        {
            LeanTween.alphaText(GetComponent<RectTransform>(), 0, fadeOutTime).setOnComplete(() =>
            {
                text.text = "";
                text.color = color;
            });
        }
    }

    public void ResetText()
    {
        text.text = "";
    }
}
