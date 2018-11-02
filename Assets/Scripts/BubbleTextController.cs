using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleTextController : MonoBehaviour {

    private RectTransform rectTransform;
    public DelayLetters text;
    public Transform followObject;
    [SerializeField] Vector3 offset;
	// Use this for initialization
	void Start () {
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

    public void PrintText(string printString, float timeBetweenLetters, float secToKeepText, bool fadeOut)
    {
        text.SetTextTo(printString, timeBetweenLetters, secToKeepText, fadeOut);
    }

    public void ShowBubbleAndPrintText(string printString, float timeBetweenLetters, float secToKeepText, bool fadeOut)
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 1f).setEaseOutBounce().setOnComplete(() => 
        {
            PrintText(printString, timeBetweenLetters, secToKeepText, fadeOut);
        });
    }

    public void SetOffsetTo(Vector3 vector3)
    {
        offset = vector3;
    }

}
