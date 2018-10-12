using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectFallFromTree : MonoBehaviour
{
    [SerializeField] float rotationAngle = 20f;
    Rigidbody2D rg;
    private bool isFallen = false;
    [SerializeField] bool zeroZWhenFallen = true;
    [SerializeField] bool jumpableWhenFallen = false;

    ////the object will not be placed on top of each other with dynamic rg..
    // Use this for initialization
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        rg.gravityScale = 0;
        //Set layer to ignore player when objects still are in tree.
        gameObject.layer = 11;
    }

    private void Update()
    {
        //Stay if something touch when they not fallen.
        if (!isFallen)
        {
            rg.velocity = Vector2.zero;
        }
    }

    public void ShakeAndFall(float dropAfterTime)
    {
        float time = dropAfterTime * 0.33f;
        
        //Rotate
        LeanTween.rotateZ(gameObject, GetRandomAngle(), time).setEaseShake().setRepeat(3).setLoopPingPong().setOnComplete(() =>
        {
            //Ignore jumpable tree
            gameObject.layer = 15;
            rg.gravityScale = 1;
            isFallen = true;
            if(!zeroZWhenFallen)
            {
                LeanTween.rotateZ(gameObject, GetRandomAngle(), time).setEaseOutQuad();
            }
            else
            {
                LeanTween.rotateZ(gameObject, 0f, time).setEaseOutQuad();
            }
        });
    }

    private float GetRandomAngle()
    {
        return Random.Range(-rotationAngle, rotationAngle);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
            rg.velocity = Vector3.zero;
            rg.isKinematic = true;
            //Set to ground.
            if(jumpableWhenFallen)
            {
                //Set to "Jumpable"
                gameObject.layer = 13;
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Jumpable"))
        {
            if (jumpableWhenFallen)
            {
                //Set to "Jumpable"
                gameObject.layer = 13;
            }
        }
    }

}
