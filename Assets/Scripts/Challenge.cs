using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour {

    public float floatValue = 0;
    public bool boolValue = false;
    public string challengeName = "";
    public string challengeText = "";
    public bool completed = false;

	public Challenge(string name, float floatValue, string text) {
        this.floatValue = floatValue;
        this.challengeName = name;
        this.challengeText = text;
    }

    public Challenge(string name, bool boolValue, string text) {
        this.boolValue = boolValue;
        this.challengeName = name;
        this.challengeText = text;
    }
   

}
