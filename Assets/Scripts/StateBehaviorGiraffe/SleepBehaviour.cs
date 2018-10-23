using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SleepBehaviour : StateMachineBehaviour {

    private float minTimeMovingEars;
    private float maxTimeMovingEars;

    private SkeletonAnimation skeletonAnimation;
    private string sleepAnimationName = "Sleep";
    private string moveEarsAnimationName = "MoveEarsSlow";
    private float moveEarsTimer;
    private GiraffeController giraffeController;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        skeletonAnimation = animator.GetComponent<SkeletonAnimation>();
        giraffeController = animator.gameObject.GetComponent<GiraffeController>();
        minTimeMovingEars = giraffeController.EarsAndBlinkMinTime;
        maxTimeMovingEars = giraffeController.EarsAndBlinkMaxTime;
        skeletonAnimation.AnimationState.SetAnimation(0, sleepAnimationName, true);
        updateTimer();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
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
}
