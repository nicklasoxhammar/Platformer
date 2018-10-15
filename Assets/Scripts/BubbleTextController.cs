using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleTextController : MonoBehaviour {

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public DelayLetters text;
    public Transform followObject;
    [SerializeField] Vector3 offset;
	// Use this for initialization
	void Start () {
        canvasGroup = GetComponent<CanvasGroup>();
	}

    private void Update()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(followObject.position - offset);
        transform.position = position;
    }

    public void Hide()
    {
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 1f).setEaseInBack();
    }

    public void PrintText(string printString, float speed, float secToKeepTextBeforeFadeOut)
    {
        text.SetTextTo(printString, speed, secToKeepTextBeforeFadeOut);
    }

    public void ShowAndPrintText(string printString, float speed, float secToKeepTextBeforeFadeOut)
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 1f).setEaseOutBounce().setOnComplete(() => 
        {
            PrintText(printString, speed, secToKeepTextBeforeFadeOut);
        });
    }

    public float GetTimeForPrintingText(string printString, float speed)
    {
        return printString.Length * speed;
    }
}
