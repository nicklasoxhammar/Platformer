using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Spine.Unity;

public class IntroScene : MonoBehaviour
{

    public DialogueSystem dialogueSystem;
    public DelayLetters startText;
    public DelayLetters disconnectEarthText;
    public BubbleTextController bubbleSunHeroTalks;
    public BubbleTextController bubblePresidentTalks;
    public CanvasGroup yellowSquare;
    public CanvasGroup whiteSquare;
    public CanvasGroup blackSquare;
    public GameObject sunGround;
    public GameObject sunAtBeginning;
    public GameObject earth;
    public GameObject sunHero;
    public GameObject skateboard;
    public GameObject president;
    public ParticleSystem StartComputerVFX;
    public Transform skatePosition1;
    public Transform skatePosition2;


    [Header("VR Cameras")]
    public CinemachineVirtualCamera cameraStartSun;
    public CinemachineVirtualCamera cameraSunClose;
    public CinemachineVirtualCamera cameraAtTheSun;
    public CinemachineVirtualCamera cameraPresidentAndComputen;
    public CinemachineVirtualCamera cameraComputerClose;
    public CinemachineVirtualCamera cameraSkate;

    //Animation
    private SkeletonAnimation sunHeroSkeleton;
    private SkeletonAnimation presidentSkeleton;
    private string runSunHero = "RUN";
    private string idleSunHero = "STANDING";
    private string runPresident = "PresidentRun";
    private string idlePresident = "PresidentIdle";

    //Text and Dialouge stuff...
    private float timeBeforeFadeOutText = 0.45f;
    private float timeBetweenLettersStartText = 0.12f;
    private float timeBetweenLettersDialogueText = 0.04f;
    private float timeBetweenLettersComputerText = 0.05f;
    private bool isPrintingText = true;
    //Computer stuff...
    private string textToComputer = "foreach(sunRayToEarth sunRay in sun)@{@sunRay.SetActive(false);@}";
    private float delayStartCodingAtComputer = 2.5f;
    private float timeBeforeFadeoutComputerText = 0.5f;
    //Move character stuff...
    private float secToWaitBeforeSunHeroMoves = 0.5f;
    private float distanceMoveSunHero = -13f;
    private float distanceMovePresident = -10f;
    //low value = FASTER..
    private float sunHeroMovingSpeed = 0.02f;
    private float presidentMovingSpeed = 0.02f;
    //Skate stuff...
    private float timeSkateFirstPosition = 2f;
    private float timeSkateLastPosition = 2f;
    private string sunHeroSkateText = "I HAVE TO SAVE THE EARTH!";
    private float timeBeforeFadeOutSkateText = 1.5f;
    //Other stuff...
    private float delayBeforeFadeOutEarth = 3f;
    private float secShowEarthWhenItsBlack = 1f;
    private float secBeforeFadeInSun = 0.5f;


    // Use this for initialization
    void Start()
    {
        sunHeroSkeleton = sunHero.GetComponent<SkeletonAnimation>();
        presidentSkeleton = president.GetComponent<SkeletonAnimation>();
        LeanTween.rotateAround(sunAtBeginning, Vector3.forward, 360, 150f).setLoopClamp();

        startText.SetTextTo("Yesterday...", timeBetweenLettersStartText, secBeforeFadeInSun, true);
        StartCoroutine(ShowCamerasAtTheSun(2.5f, 2f));
    }


    IEnumerator ShowCamerasAtTheSun(float delayFirstCamera, float timeShowCloseCamera)
    {
        //Camera zoom in at the sun
        yield return new WaitForSeconds(delayFirstCamera);
        cameraSunClose.enabled = true;
        cameraStartSun.enabled = false;

        //Camera at the sun...
        yield return new WaitForSeconds(timeShowCloseCamera);
        cameraAtTheSun.enabled = true;
        cameraSunClose.enabled = false;
        yellowSquare.alpha = 1;
        LeanTween.alphaCanvas(yellowSquare, 0f, 2.5f).setEaseOutQuad();
        //Move sun ground.
        LeanTween.scaleY(sunGround, 1.1f, 5f).setEaseInOutSine().setLoopPingPong();
        yield return new WaitForSeconds(secToWaitBeforeSunHeroMoves);
        MoveSunHeroAndStartDialogue();
    }

    private void MoveSunHeroAndStartDialogue()
    {
        sunHeroSkeleton.AnimationName = runSunHero;
        sunHero.transform.localScale = new Vector3(-1f, 1f, 1f);
        LeanTween.moveX(sunHero, sunHero.transform.position.x + distanceMoveSunHero, sunHeroMovingSpeed / Time.deltaTime).setOnComplete(() =>
        {
            sunHeroSkeleton.AnimationName = idleSunHero;
            //presidentSkeleton.state.AddAnimation(2, flaxEarsName, true, 0);
            StartCoroutine(StartDialouge());
        });
    }

    private void MovePresidentToComputer()
    {
        presidentSkeleton.state.SetAnimation(1, runPresident, true);
        president.transform.localScale = new Vector3(-1f, 1f, 1f);
        LeanTween.moveX(president, president.transform.position.x + distanceMovePresident, presidentMovingSpeed / Time.deltaTime).setOnComplete(() =>
        {
            presidentSkeleton.state.SetAnimation(1, idlePresident, true);
            StartComputerVFX.Play();
            cameraComputerClose.enabled = true;
            cameraPresidentAndComputen.enabled = false;
            StartCoroutine(WaitAndDisconnectEarth());
        });
    }

