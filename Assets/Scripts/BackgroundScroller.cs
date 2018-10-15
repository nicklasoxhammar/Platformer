using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    [SerializeField] float backgroundScrollSpeed = 0.2f;
    [SerializeField] bool scrollX = false;
    Material myMaterial;
    Vector2 offset;

    // Use this for initialization
    void Start () {
        myMaterial = GetComponent<Renderer>().material;

        if(scrollX)
        {
            offset = new Vector2(backgroundScrollSpeed * Time.deltaTime, 0f);
        }
        else
        {
            offset = new Vector2(0f, backgroundScrollSpeed * Time.deltaTime);

        }
    }
    
    // Update is called once per frame
    void Update () {
            myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}
