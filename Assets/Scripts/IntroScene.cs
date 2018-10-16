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
    public GameObject sunHero;
    public GameObject president;
    public ParticleSystem StartComputerVFX;

    public CinemachineVirtualCamera cameraStartSun;
    public CinemachineVirtualCamera cameraSunClose;
    public CinemachineVirtualCamera cameraAtTheSun;
    public CinemachineVirtualCamera cameraPresidentAndComputen;
    public CinemachineVirtualCamera computerClose;

    private SkeletonAnimation sunHeroSkeleton;
    private SkeletonAnimation presidentSkeleton;
    private string runSunHero = "RUN";
    private string idleSunHero = "STANDING";
    private string runPresident = "PresidentRun";
    private string idlePresident = "PresidentIdle";
    private string flaxEarsName = "FlaxEars";


    private float timeBeforeFadeOutText = 1.5f;
    private float timeBetweenLettersStartText = 5f;
    private float timeBetweenLettersDialogueText = 3f;
    private float timeBetweenLettersComputerText = 3f;

    // Use this for initialization
    void Start()
    {

        sunHeroSkeleton = sunHero.GetComponent<SkeletonAnimation>();
        presidentSkeleton = president.GetComponent<SkeletonAnimation>();

        startText.SetTextTo("Yesterday...", timeBetweenLettersStartText, 2f, true);

        StartCoroutine(DelayAndShowCameraCloseSun(2.5f));
        StartCoroutine(DelayAndShowCameraAtTheSun(4.5f));

        //earth.enabled = true;
        //earth.Fade(1f);
    }


    IEnumerator DelayAndShowCameraCloseSun(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cameraSunClose.enabled = true;
        cameraStartSun.enabled = false;
    }
    IEnumerator DelayAndShowCameraAtTheSun(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cameraAtTheSun.enabled = true;
        cameraSunClose.enabled = false;
        yellowSquare.alpha = 1;
        LeanTween.alphaCanvas(yellowSquare, 0f, 3f).setEaseOutQuad().setOnComplete(() =>
        {
            MoveSunHeroHorizontal(-13f);

        });
        LeanTween.scaleY(sunGround, 1.1f, 5f).setEaseInOutSine().setLoopPingPong();
    }


    private void MoveSunHeroHorizontal(float distance)
    {
        sunHeroSkeleton.AnimationName = runSunHero;
        sunHero.transform.localScale = new Vector3(-1f, 1f, 1f);
        LeanTween.moveX(sunHero, sunHero.transform.position.x + distance, 55f * Time.deltaTime).setOnComplete(() =>
        {
            sunHeroSkeleton.AnimationName = idleSunHero;
            presidentSkeleton.state.AddAnimation(2, flaxEarsName, true, 0);
            StartCoroutine(StartDialouge());
        });
    }

    private void MovePresidentToComputer(float distance)
    {
        float direction = 1f;
        if (distance < 0)
        {
            direction = -1f;
        }
        presidentSkeleton.state.SetAnimation(1, runPresident, true);
        president.transform.localScale = new Vector3(direction, 1f, 1f);
        LeanTween.moveX(president, president.transform.position.x + distance, 70f * Time.deltaTime).setOnComplete(() =>
        {
            presidentSkeleton.state.SetAnimation(1, idlePresident, true);
            StartComputerVFX.Play();
            computerClose.enabled = true;
            cameraPresidentAndComputen.enabled = false;
            StartCoroutine(WaitAndDisconnectEarth(2.5f));


        });
    }

    IEnumerator WaitAndDisconnectEarth(float secondsBeforeWriteText)
    {
        string textToComputer = "foreach(sunRayToEarth sunRay in sun)@{@sunRay.SetActive(false);@}";
        float durationPrintingText = disconnectEarthText.GetTimeForPrintingText(textToComputer, timeBetweenLettersDialogueText, 1f);
        yield return new WaitForSeconds(secondsBeforeWriteText);
        disconnectEarthText.SetTextTo(textToComputer, timeBetweenLettersComputerText, 10f, false);

        yield return new WaitForSeconds(durationPrintingText + 2f);
        sunAtBeginning.SetActive(false);
        earth.SetActive(true);
        //WhiteSquare for fade..

        LeanTween.alphaCanvas(whiteSquare, 1f, 0.5f).setOnComplete(() =>
         {
             disconnectEarthText.ResetText();
            cameraStartSun.enabled = true;
             computerClose.enabled = false;

            LeanTween.alphaCanvas(whiteSquare, 0f, 0.5f);
             FadeEarthToBlack();
         });


    }


    private void FadeEarthToBlack()
    {
        StartCoroutine(FadeEarthAndShowTextBubbles());
    }


    IEnumerator FadeEarthAndShowTextBubbles()
    {
        yield return new WaitForSeconds(4f);
        LeanTween.color(earth, Color.black, 1f).setEaseOutExpo();
    }




    IEnumerator StartDialouge()
    {
        for (int i = 0; i < dialogueSystem.GetSize(); i++)
        {
            DialogueText newDialogue = dialogueSystem.GetNextDialogue();

            if (newDialogue.name == DialogueText.Name.Elda)
            {
                bubbleSunHeroTalks.ShowBubbleAndPrintText(newDialogue.text, timeBetweenLettersDialogueText, timeBeforeFadeOutText, !newDialogue.textcontinuing);
            }
            else
            {
                bubblePresidentTalks.ShowBubbleAndPrintText(newDialogue.text, timeBetweenLettersDialogueText, timeBeforeFadeOutText, !newDialogue.textcontinuing);
            }
            yield return new WaitForSeconds(bubbleSunHeroTalks.GetTimeForPrintingText(newDialogue.text, timeBetweenLettersDialogueText, timeBeforeFadeOutText));
        }
        MovePresidentToComputer(-10f);
        bubbleSunHeroTalks.Hide();
        bubblePresidentTalks.Hide();
        cameraPresidentAndComputen.enabled = true;
        cameraAtTheSun.enabled = false;
    }
}
