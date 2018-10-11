using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapperTouchPlayer : MonoBehaviour {

    [SerializeField] private ParticleSystem VFXWhenDisappear;
    [SerializeField] private float fadeTime = 1f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Ignore player layer...
            gameObject.layer = 11;
            ParticleSystem VFX = Instantiate(VFXWhenDisappear, transform);
            LeanTween.alpha(gameObject, 0, fadeTime).setEaseOutSine().setOnComplete(() =>
            {
                Destroy(gameObject, 2f);

            });

        }
    }
}
