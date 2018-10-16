﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DelayLetters : MonoBehaviour
{

    private Text text;
    private Color color;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        color = text.color;
    }


    public void SetTextTo(string newText, float timeBetweenLetters, float secToKeepText, bool fadeOut)
    {
        text.text = "";
        StartCoroutine(DelayPrintFade(newText, timeBetweenLetters * Time.deltaTime, secToKeepText, fadeOut));
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
        if (fadeOut)
        {
            LeanTween.alphaText(GetComponent<RectTransform>(), 0, 1f).setOnComplete(() =>
            {
                text.text = "";
                text.color = color;
            });
        }
    }

    public float GetTimeForPrintingText(string printString, float timeBetweenLetters, float secToKeepText)
    {
        return printString.Length * (timeBetweenLetters * Time.deltaTime) + secToKeepText;
    }

    public void ResetText()
    {
        text.text = "";
    }

}
