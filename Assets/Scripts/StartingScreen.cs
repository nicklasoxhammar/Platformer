using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScreen : MonoBehaviour {

    private void Start() {
        Invoke("StartMainMenuScene", 10.0f);
    }

    void StartMainMenuScene() { 
        GetComponent<SceneHandler>().MainMenu();
    }

}
