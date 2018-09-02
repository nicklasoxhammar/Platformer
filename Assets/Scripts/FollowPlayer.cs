using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float moveCameraAt = 7.0f;

    float zPos;


    void Start () {

        zPos = transform.position.z;
		
	}

	void Update () {

        if (player.transform.position.x > transform.position.x + moveCameraAt) {

            transform.position = transform.position = new Vector3(player.transform.position.x - moveCameraAt, transform.position.y, zPos);
        }

        if (player.transform.position.x < transform.position.x - moveCameraAt) {

            transform.position = transform.position = new Vector3(player.transform.position.x + moveCameraAt, transform.position.y, zPos);
        }

        if (player.transform.position.y > transform.position.y + moveCameraAt) {

            transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y - moveCameraAt, zPos);
        }

        if (player.transform.position.y < transform.position.y - moveCameraAt) {

            transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y + moveCameraAt, zPos);
        }

    }
}
