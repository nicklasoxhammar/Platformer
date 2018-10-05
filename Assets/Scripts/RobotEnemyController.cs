using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class RobotEnemyController : MonoBehaviour
{

    //Changes direction when touch collider with tag "Block"

    [SerializeField] Collider2D playerToFollow;
    [SerializeField] [Header("Movement:")] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private bool makeRandomDirectionChanges = true;
    [SerializeField] private float minTimeBeforeChangeDirection = 4f;
    [SerializeField] private float maxTimeBeforeChangeDirection = 10f;
    private float changeDirectionTimer = 0f;
    private float timeBeforeChangeDirection;

    //LASER:
    [SerializeField][Header("LaserShoot:")] GameObject LaserPrefab;
    [SerializeField] float minTimeBetweenLaserShoot = 1f;
    [SerializeField] float maxTimeBetweenLaserShoot = 3f;
    [SerializeField] int poolSize = 10;
    private List<GameObject> laserPool;
    [SerializeField] ParticleSystem dieVFX;
    //ANIMATION
    private SkeletonAnimation skeletonAnimation;
    Bone pupil;
    Bone eye;
    Vector3 eyePos;
    Vector3 targetPos;
    private string pupilBoneName = "Pupill";
    private string eyeBoneName = "EYE";
    private string walkAnimationName = "WALK";
    private string dieAnimationName = "DIE";
    private string runAnimationName = "RUN";

    private bool isDead = false;
    private bool isFreezed = false;
    private bool playerInSight = false;
    private int direction = 1;
    private bool isShooting = false;

    // Use this for initialization
    void Start()
    {
        if (playerToFollow == null) { return; }

        InitLaserPool();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        pupil = skeletonAnimation.skeleton.FindBone(pupilBoneName);
        eye = skeletonAnimation.skeleton.FindBone(eyeBoneName);
        skeletonAnimation.AnimationState.Complete += AnimationCompleteListener;
        ChangeAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerAreInSight();
        SetDirectionToFollowPlayer();
        MakeEyeFollowPlayer();
        ShootLaserIfPlayerInSight();
        StartTurnTimer();
        //Change Speed
        if (playerInSight)
        {
            MoveWithThisSpeed(runSpeed);
        }
        else
        {
            MoveWithThisSpeed(walkSpeed);
        }

        //Face the direction....but not at the edge.
        if (!isFreezed && !isDead)
        { 
        transform.localScale = new Vector3(direction, 1f, 1f);
        }

        SnapOutOfFreezeWhenJumpOver();
    }

    private void StartTurnTimer()
    {
        if(makeRandomDirectionChanges && !playerInSight)
        {
            timeBeforeChangeDirection -= Time.deltaTime;
            if (timeBeforeChangeDirection <= 0)
            {
                direction *= -1;
                ResetChangeDirectionTimer();
            }
        }
    }

    private void ResetChangeDirectionTimer()
    {
        timeBeforeChangeDirection = Random.Range(minTimeBeforeChangeDirection, maxTimeBeforeChangeDirection);
        changeDirectionTimer = 0;
    }


    //Calls from Update
    private void MoveWithThisSpeed(float speed)
    {
        if (isDead != true && !isFreezed)
        {
            Vector2 position = transform.position;
            position.x += speed * direction * Time.deltaTime;
            transform.position = position;
        }
    }


    private void CheckIfPlayerAreInSight()
    {
        eyePos = pupil.GetWorldPosition(skeletonAnimation.transform);
        targetPos = playerToFollow.bounds.center;
        RaycastHit2D hit = Physics2D.Linecast(eyePos, targetPos);

        //DEBUG THING:..
        Debug.DrawLine(eyePos, targetPos);
        if (hit != false){
            Debug.DrawLine(eyePos, hit.point, Color.red);

        }
       ////
        /// 
        // HITS PLAYER         if (hit != false && hit.collider.tag == "Player" && !isDead)
        {
            if(!playerInSight)
            {
                playerInSight = true;
                ChangeAnimation();
            }
        }
        else if (hit != false && hit.collider.tag != "Player" && !isDead && playerInSight)
        {
            playerInSight = false;
            isFreezed = false;
            ChangeAnimation();
        }
    }

    //Called from Update
    private void SetDirectionToFollowPlayer()
    {
        if (!isFreezed && playerInSight)
        {
            //Direction follow the player...
            float offset = 3f;

            if (targetPos.x < eyePos.x - offset)
            {
                direction = -1;
            }
            else if (targetPos.x > eyePos.x + offset)
            {
                direction = 1;
            }
        }
    }

    private void ChangeAnimation()
    {
        if(!isFreezed && !isDead)
        {
            if(playerInSight)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, runAnimationName, true);
            }
            else
            {
                skeletonAnimation.AnimationState.SetAnimation(0, walkAnimationName, true);
            }
        }
    }

    //Called from Update
    private void SnapOutOfFreezeWhenJumpOver()
    {
        if(isFreezed && targetPos.x > eyePos.x && direction == 1 || isFreezed && targetPos.x < eyePos.x && direction == -1)
        {
            isFreezed = false;
            ChangeAnimation();
        }
    }

    //Called from Update
    private void MakeEyeFollowPlayer()
    {
        if (playerInSight)
        {
            float LowerRotationBound = -55.0f;
            float UpperRotationBound = 55.0f;
            Vector3 tempVec = targetPos - eyePos;
            float tempRot = Mathf.Atan2(tempVec.y, tempVec.x * transform.localScale.x) * Mathf.Rad2Deg;
            eye.Rotation = Mathf.Clamp(tempRot, LowerRotationBound, UpperRotationBound);
        }
        else
        {
            eye.Rotation = 0;
        }
    }


    //Freeze at edge or just change direction.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Block")
        {
            if (playerInSight)
            {
                isFreezed = true;
                skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0.5f, 0f);
            }
                direction *= -1;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.isDashing)
            {
                Die();
            }
            else if(!isDead)
            {
                player.Die();
            }
        }
    }



    private void Die()
    {
        isDead = true;
        ChangeSizeOfColliderWhenDead();
        //Die Animation and trigger when its done..
        skeletonAnimation.AnimationState.SetAnimation(0, dieAnimationName, false);
    }

    //When DIE-animation is completed...
    private void AnimationCompleteListener(TrackEntry trackEntry)
    {
        if (trackEntry.animation.Name == dieAnimationName)
        {
            //Dust Effect
            if (dieVFX != null)
            {
                ParticleSystem vfx = Instantiate(dieVFX, transform);
                Destroy(vfx, vfx.main.duration);
            }
            //Fade away and destroy
            LeanTween.value(1f, 0f, dieVFX.main.duration).setEaseOutCubic().setOnUpdate((float val) => {
                skeletonAnimation.skeleton.a = val;
            }).setOnComplete(() => {
                Destroy(gameObject);
            });
        }
    }

    private void PlayVFX()
    {
        if (dieVFX != null)
        {
            ParticleSystem vfx = Instantiate(dieVFX, transform);
            Destroy(vfx, vfx.main.duration);
        }
    }

    //Change size, player can jump over it when its dead.
    private void ChangeSizeOfColliderWhenDead()
    {
        BoxCollider2D thisCollider = GetComponent<BoxCollider2D>();
        Vector2 size = thisCollider.size;
        size.y *= 0.1f;
        thisCollider.size = size;
        Vector2 colliderOffset = thisCollider.offset;
        colliderOffset.y *= 0.1f;
        thisCollider.offset = colliderOffset;
    }





    //LASER STUFF

    private void InitLaserPool()
    {
        laserPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newLaser = Instantiate(LaserPrefab);
            newLaser.SetActive(false);
            laserPool.Add(newLaser);
        }
    }

    private void ShootLaserIfPlayerInSight()
    {
        if(playerInSight && !isDead && !isShooting)
        {
            isShooting = true;
            StartCoroutine(InstantiateLaser());
        }
        else if (!playerInSight || isDead)
        {
            isShooting = false;
        }
    }

    IEnumerator InstantiateLaser()
    {
        while (isShooting)
        {
            yield return new WaitForSeconds(GetRandomTimeBetweenShoots());
            if (!isDead)
            {
                GameObject laser = GetLaserFromPool();
                if (laser != null)
                {
                    laser.transform.position = eyePos;
                    laser.SetActive(true);
                }
            }
        }
    }


    private float GetRandomTimeBetweenShoots()
    {
        return Random.Range(minTimeBetweenLaserShoot, maxTimeBetweenLaserShoot);
    }


    private GameObject GetLaserFromPool()
    {
        for (int i = 0; i < laserPool.Count; i++)
        {
            if(!laserPool[i].gameObject.activeInHierarchy)
            {
                return laserPool[i];
            }
        }
        return null;
    }
}
