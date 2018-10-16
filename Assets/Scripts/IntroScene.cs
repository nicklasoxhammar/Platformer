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
    public GameObject sunGround;
    public GameObject sunAtBeginning;
    public GameObject earth;
    public GameObject Sun;
    public GameObject sunHero;
    public GameObject president;
    public ParticleSystem StartComputerVFX;

    [Header("VR Cameras")]
    public CinemachineVirtualCamera cameraStartSun;
    public CinemachineVirtualCamera cameraSunClose;
    public CinemachineVirtualCamera cameraAtTheSun;
    public CinemachineVirtualCamera cameraPresidentAndComputen;
    public CinemachineVirtualCamera computerClose;

    //Animation
    private SkeletonAnimation sunHeroSkeleton;
    private SkeletonAnimation presidentSkeleton;
    private string runSunHero = "RUN";
    private string idleSunHero = "STANDING";
    private string runPresident = "PresidentRun";
    private string idlePresident = "PresidentIdle";

    //Text and Dialouge stuff...
    private float timeBeforeFadeOutText = 0.5f;
    private float timeBetweenLettersStartText = 5f;
    private float timeBetweenLettersDialogueText = 3f;
    private float timeBetweenLettersComputerText = 3f;
    private bool isPrintingText = true;
    //Computer stuff...
    private string textToComputer = "foreach(sunRayToEarth sunRay in sun)@{@sunRay.SetActive(false);@}";
    private float delayStartCodingAtComputer = 2.5f;
    private float timeBeforeFadeoutComputerText = 1.5f;
    //Move character stuff...
    private float distanceMoveSunHero = -13f;
    private float distanceMovePresident = -10f;
    private float sunHeroMovingSpeed = 55f;
    private float presidentMovingSpeed = 70f;
    //Other stuff...
    private float delayBeforeFadeOutEarth = 4f;
    private float secShowEarthWhenItsBlack = 2f;


    // Use this for initialization
    void Start()
    {
        sunHeroSkeleton = sunHero.GetComponent<SkeletonAnimation>();
        presidentSkeleton = president.GetComponent<SkeletonAnimation>();

        startText.SetTextTo("Yesterday...", timeBetweenLettersStartText, 2f, true);

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
        LeanTween.alphaCanvas(yellowSquare, 0f, 3f).setEaseOutQuad().setOnComplete(() =>
        {
            //Starts the dialouge
            MoveSunHeroAndStartDialogue();
        });
        //The sun ground is moving...
        LeanTween.scaleY(sunGround, 1.1f, 5f).setEaseInOutSine().setLoopPingPong();
    }

    private void MoveSunHeroAndStartDialogue()
    {
        sunHeroSkeleton.AnimationName = runSunHero;
        sunHero.transform.localScale = new Vector3(-1f, 1f, 1f);
        LeanTween.moveX(sunHero, sunHero.transform.position.x + distanceMoveSunHero, sunHeroMovingSpeed * Time.deltaTime).setOnComplete(() =>
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
        LeanTween.moveX(president, president.transform.position.x + distanceMovePresident, presidentMovingSpeed * Time.deltaTime).setOnComplete(() =>
        {
            presidentSkeleton.state.SetAnimation(1, idlePresident, true);
            StartComputerVFX.Play();
            computerClose.enabled = true;
            cameraPresidentAndComputen.enabled = false;
            StartCoroutine(WaitAndDisconnectEarth());
        });
    }

    IEnumerator WaitAndDisconnectEarth()
    {
        yield return new WaitForSeconds(delayStartCodingAtComputer);
        disconnectEarthText.SetTextTo(textToComputer, timeBetweenLettersComputerText, timeBeforeFadeoutComputerText, false);

        yield return new WaitUntil(() => isPrintingText == false);
        sunAtBeginning.SetActive(false);
        earth.SetActive(true);
        //WhiteSquare for fade..
        LeanTween.alphaCanvas(whiteSquare, 1f, 2f).setEaseInQuad().setOnComplete(() =>
         {
             disconnectEarthText.ResetText();
             cameraStartSun.enabled = true;
             computerClose.enabled = false;

            LeanTween.alphaCanvas(whiteSquare, 0f, 2f).setEaseOutQuad();

            LeanTween.color(earth, Color.black, 1f).setDelay(delayBeforeFadeOutEarth).setEaseOutExpo().setOnComplete(() =>
             {
                Invoke("StartSkateToEarth", secShowEarthWhenItsBlack);
             });
         });
    }


    private void StartSkateToEarth()
    {
        Debug.Log("DONE");
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

    //called from delayletters.
    public void SetIsPrintingTo(bool status)
    {
        isPrintingText = status;
    }
}
