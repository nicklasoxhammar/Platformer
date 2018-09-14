using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  [SerializeField] GameObject levelCompleteScreen;
  public int flowerCounter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LevelComplete() {
        levelCompleteScreen.SetActive(true);
    }
  
  public void pickedFlower()
    {
        flowerCounter--;
        Debug.Log("FLOWER PICKED");

        if(flowerCounter <= 0)
        {
            //ALLA PLOCKADE.
            Debug.Log("ALLA PLOCKADE");
        }
    }



    public void AddFlower()
    {
        flowerCounter++;
    }
}
