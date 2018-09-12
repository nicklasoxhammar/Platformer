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
    private int currentMovingPoint;
    private Transform[] movingPoints;


    [Header("When elevator is off:")]
    [SerializeField] float speedGoingUp = 2f;
    [SerializeField] float secToWaitBeforeGoingUp = 0.5f;
    //When the stone is falling... 
    private float startMass;

    //Stop player from jumping up and down...
    private float massFallingDown = 0.01f;
    private bool corutineIsRunning = false;
    private Vector2 startPosition;
    private bool playerCollision = false;

    // Use this for initialization
    void Start()
    {
        
        myRigidBody = GetComponent<Rigidbody2D>();
        startPosition = myRigidBody.position;
        startMass = myRigidBody.mass;


        movingPoints = destinations.GetComponentsInChildren<Transform>();
    }



    private void FixedUpdate()
    {
        if(!elevatorIsOn)
        {
            moveStoneToStartPosition();
            stopElevator();

        }
        else
        {
            runElevator();
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (!elevatorIsOn && collision.transform.tag == "Player")
        {
            playerCollision = true;
            myRigidBody.mass = startMass;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!elevatorIsOn && !corutineIsRunning)
        {
            StartCoroutine(runPlayerLeftStone(collision));
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


    private void moveStoneToStartPosition()
    {

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
        if(movingPoints.Length > 1)
        {
            Vector2 moveTowards = movingPoints[currentMovingPoint].position;

            myRigidBody.position = Vector2.MoveTowards(transform.position, moveTowards, elevatorSpeed * Time.deltaTime);

            if (myRigidBody.position == moveTowards && currentMovingPoint <= movingPoints.Length - 1)
            {
                //reached your destination

                Debug.Log("CURRENT" + currentMovingPoint);
                Debug.Log(movingPoints.Length);
                currentMovingPoint++;
                if (currentMovingPoint == movingPoints.Length)
                {
                    currentMovingPoint = 0;
                }




            }
            //myRigidBody.MovePosition(movingPoints[currentMovingPoint].position * Time.deltaTime);


            //Vector2 newPos = Vector2.Lerp(transform.position, movingPoints[currentMovingPoint].position, elevatorSpeed * Time.deltaTime);
            //myRigidBody.velocity = newPos;

            //if(transform.position == movingPoints[currentMovingPoint].position)
            //{
            //    Debug.Log("HEJ");
            //}

        }






    }

    private void stopElevator()
    {
        myRigidBody.isKinematic = false;


    }





    //void FixedUpdate()
    //{
    //    Vector3 dir = patrolPoints[pointToReach].transform.position - rb.position;
    //    dir.Normalize();

    //    rb.MovePosition(rb.position + dir * Time.fixedDelta * moveSpeed);

    //    float sqrMag = (patrolPoints[pointToReach].transform.position - rb.position).sqrMagnitude;

    //    if (sqrMag > 0.1)
    //    {
    //        // You have reached your destination
    //        rb.position = patrolPoints[pointToReach].transform.position;
    //    }
    //}

}
