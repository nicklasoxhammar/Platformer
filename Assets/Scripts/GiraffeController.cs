using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class GiraffeController : MonoBehaviour {


    [SerializeField][Header("Blink And Move Ears Sleep And Idle:")] float earsAndBlinkMinTime = 1f;
    public float EarsAndBlinkMinTime {get{return earsAndBlinkMinTime;}}

    [SerializeField] float earsAndBlinkMaxTime = 8f;
    public float EarsAndBlinkMaxTime {get{return earsAndBlinkMaxTime;}}

    [SerializeField][Header("Go from Idle and Pick Up Grass...")] float goFromIdleToEatMinTime = 5f;
    public float GoFromIdleToEatMinTime {get{return goFromIdleToEatMinTime;}}

    [SerializeField] float goFromIdleToEatMaxTime = 5f;
    public float GoFromIdleToEatMaxTime {get{return goFromIdleToEatMaxTime;}}

    [SerializeField] [Header("From Eating To Idle:")] float goToIdleMinTime = 1f;
    public float GoToIdleMinTime {get{return goToIdleMinTime;}}

    [SerializeField] float goToIdleMaxTime = 2f;
    public float GoToIdleMaxTime {get{return goToIdleMaxTime;}}

    [SerializeField][Header("Blink when eat")] float blinkEatMintime = 0.5f;
    public float BlinkEatMintime {get{return blinkEatMintime;}}

    [SerializeField] float blinkEatMaxTime = 2f;
    public float BlinkEatMaxTime {get{return blinkEatMaxTime;}}

    private Animator animator;
    private int goToIdleStateHash = Animator.StringToHash("GoToIdle");

	void Start ()
    {
        animator = GetComponentInParent<Animator>();
	}

    //Called from collider att giraffes back if player touch.
    public void WakeUpGiraffe()
    {
        animator.SetTrigger(goToIdleStateHash);
    }
}
