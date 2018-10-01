using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class GiraffeController : MonoBehaviour {

    private Animator animator;
    [SerializeField] Collider2D giraffeBack;

	// Use this for initialization
	void Start () {
        
        animator = GetComponentInParent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {



	}





}
