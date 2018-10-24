using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{

    private Rigidbody2D myRigidBody;
    public bool elevatorIsOn = false;

    [Header("When elevator is on:")]
    [SerializeField] GameObject destinations;
    [SerializeField] float elevatorSpeed = 10f;
    private int indexDestination;

    [Header("When elevator is off:")]
    [SerializeField] float speedGoingUp = 2f;
    [SerializeField] float secToWaitBeforeGoingUp = 0.5f;

    private Vector3 startPosition;
    private bool playerCollision = false;
    private bool moveStoneBackToStart = true;

    private GameObject target = null;
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        startPosition = myRigidBody.position;
        myRigidBody.isKinematic = true;
    }

    private void Update()
    {
        if(!elevatorIsOn)
        {
            MoveStoneElevatorOff();
        }
    }

    private void FixedUpdate()
    {
        if (elevatorIsOn)
        {
            RunElevator();
        }
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!elevatorIsOn && collision.transform.tag == "Player")
        {
            moveStoneBackToStart = false;
            playerCollision = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (elevatorIsOn && collision.gameObject.tag == "Player")
            {
            target = collision.gameObject;
            offset = target.transform.position - transform.position;
            }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        target = null;
        if(!elevatorIsOn && collision.transform.tag == "Player")
        {
            playerCollision = false;
            StartCoroutine(SetPlayerCollisionToFalse());
        }
    }

    IEnumerator SetPlayerCollisionToFalse()
    {
        yield return new WaitForSeconds(secToWaitBeforeGoingUp);
        //If still no collision
        if(!playerCollision)
        {
            moveStoneBackToStart = true;
        }
        else
        {
            moveStoneBackToStart = false;
        }
    }

    //When elevator is off. called from fixed update
    private void MoveStoneElevatorOff()
    {
        if (Vector3.Distance(transform.position, startPosition) > 0.1f && moveStoneBackToStart)
        {
            myRigidBody.isKinematic = true;
            myRigidBody.velocity = new Vector3(0f, 0f, 0f);
            Vector3 newPos = Vector3.Lerp(transform.position, startPosition, speedGoingUp * Time.deltaTime);
            transform.position = newPos;
        }
        else if(!moveStoneBackToStart)
        {
            myRigidBody.isKinematic = false;
        }
    }

    //ELEVATOR IS ON...called from fixed update...
    private void RunElevator()
    {
        myRigidBody.isKinematic = true;
        //Just standing still if no positions.
        if(destinations.transform.childCount > 0)
        {
            Vector2 moveTowards = destinations.transform.GetChild(indexDestination).position;
            transform.position = Vector2.MoveTowards(transform.position, moveTowards, elevatorSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, moveTowards) <= 0.01)
            {
                //reached your destination
                indexDestination++;
                if (indexDestination == destinations.transform.childCount)
                {
                    indexDestination = 0;
                }
            }
        }
    }
    //When use switch...
    public void SetElevatorStatusTo(bool status)
    {
        elevatorIsOn = status;
    }
}
