using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SharkEnemyController : MonoBehaviour {


    //Flying animations... Använd en i taget av flying.
    private string flying1AnimationName = "Flying1";
    private string flying2AnimationName = "Flying2";
    private string glidInAirAnimationName = "Glidning";
    private string SetWingsDownAnimationName = "WingsDowm";
    private string SetWingsUpAnimationName = "WingsUp";

    //Mouth...använd en i taget.
    private string chewingAnimationName = "Chewing";
    private string closeMouthAnimationName = "CloseMouth";
    private string openMouthAnimationName = "OpenMouth";

    //Other
    private string sniffAnimationName = "Sniff";
    private string swimAnimationName = "Swim";


    //Annars kan alla kombineras och köras samtidigt.


	// Use this for initialization
	void Start () {

        SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.AnimationState.SetAnimation(0, flying2AnimationName, true);
        skeletonAnimation.AnimationState.SetAnimation(1, sniffAnimationName, true);
        skeletonAnimation.AnimationState.SetAnimation(4, swimAnimationName, true);
        skeletonAnimation.AnimationState.SetAnimation(3, chewingAnimationName, true);
        skeletonAnimation.AnimationState.AddAnimation(3, openMouthAnimationName, false, 3f);
        skeletonAnimation.AnimationState.AddAnimation(3, closeMouthAnimationName, false, 3f);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
