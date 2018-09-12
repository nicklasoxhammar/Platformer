using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour {

	void Start () {

        //get how many levels the player has completed
        int progress = PlayerPrefs.GetInt("progress", 1);

        //unlock completed level buttons
        for(int i = 0; i < progress; i++) {
            transform.GetChild(i).GetComponent<Button>().interactable = true;
        }

        
		
	}
	
	
}
