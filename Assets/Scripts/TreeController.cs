using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;


public class TreeController : MonoBehaviour
{
    [SerializeField] GameObject objectToPutInTreePrefab;
    [SerializeField] int numberOfObjects = 2;
    [SerializeField] int sizeOfObjectArea = 8;
    [SerializeField] float offsetYArea = 4f;
    [SerializeField] int ShakeBeforeFall = 3;
    [SerializeField] float deathAngle = 70f;

    [SerializeField] CinemachineVirtualCamera VRCamBig;
    [SerializeField] CinemachineVirtualCamera VRCamObjectInTree;
    [SerializeField] CinemachineTargetGroup targetGroup;
    private bool isMoving;
    private int shakeCounter = 0;
    private bool treeIsDead = false;
    private bool objectsIsFallen = false;

    private List<ObjectFallFromTree> listOfObjects = new List<ObjectFallFromTree>();

    //CinemachineTargetGroup targetGroup;
    // Use this for initialization
    void Start()
    {
        //targetGroup = GameObject.Find("TargetGroup2").GetComponent<CinemachineTargetGroup>();

        List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            ObjectFallFromTree newObject = Instantiate(objectToPutInTreePrefab, transform).GetComponent<ObjectFallFromTree>();
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
        if (collision.tag == "Player" && CrossPlatformInputManager.GetButtonDown("Dash") && !isMoving && !treeIsDead)
        {
            isMoving = true;

            ShakeTree();

        }
    }


    private void ShakeTree()
    {
        VRCamBig.enabled = true;
        LeanTween.rotateZ(transform.parent.gameObject, 4f, 1f).setEaseInBack().setOnComplete(() =>
        {
            //SHAKE OBJECT
            ShakeAndFallObjects();
            LeanTween.rotateZ(transform.parent.gameObject, -4f, 0.2f).setRepeat(5).setLoopPingPong()
                     .setOnComplete(() =>
                     {
                         //MAKE OBJECT FALL....
                         if (!objectsIsFallen)
                         {
                             VRCamBig.enabled = false;
                             VRCamObjectInTree.enabled = true;
                             objectsIsFallen = true;
                             LeanTween.rotateZ(transform.parent.gameObject, 0f, 1f).setEaseOutBounce()
                           .setOnComplete(() =>
                           {
                               isMoving = false;
                               VRCamObjectInTree.enabled = false;
                               ChangeParentToAvoidMoreShake();
                           });
                         }
                         else if (objectsIsFallen && shakeCounter < ShakeBeforeFall)
                         {
                             objectsIsFallen = true;
                             LeanTween.rotateZ(transform.parent.gameObject, 0f, 1f).setEaseOutBounce()
                           .setOnComplete(() =>
                           {
                               VRCamBig.enabled = false;
                               isMoving = false;

                           });
                         }
                         else
                         {
                             treeIsDead = true;
                             VRCamBig.enabled = false;
                             LeanTween.rotateZ(transform.parent.gameObject, deathAngle, 2f).setEaseOutBounce().setOnComplete(() =>
                             {
                                isMoving = false;

                             });
                         }
                     });
        });

        shakeCounter++;
    }

    private void ChangeParentToAvoidMoreShake()
    {
        foreach (ObjectFallFromTree thing in listOfObjects)
        {
            thing.transform.parent = transform.parent.parent;
        }

    }

    private void ShakeToDeath()
    {
        treeIsDead = true;
        LeanTween.rotateZ(transform.parent.gameObject, deathAngle, 1f).setEaseOutBounce().setOnComplete(() =>
        {
            VRCamBig.enabled = false;
        });
    }



    private void ShakeAndFallObjects()
    {
        if (!objectsIsFallen)
        {
            foreach (ObjectFallFromTree thing in listOfObjects)
            {
                thing.ShakeAndFall(1.2f);
            }
        }
    }




}
