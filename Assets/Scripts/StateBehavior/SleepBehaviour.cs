using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SleepBehaviour : StateMachineBehaviour {

    [SerializeField] float minTimeMovingEars = 2f;
    [SerializeField] float maxTimeMovingEars = 6f;

    private SkeletonAnimation skeletonAnimation;
    private string sleepAnimationName = "Sleep";
    private string moveEarsAnimationName = "MoveEarsSlow";


    private float moveEarsTimer;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
        skeletonAnimation = animator.GetComponent<SkeletonAnimation>();

        skeletonAnimation.AnimationState.SetAnimation(0, sleepAnimationName, true);


        updateTimer();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        moveEarsTimer -= Time.deltaTime;
        if (moveEarsTimer<=0)
        {
            skeletonAnimation.AnimationState.SetAnimation(1, moveEarsAnimationName, false);
            updateTimer();
        }
	}


    private void updateTimer()
    {
        moveEarsTimer = Random.Range(minTimeMovingEars, maxTimeMovingEars);

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
