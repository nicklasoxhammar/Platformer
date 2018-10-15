using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AllAssets : MonoBehaviour {
    public GameObject b1;
    public GameObject b3;
    public GameObject b4;
    public GameObject b5;
    public GameObject b6;
    public GameObject b7;
    public GameObject tx;

    public GameObject Clouds;
    public GameObject Grounds;
    public GameObject Mountains;
    public GameObject Plants;
    public GameObject Trees;
    
    Text txt;

    string ne= "134567";
    int han = 0;
    string be = "";
    
    // Use this for initialization
    void Start () {
        AllHidden("1");

        txt = tx.GetComponent<Text>();
        txt.text = "Press 1 - 2 - 3 - 4 - 5 Keys for Assets\nSpace Key: Change Background";

    }

    void ObjectHidden(string ac) {
        Clouds.SetActive(false);
        Grounds.SetActive(false);
        Mountains.SetActive(false);
        Plants.SetActive(false);
        Trees.SetActive(false);
        if (ac == "1")
        {
            Clouds.SetActive(true);
        }
        if (ac == "2")
        {
            Grounds.SetActive(true);
        }
        if (ac == "3")
        {
            Mountains.SetActive(true);
        }
        if (ac == "4")
        {
            Plants.SetActive(true);
        }
        if (ac == "5")
        {
            Trees.SetActive(true);
        }
    }

    void AllHidden(string ac) {
        b1.SetActive(false);
        b3.SetActive(false);
        b4.SetActive(false);
        b5.SetActive(false);
        b6.SetActive(false);
        b7.SetActive(false);
        if (ac == "1")
        {
            b1.SetActive(true);
        }
        if (ac == "3")
        {
            b3.SetActive(true);
        }
        if (ac == "4")
        {
            b4.SetActive(true);
        }
        if (ac == "5")
        {
            b5.SetActive(true);
        }
        if (ac == "6")
        {
            b6.SetActive(true);
        }
        if (ac == "7")
        {
            b7.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
      
        if (Input.GetKeyUp(KeyCode.Space) == true)
        {
            han++;
            if (han > 5) {
                han = 0;
            }
            be = ne.Substring(han, 1);
            Debug.Log(be);
            AllHidden(be);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1) == true)
        {
            ObjectHidden("1");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) == true)
        {
            ObjectHidden("2");
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) == true)
        {
            ObjectHidden("3");
        }
        if (Input.GetKeyUp(KeyCode.Alpha4) == true)
        {
            ObjectHidden("4");
        }
        if (Input.GetKeyUp(KeyCode.Alpha5) == true)
        {
            ObjectHidden("5");
        }

    }
}
