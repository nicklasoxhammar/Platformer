﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    [SerializeField] GameObject objectPoolManagerPrefab;

    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite evilSprite;
    [SerializeField] Sprite happySprite;

    [SerializeField] GameObject dropPrefab;
    [SerializeField] ParticleSystem flashPrefab;
    [SerializeField] GameObject destinations;


    [SerializeField] bool bounceIsOn = true;
    [SerializeField] float speed = 4f;

    [SerializeField] [Range(0, 100)] int evilnessPercent = 50;

    [Header("Time between freeze")]
    [SerializeField] float minTime = 1f;
    [SerializeField] float maxTime = 2f;

    [Header("FreezeTime")]
    [SerializeField] float minFreezeTime = 1f;
    [SerializeField] float maxFreezeTime = 2f;



    [Header("Raindrops")]
    [SerializeField] [Range(0f, 0.5f)] float widthOfRain = 0.3f;
    [SerializeField] float minTimeBetweenDrops = 0.2f;
    [SerializeField] float maxTimeBetweenDrops = 1f;

    private int indexDestination = 0;
    private bool areMovingForward = false;
    private bool freeze = false;
    private float timer;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private ParticleSystem flash;
    private DropObjectPool objectPool;


    private void Awake()
    {
        if (DropObjectPool.instance == null)
        {
            Instantiate(objectPoolManagerPrefab);
        }
    }


    // Use this for initialization
    void Start()
    {
        objectPool = DropObjectPool.instance;

        setNewTimeBetweenFreeze();

        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        flash = Instantiate(flashPrefab, transform.position, Quaternion.LookRotation(Vector2.up));
        flash.transform.parent = transform.parent;

        if (!bounceIsOn)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveHorizontal();
        FreezeCountDown();

        if (freeze)
        {
            animator.enabled = false;
        }
        else
        {
            animator.enabled = true;
        }
    }

    private void MoveHorizontal()
    {
        //Just standing still if no positions.
        if (destinations != null && destinations.transform.childCount > 0 && !freeze)
        {
            Vector3 moveTowards = destinations.transform.GetChild(indexDestination).position;

            transform.position = Vector3.MoveTowards(transform.position, moveTowards, speed * Time.deltaTime);

            if (transform.position == moveTowards)
            {
                //reached your destination
                if (indexDestination == 0)
                {
                    setMovingForwardTo(true);
                }
                else if (indexDestination == destinations.transform.childCount - 1)
                {
                    setMovingForwardTo(false);
                }

                if (areMovingForward)
                {
                    indexDestination++;
                }
                else
                {
                    indexDestination--;
                }
            }
        }
    }

    private void setMovingForwardTo(bool status)
    {
        if (status)
        {
            areMovingForward = true;
            //transform.localScale = new Vector3(1f, 1f, 1f);
            animator.SetBool("GoingForward", true);
        }
        else
        {
            areMovingForward = false;
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            animator.SetBool("GoingForward", false);
        }
    }



    private void setNewTimeBetweenFreeze()
    {
        float randomNumber = Random.Range(minTime, maxTime);
        timer = Mathf.Abs(randomNumber);
    }

    private void FreezeCountDown()
    {
        if (!freeze)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StartCoroutine(Freeze());
            }
        }
    }



    IEnumerator Freeze()
    {
        freeze = true;
        if (IAmEvil())
        {
            SetSpriteTo(evilSprite);

            playFlashVFX();

            StartRain();
        }
        else
        {
            SetSpriteTo(happySprite);
        }

        yield return new WaitForSeconds(GetRandomFreezeTime());
        setRandomIndexDestination();
        setNewTimeBetweenFreeze();
        flash.Stop();
        SetSpriteTo(idleSprite);
        freeze = false;
    }

    private float GetRandomFreezeTime()
    {
        return Random.Range(minFreezeTime, maxFreezeTime);
    }

    private void SetSpriteTo(Sprite sprite)
    {
        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    private void playFlashVFX()
    {
        flash.Clear();
        flash.transform.position = transform.position;
        flash.Play();
    }

    private bool IAmEvil()
    {
        int random = Random.Range(0, 100);
        return random <= evilnessPercent;
    }



    private void StartRain()
    {
        StartCoroutine(InstantiateDrop());
    }

    private Vector3 GetPositionForDrop()
    {
        Vector2 size = spriteRenderer.bounds.max - spriteRenderer.bounds.min;

        float width = size.x;
        float height = size.y;

        float minX = transform.position.x - (width * widthOfRain);
        float maxX = transform.position.x + (width * widthOfRain);

        float yPosition = transform.position.y - (height * 0.4f);

        float randomX = Random.Range(minX, maxX);
        Vector3 randomPos = new Vector3(randomX, yPosition, transform.position.z);
        return randomPos;
    }


    IEnumerator InstantiateDrop()
    {
        while (freeze)
        {
            //GameObject drop = GetDrop();
            GameObject drop = objectPool.GetObjectFromPool();
            if (drop != null)
            {
                drop.transform.position = GetPositionForDrop();
                drop.SetActive(true);
            }

            yield return new WaitForSeconds(getTimeBetweenDrops());
        }
    }



    private float getTimeBetweenDrops()
    {
        return Random.Range(minTimeBetweenDrops, maxTimeBetweenDrops);
    }

    private void setRandomIndexDestination()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            //change direction.
            if (areMovingForward)
            {
                indexDestination--;
                setMovingForwardTo(false);
            }
            else
            {
                indexDestination++;
                setMovingForwardTo(true);
            }
        }
    }
}

