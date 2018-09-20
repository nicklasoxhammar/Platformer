using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingDrop : MonoBehaviour
{

    [SerializeField] ParticleSystem VFXPrefab;

    private ParticleSystem splashVFX;

    // Use this for initialization
    void Start()
    {
        splashVFX = Instantiate(VFXPrefab, transform.position, Quaternion.LookRotation(Vector2.up));

    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.Die();
            PlayVFX();
            gameObject.SetActive(false);

        }
        else
        {
            PlayVFX();
            gameObject.SetActive(false);

        }
    }



    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }




    private void PlayVFX()
    {
        splashVFX.Clear();
        splashVFX.transform.position = transform.position;
        splashVFX.Play();


    }


}
