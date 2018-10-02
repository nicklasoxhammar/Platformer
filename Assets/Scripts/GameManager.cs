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

    //Challenges
    [Header("Challenges - pick two!")]
    [SerializeField] float timeChallenge = 0.0f;
    [SerializeField] bool eliminateAllEnemies = false;
    [SerializeField] bool neverPickUpBox = false;
    [SerializeField] bool neverDash = false;

    private float timeChallengeTimer = 0.0f;
    [HideInInspector] public bool hasPickedUpBox = false;
    [HideInInspector] public bool hasDashed = false;

    [HideInInspector] public GameObject dashBar;
    [HideInInspector] public bool dashButtonYellow = false;

    private Color startDashButtonColor;
    private Color startDashBarColor;
    private GameObject dashButton;
    private int flowersTotal = 0;
    private int pickedFlowers = 0;
    private bool started = false;
    PlayerController player;


    //Player prefs stuffs
    int currentLevel;
    int challengeOneComplete;
    int challengeTwoComplete;
    int challengeThreeComplete;

    AudioSource audioSource;

    private void Awake() {
        player = FindObjectOfType<PlayerController>();
        player.freezeMovement = true;

        audioSource = GetComponent<AudioSource>();

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
    }

    private void HandleChallenges() {
        challenges = new List<Challenge>();

        List<Challenge> currentChallenges = new List<Challenge> {
            new Challenge("Time", timeChallenge, "Complete in " + timeChallenge + " seconds"),
            new Challenge("Enemies", eliminateAllEnemies, "Eliminate all enemies"),
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
            challengeOneText.text = challengeOneText.text + " - Completed!";
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

            if (challenges[0].challengeText == "Time" || challenges[1].challengeText == "Time") {
                timeChallengeTimer += Time.deltaTime;
            }

        }
    }

    public void LevelComplete() {
        started = false;

        audioSource.clip = levelCompleteSound;
        audioSource.Play();
        levelCompleteScreen.SetActive(true);
        GameObject.Find("Mobile Input UI").SetActive(false);
        GameObject.Find("Dash Bar").SetActive(false);
        SetPlayerPrefs();
    }

    public void PlayerDied() {
        deathScreen.SetActive(true);
        GameObject.Find("Mobile Input UI").SetActive(false);
        GameObject.Find("Dash Bar").SetActive(false);
    }

    public void pickedFlower() {
        pickedFlowers++;
        //flowerCounterText.text = pickedFlowers + "/" + flowersTotal;

        if (pickedFlowers == flowersTotal) {
            //ALLA PLOCKADE.
            Debug.Log("ALLA PLOCKADE");
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
            Color yellow = Color.yellow;
            yellow.a = 0.5f;
            dashButton.GetComponent<Image>().color = yellow;
        }
        else {
            dashButton.GetComponent<Image>().color = startDashButtonColor;
        }

    }

    void SetPlayerPrefs() {
        int progress = PlayerPrefs.GetInt("Progress", 1);

        if (currentLevel >= progress) {
            PlayerPrefs.SetInt("Progress", currentLevel + 1);
        }

        CheckIfChallengesCompleted();

        if (pickedFlowers == flowersTotal) { PlayerPrefs.SetInt("Level " + currentLevel + " challenge one", 1); }
        if (challenges[0].completed) { PlayerPrefs.SetInt("Level " + currentLevel + " challenge two", 1); }
        if (challenges[1].completed) { PlayerPrefs.SetInt("Level " + currentLevel + " challenge three", 1); }

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

                case "Enemies":
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    if (enemies.Length == 0) { c.completed = true; }
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

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void NextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TryAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

