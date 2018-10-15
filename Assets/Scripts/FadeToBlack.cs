using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour {


    public void Fade(float seconds)
    {
        LeanTween.color(gameObject, Color.black, seconds);
    }
}
