using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DelayLetters : MonoBehaviour
{

    private Text text;
    public bool isPrinting = false;
    private Color color;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        color = text.color;
    }


    public void SetTextTo(string newText, float speed, float secToKeepText, bool fadeOut)
    {
        text.text = "";
        //isPrinting = true;
        StartCoroutine(DelayPrintFade(newText, speed, secToKeepText, fadeOut));
    }


    IEnumerator DelayPrintFade(string newText, float speed, float secToKeepText, bool fadeOut)
    {
        for (int i = 0; i < newText.Length; i++)
        {
            yield return new WaitForSeconds(speed);
            text.text += newText[i];
        }
        //isPrinting = false;
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
        else
        {
            text.text = "";
        }


    }

}
