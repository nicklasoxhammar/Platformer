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
    //When the stone is falling... 
    private float startMass;

    //Stop player from jumping up and down...
    private float massFallingDown = 0.5f;
    private bool corutineIsRunning = false;
    private Vector2 startPosition;
    private bool playerCollision = false;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        startPosition = myRigidBody.position;
        startMass = myRigidBody.mass;
    }

    private void FixedUpdate()
    {
        if(!elevatorIsOn)
        {
            moveStoneToStartPosition();
        }
        else
        {
            runElevator();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLL!!!" + collision.gameObject.name);

        if (!elevatorIsOn && collision.transform.tag == "Player")
        {
            playerCollision = true;
            myRigidBody.mass = startMass;

        }
        else if (elevatorIsOn && collision.transform.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //player = null;

        if(!elevatorIsOn && !corutineIsRunning && collision.transform.tag == "Player")
        {
            StartCoroutine(runPlayerLeftStone(collision));
        }
        else if(elevatorIsOn && collision.transform.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }


    IEnumerator runPlayerLeftStone(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            corutineIsRunning = true;

            yield return new WaitForSeconds(secToWaitBeforeGoingUp);
            playerCollision = false;
            corutineIsRunning = false;
        }


    }


    //When elevator is off.
    private void moveStoneToStartPosition()
    {
        myRigidBody.isKinematic = false;

        if (myRigidBody.position != startPosition && !playerCollision)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, startPosition, speedGoingUp * Time.deltaTime);
            myRigidBody.MovePosition(newPos);
            myRigidBody.velocity = new Vector3(0f, 0f, 0f);
            myRigidBody.mass = massFallingDown;

        }

    }



    //ELEVATOR IS ON...

    private void runElevator()
    {
        myRigidBody.isKinematic = true;
        //Just standing still if no positions.

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


    public void SetElevatorStatusTo(bool status)
    {
        elevatorIsOn = status;
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }
}
