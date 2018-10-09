using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleObject : MonoBehaviour {

    [SerializeField] int invincibleTime = 5;
    [SerializeField] ParticleSystem VFXWhenDisappear;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetInvincibleTime()
    {
        return invincibleTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ParticleSystem VFX = Instantiate(VFXWhenDisappear, transform);
            VFX.Play();
            Destroy(VFX, VFX.main.duration);
            LeanTween.alpha(gameObject, 0, 1f).setEaseOutSine().setOnComplete(() =>
            {
                Destroy(gameObject, 2f);

            });

        }
    }
}
