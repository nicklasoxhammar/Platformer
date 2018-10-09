using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallFromTree : MonoBehaviour
{

    Rigidbody2D rg;
    // Use this for initialization
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        //rg.isKinematic = true;
        rg.gravityScale = 0;
        rg.velocity = Vector3.zero;
    }


    public void ShakeAndFall(float time)
    {
        float animationTime = time * 0.5f;

        LeanTween.rotateZ(gameObject, 10f, animationTime).setEaseShake().setRepeat(2).setLoopPingPong().setOnComplete(() =>
        {
            LeanTween.rotateZ(gameObject, -10f, animationTime).setEaseOutBack().setOnComplete(() =>
            {
                //rg.isKinematic = false;
                rg.gravityScale = 1;

            });
        });
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rg.velocity = Vector2.zero;
        }
    }

}
