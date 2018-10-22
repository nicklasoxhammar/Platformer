using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private float parallaxScales;      //The proportion of the cameras movement to move the backgrounds by.
    public float smoothing = 1f;        //how smooth the parallax is going to be. Make sure to set this above 0.

    private Transform cam;             //reference to the main cameras transform.
    private Vector3 previousCamPos;    //the position of the camera in previous frame.

    //is called before start, Great for references.
    void Awake() {
        //set up camera reference.
        cam = Camera.main.transform;

    }

    // Use this for initialization
    void Start() {
        // the previous frame had the current frames camera position.
        cam = Camera.main.transform;

        previousCamPos = cam.position;

        //assigning corresponding parallaxScales
        parallaxScales = transform.position.z * -1;

    }

    void FixedUpdate() {

            //the parallax is the opposite of the camera movement because the previous frame multiplied by the scale.
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales;

            //set a target x position wich is the current position + the parallax
            float backgroundTargetPosX = transform.position.x + parallax;

            //create a target position wich is the backgrounds current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, transform.position.y, transform.position.z);

            //fade between current position and the target position using lerp
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPos, smoothing * Time.deltaTime);

        //set the previous camPos to the cameras position at the end of the frame
        previousCamPos = cam.position;


    }
}
