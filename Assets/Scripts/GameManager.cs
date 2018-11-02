using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    GameObject levelCompleteScreen;
    GameObject deathScreen;
    GameObject challengesScreen;
    Text challengeOneText;
    Text challengeTwoText;
    Text challengeThreeText;

    [SerializeField] AudioClip levelCompleteSound;
    Text flowerCounterText;

    List<Challenge> challenges;
    List<string> challengesCompleted;
    private string challengeOneString = "Find all flowers";

    //Challenges
    [Header("Challenges - pick two!")]
    [SerializeField] float timeChallenge = 0.0f;
    [SerializeField] bool eliminateAllEnemies = false;
    [SerializeField] bool dontEliminateEnemies = false;
    [SerializeField] bool neverPickUpBox = false;
    [SerializeField] bool neverDash = false;

    public float timeChallengeTimer = 0.0f;
    [HideInInspector] public bool hasPickedUpBox = false;
    [HideInInspector] public bool hasDashed = false;

    [HideInInspector] public GameObject dashBar;
    [HideInInspector] public bool dashButtonYellow = false;

    private Color startDashButtonColor;
    private Color startDashBarColor;
    private GameObject dashButton;
    private GameObject goBackToMainMenuButton;
    private int flowersTotal = 0;
    private int pickedFlowers = 0;
    [HideInInspector] public bool started = false;
    PlayerController player;

    //pulsing dashbutton stuff
    Vector3 startScale = Vector3.one;
    Vector3 endScale = new Vector3(1.1f, 1.1f, 1.0f);
    float pulseCounter = 0.0f;


    //Player prefs stuffs
    int currentLevel;
    int challengeOneComplete;
    int challengeTwoComplete;
    int challengeThreeComplete;

    AudioSource audioSource;
    SceneHandler sceneHandler;


    private void Awake() {
        player = FindObjectOfType<PlayerController>();
        player.freezeMovement = true;
        player.cantDie = true;

        audioSource = GetComponent<AudioSource>();
        sceneHandler = GetComponent<SceneHandler>();

        challengesScreen = GameObject.Find("Challenges Screen");
        challengeOneText = GameObject.Find("Challenge One").GetComponent<Text>();
        challengeTwoText = GameObject.Find("Challenge Two").GetComponent<Text>();
        challengeThreeText = GameObject.Find("Challenge Three").GetComponent<Text>();

        levelCompleteScreen = GameObject.Find("Level Complete Screen");
        levelCompleteScreen.SetActive(false);

        deathScreen = GameObject.Find("Death Screen");
        deathScreen.SetActive(false);


        //flowerCounterText = GameObject.Find("Flower Counter Text").GetComponent<Text>();

        dashBar = GameObject.Find("Dash Bar Meter");
        startDashBarColor = dashBar.GetComponent<Image>().color;
        dashBar.transform.parent.gameObject.SetActive(false);
    }

    private void Start() {
        StartCoroutine(InitCoroutine());

        GetPlayerPrefs();
        HandleChallenges();

        goBackToMainMenuButton = GameObject.Find("GoBackToMenuButton");
        goBackToMainMenuButton.SetActive(false);
    }

    private void HandleChallenges() {
        challenges = new List<Challenge>();

        List<Challenge> currentChallenges = new List<Challenge> {
            new Challenge("Time", timeChallenge, "Complete in " + timeChallenge + " seconds"),
            new Challenge("EliminateEnemies", eliminateAllEnemies, "Eliminate all enemies"),
            new Challenge("DontEliminateEnemies", dontEliminateEnemies, "Don't eliminate enemies"),
            new Challenge("Box", neverPickUpBox, "Don't pick up a box"),
            new Challenge("Dash", neverDash, "No dashing!")
         };

        foreach (Challenge c in currentChallenges) {

            if (c.boolValue || c.floatValue > 0.1f) {
                challenges.Add(c);
            }

            if (challenges.Count == 2) { break; }
        }


        if (challengeOneComplete == 1) {
            challengeOneText.text = challengeOneString + " - Completed!";
        }
        else {
            challengeOneText.text = challengeOneString;
        }

        if (challengeTwoComplete == 1) {
            challengeTwoText.text = challenges[0].challengeText + " - Completed!";
        }
        else {
            challengeTwoText.text = challenges[0].challengeText;
        }


        if (challengeThreeComplete == 1) {
            challengeThreeText.text = challenges[1].challengeText + " - Completed!";
        }
        else {
            challengeThreeText.text = challenges[1].challengeText;
        }

    }


    public void StartButtonPressed() {
        started = true;

        dashBar.transform.parent.gameObject.SetActive(true);
        challengesScreen.SetActive(false);

        player.freezeMovement = false;
        player.cantDie = false;

        goBackToMainMenuButton.SetActive(true);
    }

    //For some reason the gameManager couldnt find the dash button in Awake or Start(after loading from another scene), so we find it here instead.
    IEnumerator InitCoroutine() {
        yield return new WaitForEndOfFrame();

#if (UNITY_IOS || UNITY_ANDROID)
        dashButton = GameObject.Find("Dash Button");
        startDashButtonColor = dashButton.GetComponent<Image>().color;
#endif
    }


    void Update() {
        HandleDashBar();
        SetDashButtonColor();

        if (started) {
            if (challenges[0].challengeName == "Time" || challenges[1].challengeName == "Time") {
                timeChallengeTimer += Time.deltaTime;
            }

        }
    }

    public void LevelComplete() {
        //started = false; //this is done in completelevelskateboard for now.

        audioSource.clip = levelCompleteSound;
        audioSource.Play();
        GameObject.Find("Mobile Input UI").SetActive(false);
        GameObject.Find("Dash Bar").SetActive(false);
        SetPlayerPrefs();

        StartCoroutine(SetUpLevelCompleteScreen());
    }

    IEnumerator SetUpLevelCompleteScreen() {
        yield return new WaitForSeconds(1.0f);
        levelCompleteScreen.SetActive(true);
        goBackToMainMenuButton.SetActive(false);
        GameObject flowers = GameObject.Find("Level Complete Flowers");
        Animator[] flowerAnimators = flowers.GetComponentsInChildren<Animator>();
        GameObject completedChallengesObject = GameObject.Find("Completed Challenges");
        Text[] challengesText = completedChallengesObject.GetComponentsInChildren<Text>();

        //Hide the "next level button" if the next scene is the intro.
        GameObject nextLevelButton = GameObject.Find("Next Level Button");
        if (SceneManager.GetActiveScene().buildIndex + 3 == SceneManager.sceneCountInBuildSettings) {
            nextLevelButton.SetActive(false);
            GameObject menuButton = GameObject.Find("Main Menu Button");
            menuButton.transform.position = new Vector3(Screen.width * 0.5f, menuButton.transform.position.y, 0);
        }

        for (int i = 0; i < challengesCompleted.Count; i++) {
            flowerAnimators[i].SetBool("run", true);
            challengesText[i].text = challengesCompleted[i];
            yield return new WaitForSeconds(1.0f);
        }

    }

    public void PlayerDied() {
        deathScreen.SetActive(true);
        GameObject.Find("Mobile Input UI").SetActive(false);
        GameObject.Find("Dash Bar").SetActive(false);
        goBackToMainMenuButton.SetActive(false);

    }

    public void pickedFlower(FlowerController flower) {
        pickedFlowers++;
        //flowerCounterText.text = pickedFlowers + "/" + flowersTotal;

        if (pickedFlowers == flowersTotal) {
            flower.gameObject.AddComponent<FoundAllFlowers>();
        }
    }



    public void AddFlower() {
        flowersTotal++;
        //flowerCounterText.text = pickedFlowers + "/" + flowersTotal;
    }

    private void HandleDashBar() {

        //Scale dashbar with current dashtime
        dashBar.transform.localScale = new Vector3(player.dashTime / player.startDashTime, 1, 1);

        //Just here so the dashbar doesnt scale below zero
        if (dashBar.transform.localScale.x < 0) {
            dashBar.transform.localScale = new Vector3(0, 1, 1);
        }

        if (player.canDash) {
            dashBar.GetComponent<Image>().color = startDashBarColor;
        }
        else {
            Color gray = Color.gray;
            gray.a = 0.5f;
            dashBar.GetComponent<Image>().color = gray;
        }

    }

    public void SetDashButtonColor() {
        if (dashButton == null) { return; }

        if (dashButtonYellow || player.isCarryingBox) {
            Color yellow = new Color32(191, 185, 30, 200);
            dashButton.GetComponent<Image>().color = yellow;
            pulseDashButton();     
        }
        else {
            dashButton.GetComponent<Image>().color = startDashButtonColor;
            dashButton.transform.localScale = Vector3.one;
        }

    }

    public void pulseDashButton() {

        pulseCounter += Time.deltaTime;
        Vector3 scale = Vector3.Lerp(startScale, endScale, pulseCounter);
        dashButton.transform.localScale = scale;
        
        if(pulseCounter >= 1) {
            pulseCounter = 0.0f;
            Vector3 temp = startScale;
            startScale = endScale;
            endScale = temp;
        }
    }

    void SetPlayerPrefs() {
        int progress = PlayerPrefs.GetInt("Progress", 1);

        if (currentLevel >= progress) {
            PlayerPrefs.SetInt("Progress", currentLevel + 1);
        }

        CheckIfChallengesCompleted();
        challengesCompleted = new List<string>();
        if (pickedFlowers == flowersTotal) { PlayerPrefs.SetInt("Level " + currentLevel + " challenge one", 1); challengesCompleted.Add(challengeOneString); }
        if (challenges[0].completed) { PlayerPrefs.SetInt("Level " + currentLevel + " challenge two", 1); challengesCompleted.Add(challenges[0].challengeText); }
        if (challenges[1].completed) { PlayerPrefs.SetInt("Level " + currentLevel + " challenge three", 1); challengesCompleted.Add(challenges[1].challengeText); }

    }

    void GetPlayerPrefs() {

        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //1 = challenge complete
        challengeOneComplete = PlayerPrefs.GetInt("Level " + currentLevel + " challenge one", 0);
        challengeTwoComplete = PlayerPrefs.GetInt("Level " + currentLevel + " challenge two", 0);
        challengeThreeComplete = PlayerPrefs.GetInt("Level " + currentLevel + " challenge three", 0);

    }


    void CheckIfChallengesCompleted() {

        foreach (Challenge c in challenges) {

            switch (c.challengeName) {

                case "Time":
                    if (timeChallengeTimer <= timeChallenge) { c.completed = true; }
                    break;

                case "EliminateEnemies":
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    if (enemies.Length == 0) { c.completed = true; }
                    break;

                case "DontEliminateEnemies":
                    GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("Enemy");
                    if (enemies1.Length > 0) { c.completed = true; }
                    break;

                case "Box":
                    if (!hasPickedUpBox) { c.completed = true; }
                    break;

                case "Dash":
                    if (!hasDashed) { c.completed = true; }
                    break;

                default: break;
            }
        }

    }

}

