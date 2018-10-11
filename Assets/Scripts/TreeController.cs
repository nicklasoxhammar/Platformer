using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;

[System.Serializable]
public class ItemInTree
{
    public ObjectFallFromTree objectPrefab;
    public int numberOfItems;
}


public class TreeController : MonoBehaviour
{
    [SerializeField] List<ItemInTree> objectsToPutInTree = new List<ItemInTree>();

    [Space(20)]
    [Header("Tree")]
    //[SerializeField] int numberOfObjects = 2;
    [SerializeField] int sizeOfObjectArea = 8;
    [SerializeField] float offsetYArea = 4f;
    [SerializeField][Range(2, 100)] int ShakeBeforeFall = 3;
    [SerializeField] float deathAngle = 70f;
    [SerializeField] float waitBeforeDrop = 1.5f;
    [SerializeField] [Range(0, 5)] float dropRandomness = 0;

    [Header("Cameras")]
    [SerializeField] CinemachineVirtualCamera VRCamBig;
    [SerializeField] CinemachineVirtualCamera VRCamObjectInTree;
    [SerializeField] CinemachineTargetGroup targetGroup;
    private bool isMoving;
    private int shakeCounter = 0;
    private bool treeIsDead = false;
    private bool objectsIsFallen = false;

    private List<ObjectFallFromTree> listOfObjects = new List<ObjectFallFromTree>();

    // Use this for initialization
    void Start()
    {
        List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>();

        foreach (ItemInTree treeObject in objectsToPutInTree)
        {
            for (int i = 0; i < treeObject.numberOfItems; i++)
            {
                ObjectFallFromTree newObject = Instantiate(treeObject.objectPrefab, transform).GetComponent<ObjectFallFromTree>();
                //Change position to match tree...
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
            //SHAKE TREE WITH PING PONG LOOP
            LeanTween.rotateZ(transform.parent.gameObject, -4f, 0.2f).setRepeat(5).setLoopPingPong()
                     .setOnComplete(() =>
                     {
                         if (!objectsIsFallen && targetGroup.m_Targets.Length != 0)
                         {
                             FirstTimeShake();
                         }
                else if (objectsIsFallen && shakeCounter < ShakeBeforeFall|| 
                                  targetGroup.m_Targets.Length == 0 && shakeCounter < ShakeBeforeFall)
                         {
                             EveryOtherTimeShake();
                         }

                         else
                         {
                             ShakeToDeath();
                         }
                     });
        });
        shakeCounter++;
    }

    private void FirstTimeShake()
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

    private void EveryOtherTimeShake()
    {
        LeanTween.rotateZ(transform.parent.gameObject, 0f, 1f).setEaseOutBounce().setOnComplete(() =>
        {
            VRCamBig.enabled = false;
            isMoving = false;
        });
    }

    private void ShakeToDeath()
    {
        treeIsDead = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        VRCamBig.enabled = false;
        LeanTween.rotateZ(transform.parent.gameObject, deathAngle, 2f).setEaseOutBounce().setOnComplete(() =>
        {
            isMoving = false;
            //Set Layer to Ground so player can jump on it.
            gameObject.layer = 9;
        });
    }

    private void ShakeAndFallObjects()
    {
        if (!objectsIsFallen)
        {
            foreach (ObjectFallFromTree thing in listOfObjects)
            {
                thing.ShakeAndFall(GetRandomDropTime());
            }
        }
    }

    private void ChangeParentToAvoidMoreShake()
    {
        foreach (ObjectFallFromTree thing in listOfObjects)
        {
            thing.transform.parent = transform.parent.parent;
        }
    }


    private float GetRandomDropTime()
    {
        return Random.Range(waitBeforeDrop, waitBeforeDrop + dropRandomness);
    }

}