    IEnumerator WaitAndDisconnectEarth()
    {
        yield return new WaitForSeconds(delayStartCodingAtComputer);
        disconnectEarthText.SetTextTo(textToComputer, timeBetweenLettersComputerText, timeBeforeFadeoutComputerText, false);
        yield return new WaitUntil(() => isPrintingText == false);
        isPrintingText = true;
        sunAtBeginning.SetActive(false);
        earth.SetActive(true);
        //WhiteSquare for fade..
        LeanTween.alphaCanvas(whiteSquare, 1f, 1f).setEaseInQuad().setOnComplete(() =>
         {
             disconnectEarthText.ResetText();
             cameraSkate.enabled = true;
             cameraComputerClose.enabled = false;
             LeanTween.alphaCanvas(whiteSquare, 0f, 1f).setEaseOutQuad();
             //Make Earth Black...
             LeanTween.color(earth, Color.black, 1f).setDelay(delayBeforeFadeOutEarth).setEaseOutExpo().setOnComplete(() =>
              {
                  //StartCoroutine(ShowBlackFade());
                  ShowSunClose();
              });
         });
    }

    //IEnumerator ShowBlackFade()
    //{
    //    yield return new WaitForSeconds(secShowEarthWhenItsBlack);
    //    LeanTween.alphaCanvas(blackSquare, 1f, 0.8f).setOnComplete(() =>
    //    {
    //        //Fade In done..do stuff...
    //        earth.SetActive(false);
    //        sunAtBeginning.SetActive(true);
    //        LeanTween.alphaCanvas(blackSquare, 0f, 0.8f).setOnComplete(() => 
    //        {
    //            //fade out done..
    //            StartSkateToEarth();  
    //        });
    //    });
    //}

    private void ShowSunClose()
    {
        LeanTween.alphaCanvas(yellowSquare, 1f, 0.8f).setDelay(secShowEarthWhenItsBlack).setOnComplete(() =>
        {
            cameraSunClose.enabled = true;
            sunAtBeginning.SetActive(true);
            //cameraStartSun.enabled = false;
            cameraSkate.enabled = false;

            LeanTween.alphaCanvas(yellowSquare, 0f, 0.5f);
            earth.SetActive(false);
            StartCoroutine(ZoomOutFromSun());
        });
    }


    IEnumerator ZoomOutFromSun()
    {
        yield return new WaitForSeconds(1f);
        cameraSkate.enabled = true;
        yield return new WaitForSeconds(2f);
        StartSkateToEarth();
    }



    private void StartSkateToEarth()
    {
        LeanTween.rotateZ(sunHero, 10f, 3f).setLoopPingPong(5);
        cameraSkate.enabled = true;
        cameraStartSun.enabled = false;
        skateboard.SetActive(true);
        sunHero.GetComponent<Rigidbody2D>().isKinematic = true;
        sunHero.transform.position = sunAtBeginning.transform.position;
        sunHero.transform.localScale = Vector3.zero;
        sunHeroSkeleton.AnimationName = runSunHero;
        LeanTween.scale(sunHero, new Vector3(1f, 1f, 1f), timeSkateFirstPosition);
        LeanTween.move(sunHero, skatePosition1, timeSkateFirstPosition).setEaseInQuad().setOnComplete(() =>
        {
            sunHeroSkeleton.AnimationName = idleSunHero;
            StartCoroutine(SunHeroTalksOnSkateboard());
        });

    }

    IEnumerator SunHeroTalksOnSkateboard()
    {
        bubbleSunHeroTalks.SetOffsetTo(new Vector3(-4.5f, -6f, 0f));
        bubbleSunHeroTalks.ShowBubbleAndPrintText(sunHeroSkateText, timeBetweenLettersDialogueText, timeBeforeFadeOutSkateText, true);
        yield return new WaitUntil(() => isPrintingText == false);
        bubbleSunHeroTalks.Hide();
        sunHeroSkeleton.AnimationName = runSunHero;
        LeanTween.scale(sunHero, new Vector3(5f, 5f, 5f), timeSkateFirstPosition);
        LeanTween.move(sunHero, skatePosition2, timeSkateLastPosition).setEaseInExpo().setOnComplete(() =>
        {
            //LOAD NEXT SCENE?
            //SceneManager.LoadScene(1); 
        });
    }


    IEnumerator StartDialouge()
    {
        //StartIsPrintingCheck();
        for (int i = 0; i < dialogueSystem.GetSize(); i++)
        {
            isPrintingText = true;
            DialogueText newDialogue = dialogueSystem.GetNextDialogue();
            if (newDialogue.name == DialogueText.Name.Elda)
            {
                bubbleSunHeroTalks.ShowBubbleAndPrintText(newDialogue.text, timeBetweenLettersDialogueText, timeBeforeFadeOutText, !newDialogue.textcontinuing);
            }
            else
            {
                bubblePresidentTalks.ShowBubbleAndPrintText(newDialogue.text, timeBetweenLettersDialogueText, timeBeforeFadeOutText, !newDialogue.textcontinuing);
            }
            yield return new WaitUntil(() => isPrintingText == false);
        }
        //Dialouge is done.. move to computer.
        isPrintingText = true;
        MovePresidentToComputer();
        bubbleSunHeroTalks.Hide();
        bubblePresidentTalks.Hide();
        cameraPresidentAndComputen.enabled = true;
        cameraAtTheSun.enabled = false;
    }

    //called from delayletters.remember to set it to true...
    public void SetIsPrintingTo(bool status)
    {
        isPrintingText = status;
    }
}
