using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Spine.Unity;

public class IntroScene : MonoBehaviour {

    public DialogueSystem dialogueSystem;
    public FadeToBlack earth;
    public DelayLetters startText;


    public BubbleTextController bubbleSunHeroTalks;
    public BubbleTextController bubblePresidentTalks;
    public GameObject yellowSquare;
    public GameObject sunGround;
    public GameObject sunHero;
    public GameObject president;
    public ParticleSystem StartComputerVFX;

    public CinemachineVirtualCamera cameraStartSun;
    public CinemachineVirtualCamera cameraSunClose;
    public CinemachineVirtualCamera cameraAtTheSun;
    public CinemachineVirtualCamera cameraPresidentAndComputen;
    public CinemachineVirtualCamera computerClose;

    private SkeletonAnimation sunHeroSkeleton;
    private SkeletonAnimation presidentSkeletonn;
    private string runSunHero = "RUN";
    private string idleSunHero = "STANDING";
    private string runPresident = "PresidentRun";
    private string idlePresident = "PresidentIdle";
    private string flaxEarsName = "FlaxEars";


    private bool isPrinting = false;
    private float timeBeforeFadeOutText = 1f;
    private float speedWritingText = 0.1f; 

	// Use this for initialization
	void Start () {
        
        sunHeroSkeleton = sunHero.GetComponent<SkeletonAnimation>();
        presidentSkeletonn = president.GetComponent<SkeletonAnimation>();

        startText.SetTextTo("Yesterday...", 0.15f, 1f, true);

        StartCoroutine(DelayAndShowCameraCloseSun(2f));
        StartCoroutine(DelayAndShowCameraAtTheSun(5f));

        //earth.enabled = true;
        //earth.Fade(1f);
    }

    IEnumerator DelayAndShowCameraCloseSun(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cameraSunClose.enabled = true;
    }
    IEnumerator DelayAndShowCameraAtTheSun(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cameraAtTheSun.enabled = true;
        LeanTween.alpha(yellowSquare, 0f, 1f).setOnComplete(() => 
        {
            MoveSunHeroHorizontal(-13f);

        });
        LeanTween.scaleY(sunGround, 1.1f, 5f).setEaseInOutSine().setLoopPingPong();
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    private void MoveSunHeroHorizontal(float distance)
    {
        sunHeroSkeleton.AnimationName = runSunHero;
        sunHero.transform.localScale = new Vector3(-1f, 1f, 1f);
        LeanTween.moveX(sunHero, sunHero.transform.position.x + distance, 3f).setOnComplete(() => {
            sunHeroSkeleton.AnimationName = idleSunHero;
            StartCoroutine(StartDialouge());
        });
    }

    private void MovePresidentHorizontal(float distance)
    {
        float direction = 1f;
        if (distance < 0)
        {
            direction = -1f;
        }
        presidentSkeletonn.state.SetAnimation(1, runPresident, true);
        president.transform.localScale = new Vector3(direction, 1f, 1f);
        LeanTween.moveX(president, president.transform.position.x + distance, 3f).setOnComplete(() => {
            presidentSkeletonn.state.SetAnimation(1, idlePresident, true);
            StartComputerVFX.Play();
            computerClose.enabled = true;




        });
    }



    IEnumerator StartDialouge()
    {
        for (int i = 0; i < dialogueSystem.GetSize(); i++)
        {
            DialogueText newDialogue = dialogueSystem.GetNextDialogue();

            if (newDialogue.name == DialogueText.Name.Elda)
            {
                bubbleSunHeroTalks.ShowAndPrintText(newDialogue.text, speedWritingText, timeBeforeFadeOutText, !newDialogue.textcontinuing);
            }
            else
            {
                bubblePresidentTalks.ShowAndPrintText(newDialogue.text, speedWritingText, timeBeforeFadeOutText, !newDialogue.textcontinuing);
            }

            yield return new WaitForSeconds(bubbleSunHeroTalks.GetTimeForPrintingText(newDialogue.text, speedWritingText) + timeBeforeFadeOutText + 1f);
        }
        MovePresidentHorizontal(-10f);
        bubbleSunHeroTalks.Hide();
        bubblePresidentTalks.Hide();
        cameraPresidentAndComputen.enabled = true;
    }
}
