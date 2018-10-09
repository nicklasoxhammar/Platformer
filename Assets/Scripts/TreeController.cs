using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;


public class TreeController : MonoBehaviour
{
    [SerializeField] GameObject objectsToPutInTreePrefab;
    [SerializeField] int numberOfObjects = 2;
    [SerializeField] int sizeOfObjectArea = 8;
    [SerializeField] float offsetYArea = 4f;
    [SerializeField] CinemachineVirtualCamera VRCamBig;
    [SerializeField] CinemachineVirtualCamera VRCamObjectInTree;
    private ObjectFallFromTree objectInTree;
    private bool isMoving;

    private List<ObjectFallFromTree> listOfObjects = new List<ObjectFallFromTree>();

    CinemachineTargetGroup targetGroup;
    // Use this for initialization
    void Start()
    {
        targetGroup = GameObject.Find("TargetGroup2").GetComponent<CinemachineTargetGroup>();

        List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            ObjectFallFromTree newObject = Instantiate(objectsToPutInTreePrefab, transform).GetComponent<ObjectFallFromTree>();
            Vector2 position = new Vector2(newObject.transform.position.x, newObject.transform.position.y + offsetYArea);
            newObject.transform.position = position; 
            newObject.transform.position += Random.insideUnitSphere * sizeOfObjectArea;
            listOfObjects.Add(newObject);
            CinemachineTargetGroup.Target targetObject = new CinemachineTargetGroup.Target();
            targetObject.target = newObject.transform;
            targetObject.radius = 2f;
            targetObject.weight = 1;
            targets.Add(targetObject);
        }
        targetGroup.m_Targets = targets.ToArray();


    }


    //Start animation and shake things from tree
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && CrossPlatformInputManager.GetButtonDown("Dash") && !isMoving)
        {
            isMoving = true;
            VRCamBig.enabled = true;
            LeanTween.rotateZ(transform.parent.gameObject, 4f, 1f).setEaseInBack().setOnComplete(() =>
            {
                ShakeAndFallObjects();
                LeanTween.rotateZ(transform.parent.gameObject, -4f, 0.2f).setRepeat(5).setLoopPingPong()
                         .setOnComplete(() =>
                         {
                             VRCamBig.enabled = false;
                             VRCamObjectInTree.enabled = true;
                             LeanTween.rotateZ(transform.parent.gameObject, 0f, 1f).setEaseOutBounce()
                                      .setOnComplete(() =>
                                      {
                                          isMoving = false;
                                          StartCoroutine(EnableOriginalCamera(1.5f));
                                      });
                         });
            });
        }
    }

    private void ShakeAndFallObjects()
    {
        foreach(ObjectFallFromTree thing in listOfObjects)
        {
            thing.ShakeAndFall(2f);
        }
    }


    IEnumerator EnableOriginalCamera(float delaySec)
    {
        yield return new WaitForSeconds(delaySec);
        VRCamObjectInTree.enabled = false;
        isMoving = false;
    }



}
