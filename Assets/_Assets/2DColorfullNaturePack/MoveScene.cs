using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour {
    public GameObject b1;
    public GameObject b3;
    public GameObject b4;
    public GameObject b5;
    public GameObject b6;
    public GameObject b7;
    public GameObject tx;
    Text txt;

    string ne= "134567";
    int han = 0;
    string be = "";
    
    // Use this for initialization
    void Start () {
        AllHidden("1");

        txt = tx.GetComponent<Text>();
        txt.text = "Arrow Keys: Left, Right, Up, Down\nSpace Key: Change Background\n1-2 Keys For Scene 1 or Scene 2";

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
        if (Input.GetKey(KeyCode.LeftArrow) == true) {
            Vector3 position = this.transform.position;
            position.x = position.x - 10 * Time.deltaTime;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            Vector3 position = this.transform.position;
            position.x = position.x + 10 * Time.deltaTime;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            Vector3 position = this.transform.position;
            position.y = position.y + 10 * Time.deltaTime;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.DownArrow) == true)
        {
            Vector3 position = this.transform.position;
            position.y = position.y - 10 * Time.deltaTime;
            this.transform.position = position;
        }
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
            SceneManager.LoadScene("SampleScene1");
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) == true)
        {
            SceneManager.LoadScene("SampleScene2");
        }

    }
}
