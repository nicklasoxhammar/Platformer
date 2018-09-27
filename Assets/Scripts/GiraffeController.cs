using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class GiraffeController : MonoBehaviour {

    private Animator animator;

    int goToIdleStateHash = Animator.StringToHash("GoToIdle");

	// Use this for initialization
	void Start () {

        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.Log("HEJKLAJSKLJSDKLAJDKALSDJKSLADJKLASD");
        }
	}
	
	// Update is called once per frame
	void Update () {


	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger(goToIdleStateHash);
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        animator.SetTrigger(goToIdleStateHash);
    //    }
    //}

}
