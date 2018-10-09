using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectFallFromTree : MonoBehaviour
{
    [SerializeField] float rotationAngle = 20f;
    Rigidbody2D rg;
    // Use this for initialization
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        rg.gravityScale = 0;
        rg.velocity = Vector3.zero;
    }


    public void ShakeAndFall(float time)
    {
        float animationTime = time * 0.5f;

        LeanTween.rotateZ(gameObject, GetRandomAngle(), animationTime).setEaseShake().setRepeat(2).setLoopPingPong().setOnComplete(() =>
        {
            LeanTween.rotateZ(gameObject, GetRandomAngle(), animationTime).setEaseOutBack().setOnComplete(() =>
            {
                //rg.isKinematic = false;
                rg.gravityScale = 1;

            });
        });
    }

    private float GetRandomAngle()
    {
        return Random.Range(-rotationAngle, rotationAngle);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rg.velocity = Vector2.zero;
        }
    }

}
