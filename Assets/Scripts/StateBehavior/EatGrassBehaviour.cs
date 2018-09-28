using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EatGrassBehaviour : StateMachineBehaviour {

    SkeletonAnimation skeletonAnimation;

    int isEatingHash = Animator.StringToHash("isEating");

    //Animation Names
    private string idleDown = "EatingGrassDown";
    private string moveThroatDown = "MoveThroatDown";
    private string MoveThroatUp = "MoveThroatUp";
    private string eatingGrass = "EatUpGrass";
    private string blink = "Blink";
    string eatGrassAnimationName = "EatUpGrass";


    private float goToIdleTimer;
    [SerializeField]float goToIdleMinTime = 1f;
    [SerializeField]float goToIdleMaxTime = 2f;

    private float blinkTimer;
    [SerializeField] float blinkMintime = 0.5f;
    [SerializeField] float blinkMaxTime = 2f;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        skeletonAnimation = animator.GetComponent<SkeletonAnimation>();

        AnimationState state = new AnimationState();

        goToIdleTimer = Random.Range(goToIdleMinTime, goToIdleMaxTime);
        SetBlinkTimer();

        skeletonAnimation.AnimationState.SetAnimation(0, moveThroatDown, false);
        skeletonAnimation.AnimationState.AddAnimation(0, idleDown, true, 0f);
        skeletonAnimation.AnimationState.AddAnimation(2, eatGrassAnimationName, true, 2f);






	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

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

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
