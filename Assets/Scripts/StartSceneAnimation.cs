using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class StartSceneAnimation : MonoBehaviour {

    private string fallingAnimationName = "FALLING";
    private string jumpAnimationName = "JUMP";
    private string idleAnimationName = "STANDING";
    private string runAnimationName = "RUN";
    public GameObject elda;
    public Transform movingPoints;
    private SkeletonAnimation skeletonAnimationElda;

	// Use this for initialization
	void Start () {
        skeletonAnimationElda = elda.GetComponent<SkeletonAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void MoveToRight()
    {
        Debug.Log("MOVE");
        LeanTween.moveX(elda, movingPoints.GetChild(1).transform.position.x, 2f);
        skeletonAnimationElda.AnimationName = runAnimationName;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MoveToRight();
    }
}
