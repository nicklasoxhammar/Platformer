using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWhenTouch : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    [SerializeField] private float duration = 0.5f;

    bool colorIsChanging = false;

    //Lerp the platform...
    private int id;
    private ColorLerpPlatform colorLerpPlatform;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetColorToBlack();
        colorLerpPlatform = transform.GetComponentInParent<ColorLerpPlatform>();

    }

    public void SetColorToWhite()
    {
        if (spriteRenderer.color != Color.white && !colorIsChanging)
        {
            Debug.Log("CHANGECOLOR");
            colorIsChanging = true;
            LeanTween.color(gameObject, Color.white, duration).setOnComplete(() => 
            {
                colorIsChanging = false;
            });
        }
    }

    private void SetColorToBlack()
    {
        if (spriteRenderer.color != Color.black)
        {
            LeanTween.color(gameObject, Color.black, duration);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SetColorToWhite();
            if(colorLerpPlatform != null)
            {
                colorLerpPlatform.StartLerpColor(id);
            }
        }
    }

    //Other stuff, like tree?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SetColorToWhite();
        }
    }

    //Called from colorLerpPlatform
    public void SetIdToThisTile(int id)
    {
        this.id = id;
    }
}
