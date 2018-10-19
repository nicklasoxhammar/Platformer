using UnityEngine;

public class RepeatBackground : MonoBehaviour {

    public Transform[] backgrounds;        //array (list) of all the backgrounds and forgrounds to be parallaxed.

    private Transform cam;             //reference to the main cameras transform.

    public Transform cameraIfNotMain;
    private Transform currentBackground;
    private Transform otherBackground;

    private float backgroundWidth;

    private float speed = 10.0f;


    void Start() {

        cam = Camera.main.transform;
        if (cameraIfNotMain!=null)
        {
            cam = cameraIfNotMain;
        }

        currentBackground = backgrounds[0];
        otherBackground = backgrounds[1];

        backgroundWidth = currentBackground.gameObject.GetComponent<Renderer>().bounds.size.x;

    }

    void Update() {

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

        transform.position = new Vector3(transform.position.x + 1 * speed * Time.deltaTime, transform.position.y);

    }

    


}
