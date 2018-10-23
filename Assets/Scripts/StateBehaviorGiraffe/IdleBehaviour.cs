using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class IdleBehaviour : StateMachineBehaviour {

    Animator animator;

    private bool fromSleep = true;
    private string getUpAnimationName = "GetUp";
    private string idleAnimationName = "Idle";
    private string MoveThroatUp = "MoveThroatUp";
    private string moveTail = "MoveTail";
    int isEatingHash = Animator.StringToHash("isEating");

    private SkeletonAnimation skeletonAnimation;
    private GiraffeController giraffeController;

    private string[] earsAndBlinkAnimationNames = new string[] { "MoveEarsFast", "Blink" };
    private float earsAndBlinkTimer;
    //Blink And Move Ears:
    private float earsAndBlinkMinTime;
    private float earsAndBlinkMaxTime;

    //Go from Idle and pick Up Grass...
    private float pickUpGrassMinTime;
    private float pickUpGrassMaxTime;
    private float pickUpgrassTimer;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        skeletonAnimation = animator.GetComponent<SkeletonAnimation>();
        giraffeController = animator.gameObject.GetComponent<GiraffeController>();

        earsAndBlinkMinTime = giraffeController.EarsAndBlinkMinTime;
        earsAndBlinkMaxTime = giraffeController.EarsAndBlinkMaxTime;
        pickUpGrassMinTime = giraffeController.GoFromIdleToEatMinTime;
        pickUpGrassMaxTime = giraffeController.GoFromIdleToEatMaxTime;

        SetMoveEarsAndBlinkTimer();
        pickUpgrassTimer = Random.Range(pickUpGrassMinTime, pickUpGrassMaxTime);

        if(fromSleep)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, getUpAnimationName, false);
            skeletonAnimation.AnimationState.AddAnimation(3, moveTail, true, 2f);

            fromSleep = false;
        }
        else
        {
            skeletonAnimation.AnimationState.SetAnimation(0, MoveThroatUp, false);
        }
        skeletonAnimation.AnimationState.AddAnimation(0, idleAnimationName, true, 0f);
	}



	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        CountDownEarTimer();
        CountDownGoToGrassState(animator);
    }

    private void CountDownGoToGrassState(Animator animatorObject)
    {
        pickUpgrassTimer -= Time.deltaTime;
        if(pickUpgrassTimer<= 0)
        {
            animatorObject.SetBool(isEatingHash, true);
        }
    }

    private void CountDownEarTimer()
    {
        earsAndBlinkTimer -= Time.deltaTime;
        if (earsAndBlinkTimer <= 0)
        {
            int randomInt = Random.Range(0, earsAndBlinkAnimationNames.Length);

            skeletonAnimation.AnimationState.SetAnimation(1, earsAndBlinkAnimationNames[randomInt], false);
            SetMoveEarsAndBlinkTimer();
        }
    }

    private void SetMoveEarsAndBlinkTimer()
    {
        earsAndBlinkTimer = Random.Range(earsAndBlinkMinTime, earsAndBlinkMaxTime);
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
