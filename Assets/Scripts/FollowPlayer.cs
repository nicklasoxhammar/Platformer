using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float moveCameraAt = 7.0f;

    //this is so the camera doesnt move to the players z position
    float zPos;


    void Start () {

        zPos = transform.position.z;
		
	}

	void Update () {

        //move camera to the right
        if (player.transform.position.x > transform.position.x + moveCameraAt) {

            transform.position = transform.position = new Vector3(player.transform.position.x - moveCameraAt, transform.position.y, zPos);
        }
        //move camera to the left
        if (player.transform.position.x < transform.position.x - moveCameraAt) {

            transform.position = transform.position = new Vector3(player.transform.position.x + moveCameraAt, transform.position.y, zPos);
        }
        //move camera up
        if (player.transform.position.y > transform.position.y + moveCameraAt) {

            transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y - moveCameraAt, zPos);
        }
        //move camera down
        if (player.transform.position.y < transform.position.y - moveCameraAt) {

            transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y + moveCameraAt, zPos);
        }

    }
}
