using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    [SerializeField] GameObject player;
    //[SerializeField] float moveCameraAt = 7.0f;

    //this is so the camera doesnt move to the players z position
    //float zPos;

    bool isLerping = false;

    private Vector3 targetPos;
    [SerializeField] float followAhead = 6f;
    [SerializeField] float followAheadUpAndDown = 4f;
    [SerializeField] float smoothing = 1f;

    void Start() {

        //zPos = transform.position.z;

    }

    //FixedUpdate syncs better with player movement
    void FixedUpdate() {

        if (player == null) {
            return;
        }

        ////move camera to the right
        //if (player.transform.position.x > transform.position.x + moveCameraAt) {

        //    transform.position = transform.position = new Vector3(player.transform.position.x - moveCameraAt, transform.position.y, zPos);
        //}
        ////move camera to the left
        //if (player.transform.position.x < transform.position.x - moveCameraAt) {

        //    transform.position = transform.position = new Vector3(player.transform.position.x + moveCameraAt, transform.position.y, zPos);
        //}
        ////move camera up
        //if (player.transform.position.y > transform.position.y + moveCameraAt) {

        //    transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y - moveCameraAt, zPos);
        //}
        ////move camera down
        //if (player.transform.position.y < transform.position.y - moveCameraAt) {

        //    transform.position = transform.position = new Vector3(transform.position.x, player.transform.position.y + moveCameraAt, zPos);
        //}


        targetPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

        isLerping = true;


        if (player.transform.localScale.x > 0f) {
            targetPos = new Vector3(targetPos.x + followAhead, targetPos.y, targetPos.z);
        }
        else {
            targetPos = new Vector3(targetPos.x - followAhead, targetPos.y, targetPos.z);
        }

        if (player.transform.position.y > transform.position.y + followAheadUpAndDown) {
            targetPos = new Vector3(targetPos.x, targetPos.y + followAheadUpAndDown, targetPos.z);
        }
        else if (player.transform.position.y < transform.position.y - followAheadUpAndDown) {
            targetPos = new Vector3(targetPos.x, targetPos.y - followAheadUpAndDown, targetPos.z);

        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothing);


    }
}

