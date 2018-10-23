using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiraffeStandUpWhenTouch : MonoBehaviour {
    
    private GiraffeController giraffeController;

	void Start ()
    {
        giraffeController = GetComponentInParent<GiraffeController>();	
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            giraffeController.WakeUpGiraffe();
        }
    }
}
