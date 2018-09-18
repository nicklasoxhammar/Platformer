using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWhenTouch : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    private Color changeFromColor = Color.white;
    private Color changeToColor = Color.black;

    private float colorCount = 0;
    private float duration = 0.5f;



    bool isDark = false;
    bool colorIsChanging = true;


    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //SetColorToBlack();
    }




    // Update is called once per frame
    void Update()
    {

        ChangeColor();

    }



    private void SetColorToWhite()
    {
        if (spriteRenderer.color != Color.white)
        {
            changeFromColor = Color.black;
            changeToColor = Color.white;
            colorIsChanging = true;
        }

    }

    private void SetColorToBlack()
    {
        if (spriteRenderer.color != Color.black)
        {
            changeFromColor = Color.white;
            changeToColor = Color.black;
            colorIsChanging = true;


        }


    }



    private void ChangeColor()
    {
        if (colorIsChanging)
        {
            spriteRenderer.color = Color.Lerp(changeFromColor, changeToColor, colorCount);

            if (colorCount < 1)
            {
                colorCount += Time.deltaTime / duration;
            }
            else
            {
                colorIsChanging = false;
                colorCount = 0;
            }

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            SetColorToWhite();
        }
    }

}
