using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Cinemachine;
public class RobotEnemyController : MonoBehaviour
{

    //Changes direction when touch collider with tag "Block"
    [SerializeField] GameObject objectPoolManagerPrefab;
    [SerializeField] Collider2D playerToFollow;
    [SerializeField] [Header("Movement:")] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private bool makeRandomDirectionChanges = true;
    [SerializeField] private float minTimeBeforeChangeDirection = 4f;
    [SerializeField] private float maxTimeBeforeChangeDirection = 10f;
    private float timeBeforeChangeDirection;

    //LASER:
    [SerializeField][Header("LaserShoot:")] GameObject LaserPrefab;
    [SerializeField] float minTimeBetweenLaserShoot = 1f;
    [SerializeField] float maxTimeBetweenLaserShoot = 3f;
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
    public bool isFreezed = false;
    private bool playerInSight = false;
    private int direction = 1;
    private bool isShooting = false;

    private LaserObjectPool objectPool;
    private bool isSeenByTheCamera = false;
    [SerializeField] [Range(0.0f, 1.0f)] float increaseSightOutsideCamera;


    //private void Reset()
    //{
    //    playerToFollow = FindObjectOfType<PlayerController>().GetComponent<Collider2D>();
    //}
    private void Awake()
    {
        if (LaserObjectPool.instance == null)
        {
            Instantiate(objectPoolManagerPrefab);
        }
    }
    // Use this for initialization
    void Start()
    {
        objectPool = LaserObjectPool.instance;
        playerToFollow = FindObjectOfType<PlayerController>().GetComponent<Collider2D>();

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        pupil = skeletonAnimation.skeleton.FindBone(pupilBoneName);
        eye = skeletonAnimation.skeleton.FindBone(eyeBoneName);
        skeletonAnimation.AnimationState.Complete += AnimationCompleteListener;
        ChangeAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfEnemyIsSeenByCamera();
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

    private void CheckIfEnemyIsSeenByCamera()
    {
        Vector2 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        float minPoint = 0 - increaseSightOutsideCamera;
        float maxPoint = 1 + increaseSightOutsideCamera;
        if(screenPoint.x < maxPoint && screenPoint.x > minPoint && screenPoint.y < maxPoint && screenPoint.y > minPoint)
        {
            isSeenByTheCamera = true;
        }
        else
        {
            isSeenByTheCamera = false;
        }

    }

    private void StartTurnTimer()
    {
        if(makeRandomDirectionChanges && !playerInSight && !isFreezed)
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
    }


    //Calls from Update
    private void MoveWithThisSpeed(float speed)
    {
        if (!isDead && !isFreezed)
        {
            Vector2 position = transform.position;
            position.x += speed * direction * Time.deltaTime;
            transform.position = position;
        }
    }

    //called from update
    private void CheckIfPlayerAreInSight()
    {
        if (isSeenByTheCamera)
        {
            eyePos = pupil.GetWorldPosition(skeletonAnimation.transform);
            targetPos = playerToFollow.bounds.center;
            RaycastHit2D hit = Physics2D.Linecast(eyePos, targetPos);

            // HITS PLAYER
            if (hit != false && hit.collider.tag == "Player" && !isDead)
            {
                if (!playerInSight)
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
            // so the enemy not change direction close to the edge:
            timeBeforeChangeDirection += 1;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.isDashing && !isDead)
            {
                Die();
            }
        }
        else if(collision.gameObject.tag == "KillsEnemy" && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        //Ignore player layer.
        gameObject.layer = 11;
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
                //GameObject laser = GetLaserFromPool();
                GameObject laser = objectPool.GetObjectFromPool();
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

}
