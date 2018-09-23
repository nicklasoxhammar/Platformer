//using UnityEngine;

//public class FollowPlayer : MonoBehaviour {

//    [SerializeField] GameObject player;
//    //[SerializeField] float moveCameraAt = 7.0f;

//    //this is so the camera doesnt move to the players z position
//    //float zPos;

//    bool isLerping = false;

//    private Vector3 targetPos;
//    [SerializeField] float followAhead = 6f;
//    [SerializeField] float followAheadUpAndDown = 4f;
//    [SerializeField] float smoothing = 1f;


//    private float yValue;
//    void Start() {

//        //zPos = transform.position.z;

//        yValue = transform.position.y;

//    }


//    private void Update()

//    {
//        if (player == null)
//        {
//            return;
//        }
//        targetPos = new Vector3(player.transform.position.x, yValue, transform.position.z);


//        isLerping = true;


//        if (player.transform.localScale.x > 0f)
//        {
//            targetPos = new Vector3(targetPos.x + followAhead, targetPos.y, targetPos.z);
//            MoveUpAndDown();
//        }
//        else
//        {
//            targetPos = new Vector3(targetPos.x - followAhead, targetPos.y, targetPos.z);
//            MoveUpAndDown();
//        }





//        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);

//        //transform.position = targetPos;   //Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
       
//    }


//    private void MoveUpAndDown()
//    {

//        //if (player.transform.position.y > transform.position.y + followAheadUpAndDown)
//        //{
//        //    Debug.Log("ÖVER");
//        //    targetPos.y += followAheadUpAndDown;
//        //    yValue = targetPos.y;
//        //    //targetPos = new Vector3(targetPos.x, targetPos.y + followAheadUpAndDown, targetPos.z);
//        //}
//        //else if (player.transform.position.y < transform.position.y - followAheadUpAndDown)
//        //{
//        //    Debug.Log("UNDER");
//        //    targetPos.y -= followAheadUpAndDown;
//        //    yValue = targetPos.y;

//        //    //targetPos = new Vector3(targetPos.x, targetPos.y - followAheadUpAndDown, targetPos.z);

//        //}


//    }









//    private void LateUpdate()
//    {





//    }

//    //FixedUpdate syncs better with player movement
//    void FixedUpdate() {



//        ////move camera to the right
//        //if (player.transform.position.x > transform.position.x + moveCameraAt) {

//        //    transform.position = transform.position = new Vector3(player.transform.position.x - moveCameraAt, transform.position.y, zPos);
//        //}
//        ////move camera to the left
//        //if (player.transform.position.x < transform.position.x - moveCameraAt) {

//        //    transform.position = transform.position = new Vector3(player.transform.position.x + moveCameraAt, transform.position.y, zPos);
//        //}
//        ////move camera up
//        //if (player.transform.position.y > transform.position.y + moveCameraAt) {

//        //    transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y - moveCameraAt, zPos);
//        //}
//        ////move camera down
//        //if (player.transform.position.y < transform.position.y - moveCameraAt) {

//        //    transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y + moveCameraAt, zPos);
//        //}




//        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothing);

//    }
//}

