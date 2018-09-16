using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxAndScrollBackground : MonoBehaviour {

    public Transform[] backgrounds;        //array (list) of all the backgrounds and forgrounds to be parallaxed.
    private float[] parallaxScales;      //The proportion of the cameras movement to move the backgrounds by.
    public float smoothing = 1f;        //how smooth the parallax is going to be. Make sure to set this above 0.

    private Transform cam;             //reference to the main cameras transform.
    private Vector3 previousCamPos;    //the position of the camera in previous frame.

    private Transform currentBackground;
    private Transform otherBackground;

    private float backgroundWidth;


    //is called before start, Great for references.
    void Awake() {
        //set up camera reference.
        cam = Camera.main.transform;

    }

    // Use this for initialization
    void Start() {
        // the previous frame had the current frames camera position.
        previousCamPos = cam.position;

        currentBackground = backgrounds[0];
        otherBackground = backgrounds[1];

        backgroundWidth = currentBackground.gameObject.GetComponent<Renderer>().bounds.size.x;

        //assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z * -1;

        }

    }

    void Update() {

        //for each background
        for (int i = 0; i < backgrounds.Length; i++) {

            //the parallax is the opposite of the camera movement because the previous frame multiplied by the scale.
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            //set a target x position wich is the current position + the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create a target position wich is the backgrounds current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //set the previous camPos to the cameras position at the end of the frame
        previousCamPos = cam.position;


        //Change the "currentBackground" depending on camera position
        if (cam.position.x > currentBackground.position.x + backgroundWidth || cam.position.x < currentBackground.position.x - backgroundWidth) {

            Transform previous = currentBackground;

            currentBackground = otherBackground;
            otherBackground = previous;
        }

        //if current background is to the left of camera then move the other background left
         if (currentBackground.position.x < cam.position.x){
             otherBackground.position = new Vector3(currentBackground.position.x + backgroundWidth, otherBackground.position.y, otherBackground.position.z);
         }

        //if current background is to the right of camera then move the other background right
        if (currentBackground.position.x > cam.position.x) {
            otherBackground.position = new Vector3(currentBackground.position.x - backgroundWidth, otherBackground.position.y, otherBackground.position.z);
        }




    }


}
