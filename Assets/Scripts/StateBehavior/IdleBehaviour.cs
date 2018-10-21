using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class IdleBehaviour : StateMachineBehaviour {

    Animator animator;

    private bool fromSleep = true;
    string getUpAnimationName = "GetUp";
    string idleAnimationName = "Idle";
    private string MoveThroatUp = "MoveThroatUp";
    private string moveTail = "MoveTail";

    private SkeletonAnimation skeletonAnimation;

    string[] earsAndBlinkAnimationNames = new string[] { "MoveEarsFast", "Blink" };
    float earsAndBlinkTimer;
    [SerializeField][Header("Blink And Move Ears:")] float earsAndBlinkMinTime = 1f;
    [SerializeField] float earsAndBlinkMaxTime = 8f;


    int isEatingHash = Animator.StringToHash("isEating");
    [SerializeField]
    [Header("Pick Up Grass...")]
    float pickUpGrassMinTime = 5f;
    [SerializeField] float pickUpGrassMaxTime = 5f;
    private float pickUpgrassTimer;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
        skeletonAnimation = animator.GetComponent<SkeletonAnimation>();

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

    private void CountDownGoToGrassState(Animator animator)
    {
        pickUpgrassTimer -= Time.deltaTime;
        if(pickUpgrassTimer<= 0)
        {
            animator.SetBool(isEatingHash, true);
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
