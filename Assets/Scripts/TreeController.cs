using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;


public class TreeController : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera virualCamBig;
    [SerializeField] CinemachineVirtualCamera virualCamObjectInTree;
    [SerializeField] float waitSecBeforeOriginalCamera = 2f;
    private ObjectFallFromTree objectInTree;
    private bool isMoving;

    // Use this for initialization
    void Start()
    {
        objectInTree = GetComponentInChildren<ObjectFallFromTree>();
        //virualCamBig = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && CrossPlatformInputManager.GetButtonDown("Dash") && !isMoving)
        {
            isMoving = true;
            virualCamBig.enabled = true;
            LeanTween.rotateZ(transform.parent.gameObject, 4f, 1f).setEaseInBack().setOnComplete(() =>
            {
                LeanTween.rotateZ(transform.parent.gameObject, -4f, 0.2f).setRepeat(5).setLoopPingPong()
                         .setOnComplete(() =>
                         {
                             LeanTween.rotateZ(transform.parent.gameObject, 0f, 1f).setEaseOutBounce()
                                      .setOnComplete(() =>
                                      {

                                          isMoving = false;
                                          virualCamBig.enabled = false;
                                          virualCamObjectInTree.enabled = true;
                                          objectInTree.ShakeAndFall();
                                          StartCoroutine(EnableOriginalCamera());
                                      });
                         });
            });
        }

    }




    IEnumerator EnableOriginalCamera()
    {
        yield return new WaitForSeconds(waitSecBeforeOriginalCamera);
        virualCamObjectInTree.enabled = false;
        isMoving = false;
    }



}
