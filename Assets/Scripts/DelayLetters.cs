using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DelayLetters : MonoBehaviour {

    private Text text;
    public bool isPrinting = false;
    private Color color;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        color = text.color;
	}
	

    public void SetTextTo(string newText, float speed, float secToKeepTextBeforeFadeOut)
    {
        text.text = "";
        isPrinting = true;
        StartCoroutine(DelayPrintFade(newText, speed, secToKeepTextBeforeFadeOut));
    }

    //public bool GetIsPrinting()
    //{
    //    return isPrinting;
    //}

    IEnumerator DelayPrintFade(string newText, float speed, float secToKeepTextBeforeFadeOut)
    {
        for (int i = 0; i < newText.Length; i++)
        {
            yield return new WaitForSeconds(speed);
            text.text += newText[i];
        }
        isPrinting = false;
        FadeOut(secToKeepTextBeforeFadeOut);
    }

    private void FadeOut(float seconds)
    {
        StartCoroutine(FadeOutDelay(seconds));
    }
    IEnumerator FadeOutDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(!isPrinting){
            LeanTween.alphaText(GetComponent<RectTransform>(), 0, 1f).setOnComplete(() =>
            {
                text.text = "";
                text.color = color;
            });
        }

    }

}
