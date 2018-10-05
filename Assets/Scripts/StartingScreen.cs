using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScreen : MonoBehaviour {

    public static bool started = false; 

    private void Awake() {
        DontDestroyOnLoad(this);
    }

}
