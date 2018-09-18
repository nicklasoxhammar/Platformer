using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    SpriteRenderer sprite;


    float oldAlpha;
    float newAlpha;
    float progress = 0;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();

        oldAlpha = sprite.color.a;
        newAlpha = Random.Range(0.3f, 0.5f);
    }

    void Update() {

        //Lerp between different alphas
        progress += Time.deltaTime;
        Color color = sprite.color;
        color.a = Mathf.Lerp(oldAlpha, newAlpha, progress);
        sprite.color = color;

        if (progress > 1) {
            oldAlpha = sprite.color.a;
            newAlpha = Random.Range(0.3f, 0.5f);
            progress = 0;
        }


    }



}
