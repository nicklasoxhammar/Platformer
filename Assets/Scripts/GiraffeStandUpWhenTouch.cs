using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiraffeStandUpWhenTouch : MonoBehaviour {
    
    private Animator animator;

    int goToIdleStateHash = Animator.StringToHash("GoToIdle");

	// Use this for initialization
	void Start () {

        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.Log("Did not find animator component");
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger(goToIdleStateHash);
    }
}
