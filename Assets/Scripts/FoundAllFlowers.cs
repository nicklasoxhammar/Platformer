using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundAllFlowers : MonoBehaviour
{

    private int rotateId;
    [SerializeField] float rotateTime = 1f;
    [SerializeField] float timeBeforeDisappear = 1f;
    [SerializeField] float size = 3f;
 
    void Start()
    {
        ShowFlower();
    }


    public void ShowFlower()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        transform.localPosition = new Vector3(0.5f, 7.0f);

        transform.localScale = Vector3.zero;
        rotateId = LeanTween.rotateAround(gameObject, Vector3.forward, 360, 0.2f).setLoopClamp().id;
        LeanTween.scale(gameObject, new Vector3(size, size, size), rotateTime).setEaseInQuad().setOnComplete(() =>
        {
            LeanTween.pause(rotateId);
            LeanTween.rotateZ(gameObject, 0, 0.5f).setEaseOutBack();
            LeanTween.alpha(gameObject, 0, 1f).setDelay(timeBeforeDisappear);
        });
    }

}
