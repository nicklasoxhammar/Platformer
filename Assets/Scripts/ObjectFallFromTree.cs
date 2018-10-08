using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallFromTree : MonoBehaviour {

    Rigidbody2D rg;
	// Use this for initialization
	void Start () {
        rg = GetComponent<Rigidbody2D>();
        rg.gravityScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}




    public void ShakeAndFall()
    {
        LeanTween.rotateY(gameObject, 10f, 1f).setEaseShake().setOnComplete(() => {
            rg.gravityScale = 1;
        });
    }

}
