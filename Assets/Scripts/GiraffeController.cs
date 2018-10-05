using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class GiraffeController : MonoBehaviour {

    private Animator animator;
    private int goToIdleStateHash = Animator.StringToHash("GoToIdle");

	void Start ()
    {
        animator = GetComponentInParent<Animator>();
	}

    public void WakeUpGiraffe()
    {
        animator.SetTrigger(goToIdleStateHash);
    }
}
