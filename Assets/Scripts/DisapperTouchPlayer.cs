using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapperTouchPlayer : MonoBehaviour {

    [SerializeField] private ParticleSystem VFXWhenDisappear;
    [SerializeField] private float fadeTime = 1f;
    private int ignorePlayerLayer = 11;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DisappearAndPlayVFX();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DisappearAndPlayVFX();
        }
    }

    private void DisappearAndPlayVFX()
    {
        gameObject.layer = ignorePlayerLayer;
        ParticleSystem VFX = Instantiate(VFXWhenDisappear, transform);
        LeanTween.alpha(gameObject, 0, VFX.main.duration).setEaseOutSine().setOnComplete(() =>
        {
            Destroy(gameObject);

        });
    }
}
