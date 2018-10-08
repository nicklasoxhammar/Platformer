using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour {

    private bool isShaking = false;
    private Vector3 startPos;
    [SerializeField] float amount = 2f;
    [SerializeField] float shakeTime = 0.5f;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(isShaking)
        {
            Vector3 newPos = startPos + Random.insideUnitSphere * (Time.deltaTime * amount);
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
	}

    public void ShakeThisObjectNow()
    {
        StartCoroutine(ShakeNow());
    }

    IEnumerator ShakeNow()
    {
        if(!isShaking)
        {
            isShaking = true;
        }
        yield return new WaitForSeconds(shakeTime);

        isShaking = false;
        transform.position = startPos;
    }
}
