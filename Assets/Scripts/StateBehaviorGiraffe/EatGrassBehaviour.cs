using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EatGrassBehaviour : StateMachineBehaviour {

    SkeletonAnimation skeletonAnimation;
    private GiraffeController giraffeController;
    int isEatingHash = Animator.StringToHash("isEating");
    //Animation Names
    private string idleDown = "EatingGrassDown";
    private string moveThroatDown = "MoveThroatDown";
    private string blink = "Blink";
    string eatGrassAnimationName = "EatUpGrass";

    private float goToIdleTimer;
    private float goToIdleMinTime;
    private float goToIdleMaxTime;

    private float blinkTimer;
    private float blinkMintime;
    private float blinkMaxTime;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        skeletonAnimation = animator.GetComponent<SkeletonAnimation>();
        giraffeController = animator.gameObject.GetComponent<GiraffeController>();
        goToIdleMinTime = giraffeController.GoToIdleMinTime;
        goToIdleMaxTime = giraffeController.GoToIdleMaxTime;
        blinkMintime = giraffeController.BlinkEatMintime;
        blinkMaxTime = giraffeController.BlinkEatMaxTime;
        goToIdleTimer = Random.Range(goToIdleMinTime, goToIdleMaxTime);
        SetBlinkTimer();
        skeletonAnimation.AnimationState.SetAnimation(0, moveThroatDown, false);
        skeletonAnimation.AnimationState.AddAnimation(0, idleDown, true, 0f);
        skeletonAnimation.AnimationState.AddAnimation(2, eatGrassAnimationName, true, 2f);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goToIdleTimer -= Time.deltaTime;
        if(goToIdleTimer<=0)
        {
            animator.SetBool(isEatingHash, false);
        }
        blinkTimer -= Time.deltaTime;
        if (blinkTimer<=0)
        {
            skeletonAnimation.AnimationState.AddAnimation(1, blink, false, 0f);
            SetBlinkTimer();
        }
	}

    private void SetBlinkTimer()
    {
        blinkTimer = Random.Range(blinkMintime, blinkMaxTime);
    }
}
