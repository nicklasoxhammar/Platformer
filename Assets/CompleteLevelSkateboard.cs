using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevelSkateboard : MonoBehaviour {

    private GameObject player = null;
    private Vector3 offset;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionEnter2D(Collision2D col) {

        if (col.gameObject.tag == "Player") {
            player = col.gameObject;
            //offset = player.transform.position - transform.position;

            player.transform.parent = transform;

            CompleteLevel();
        }
    }

    void CompleteLevel() {

       
        player.GetComponent<PlayerController>().freezeMovement = true;
        Destroy(player.GetComponent<Rigidbody2D>());

        StartCoroutine(MoveAround());

        Debug.Log("LEVEL COMPLETE!");


    }

    IEnumerator MoveAround() {

        float duration = 1.0f;

        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration) {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(0, 0, zRotation);
            yield return null;
        }

    }
   
    
}
