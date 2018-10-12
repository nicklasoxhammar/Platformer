using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{


    public float shieldTimer = 0;
    private bool shieldIsOn = false;
    [SerializeField] int blinkThreshold = 1;
    [SerializeField] float timeBetweenBlink = 0.3f;
    [SerializeField] float scaleSize = 1.2f;
    [SerializeField] float scaleTime = 0.6f;

    private SpriteRenderer spriteRenderer;
    private Color color;
    private int pingPongAnimation;
    private bool isBlinking = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pingPongAnimation = LeanTween.scale(gameObject, Vector2.one * scaleSize, scaleTime).setEaseInOutBack().setLoopPingPong().id;
        LeanTween.pause(pingPongAnimation);
        SetShieldAlphaTo(0f);
    }

    // Update is called once per frame
    void Update()
    {
        StartTimer();
    }

    public bool GetIsWearingShield()
    {
        return shieldIsOn;
    }


    public void WearShieldInSec(float sec)
    {
        shieldTimer += sec;
        FadeInAndStartShield();
    }


    //Called from update
    private void StartTimer()
    {
        if (shieldIsOn)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                FadeOutAndStopShield();
            }
            else if (shieldTimer < blinkThreshold &&!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkWithDelay(timeBetweenBlink));
            }
        }
    }

    private void SetShieldAlphaTo(float alpha)
    {
        color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    IEnumerator BlinkWithDelay(float delay)
    {
        while (shieldTimer > 0 && shieldTimer < blinkThreshold)
        {
            SetShieldAlphaTo(0f);
            yield return new WaitForSeconds(delay);
            SetShieldAlphaTo(1f);
            yield return new WaitForSeconds(delay);

        }
        isBlinking = false;
    }

    private void FadeInAndStartShield()
    {
        if(!shieldIsOn){
            shieldIsOn = true;
            LeanTween.alpha(gameObject, 1, 1f);
            LeanTween.resume(pingPongAnimation);
        }
    }


    private void FadeOutAndStopShield()
    {
        LeanTween.alpha(gameObject, 0, 1f).setOnComplete(() => 
        {
            LeanTween.pause(pingPongAnimation);
            shieldIsOn = false;
        });
    }


    //Change color to...

}
