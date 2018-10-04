using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
    
    [SerializeField] float speed = 1f;
    [SerializeField] ParticleSystem crashVFXPrefan;
    private ParticleSystem crashVFX;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Collider2D target;

	// Use this for initialization
	void Start () {
        target = FindObjectOfType<PlayerController>().GetComponent<Collider2D>();
        if (target == null) { return; }
        rb = GetComponent<Rigidbody2D>();

        crashVFX = Instantiate(crashVFXPrefan);
	}

    // Update is called once per frame
    void Update()
    {
        rb.position += targetPosition * speed * Time.deltaTime;
    }

    private void PlayCrashVFX()
    {
        crashVFX.Clear();
        crashVFX.transform.rotation = transform.rotation;
        crashVFX.transform.position = transform.position;
        crashVFX.Play();
    }

    public void ResetLaser()
    {
        targetPosition = (target.bounds.center - transform.position).normalized;
        float atan2 = Mathf.Atan2(targetPosition.y, targetPosition.x);
        transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "LaserFlyingThru")
        {
            PlayCrashVFX();
            gameObject.SetActive(false);
        }
    }

    private void OnBecameVisible()
    {
        ResetLaser();   
    }


    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
