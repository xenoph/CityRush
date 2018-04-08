using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;

public class MenuScript : MonoBehaviour {

    //Public GameObjects that are set in the inspector of each scene
    public GameObject ResourceText;
    public GameObject EnemyWallHealthText;
    public GameObject FriendlyWallHealthText;
    public GameObject PauseMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public static bool LevelFinished;
    public static string LevelWinner = "";
    public static bool EnemyIsFiring;

    private string _previousGameState;

    //Tutorial level only
    public static GameObject[] TutorialTexts;
    public static GameObject CentreTutorialText;
    public static GameObject DialogueText;
    public static GameObject TutorialStones;

    //Counters for the Player units
    public GameObject TankCounter;
    public GameObject JeepCounter; 

    void Awake() {
        //Make sure everything is reset when a level is started
        LevelFinished = false;
        LevelWinner = "";
        TutorialTexts = GameObject.FindGameObjectsWithTag("TutorialText");
        DialogueText = GameObject.FindGameObjectWithTag("DialogueText");
        CentreTutorialText = GameObject.FindGameObjectWithTag("CentreText");
        TutorialStones = GameObject.FindGameObjectWithTag("TutorialStones");
        CentreTutorialText = GameObject.FindGameObjectWithTag("CentreText");

        //Set the GameState depending on level loaded
        if(Application.loadedLevelName == "Level_01") {            
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Level1Start);
        }
        if(Application.loadedLevelName == "Tutorial") {
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Tutorial1);
        }
        if (Application.loadedLevelName == "Level_02") {
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Level2Start);
        }
        if (Application.loadedLevelName == "Level_03") {
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Level3Start);
        }
        if (Application.loadedLevelName == "Level_04") {
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Level4Start);
        }
        if (Application.loadedLevelName == "Level_05") {
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Level5Start);
        }
        if (Application.loadedLevelName == "Level_06") {
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Level6Start);
        }
        if (Application.loadedLevelName == "Level_07") {
            AudioListener.pause = false;
            GameManager.Instance.SetGameState(GameState.Level7Start);
        }
        //Make sure the various menus are disabled when the level starts
        PauseMenu.SetActive(false);
        WinMenu.SetActive(false);
        LoseMenu.SetActive(false);
    }

    void Start() {
        //Run a Coroutine that will count the number of units in play at each FixedUpdate
        StartCoroutine(CheckUnitsInPlay());
    }

    void Update () {
        //This will send the wall health to the UI elements, it will also make sure it doesn't go below 0
        ResourceText.GetComponent<Text>().text = GameManager.Resources.ToString();
        if (GameManager.EnemyWallHealth < 0) {
            EnemyWallHealthText.GetComponent<Text>().text = 0.ToString();
        }
        else {
            EnemyWallHealthText.GetComponent<Text>().text = GameManager.EnemyWallHealth.ToString();
        }
        if (GameManager.FriendlyWallHealth < 0)
        {
            FriendlyWallHealthText.GetComponent<Text>().text = 0.ToString();
        }
        else
        {
            FriendlyWallHealthText.GetComponent<Text>().text = GameManager.FriendlyWallHealth.ToString();
        }
        //Get the input to pause the game - first it makes a check to make sure it's not already paused
        if (GameManager.CurrentGameState != "PauseMenu") {
            if (Input.GetButtonDown("Cancel")) {
                PauseMenu.SetActive(true);
                AudioListener.pause = true;
                _previousGameState = GameManager.CurrentGameState;
                GameManager.Instance.SetGameState(GameState.PauseMenu);
            }
        }

        //These three if's checks if the Wall healths have gone to 0, or if the timer has run out
        //They will then execute either the Win scenario, or the Lose scenario
	    if (GameManager.EnemyWallHealth <= 0) {
	        StartCoroutine(WinConditionMet());
	    }

        if (GameManager.FriendlyWallHealth <= 0) {
            StartCoroutine(LoseConditionMet());
        }

        if (GameManager.TimeToFinishLevel <= 0.1f) {           
            StartCoroutine(LoseConditionMet());
        }
	}

    //GUI button functions
	public void StartGameButton(){
		Application.LoadLevel("Tutorial");
        GameManager.Instance.SetGameState(GameState.Tutorial1);
	}

	public void QuitGameButton(){
		Application.LoadLevel("Menu");
	}

	public void MenuButton(){
		Application.LoadLevel("Menu");
	}
    //Load next level depending on what the previous level was.
    //This is messy and should function differently - but currently I don't know any other way.
    public void NextLevel() {
        if (_previousGameState == "Tutorial3" || _previousGameState == "Tutorial4") {           
            Application.LoadLevel("Level_01");          
        }
        if (_previousGameState == "Level" || _previousGameState == "Level1Start") {
            Application.LoadLevel("Level_02");                             
        }
        if (_previousGameState == "Level2" || _previousGameState == "Level2Start") {
            Application.LoadLevel("Level_03");
        }
        if (_previousGameState == "Level3" || _previousGameState == "Level3Start") {
            Application.LoadLevel("Level_04");
        }
        if (_previousGameState == "Level4" || _previousGameState == "Level4Start") {
            Application.LoadLevel("Level_05");
        }
        if (_previousGameState == "Level5" || _previousGameState == "Level5Start") {
            Application.LoadLevel("Level_06");
        }
        if (_previousGameState == "Level6" || _previousGameState == "Level6Start") {
            Application.LoadLevel("Level_07");
        }
        AudioListener.pause = false;
        WinMenu.SetActive(false);
    }
    //Restart the level if the user loses and want to try again
    public void TryAgain() {
        Application.LoadLevel(Application.loadedLevel);
    }
    //Resume the game based on what the previous game state was when the Player clicked pause.
    //Again, it's messy and and over-use of if statements. This can surely be done better.
    public void ResumeGame() {
        if (_previousGameState == "Tutorial1") {
            GameManager.Instance.SetGameState(GameState.Tutorial1);
        }

        if (_previousGameState == "Tutorial2") {
            GameManager.Instance.SetGameState(GameState.Tutorial2);
        }

        if (_previousGameState == "Tutorial3") {
            GameManager.Instance.SetGameState(GameState.Tutorial3);
        }

        if (_previousGameState == "Tutorial4") {
            GameManager.Instance.SetGameState(GameState.Tutorial4);
        }

        if (_previousGameState == "Level1Start" || _previousGameState == "Level1") {
            GameManager.Instance.SetGameState(GameState.Level1);
        }

        if (_previousGameState == "Level2Start" || _previousGameState == "Level2") {
            GameManager.Instance.SetGameState(GameState.Level2);
        }

        if (_previousGameState == "Level3Start" || _previousGameState == "Level3") {
            GameManager.Instance.SetGameState(GameState.Level3);
        }

        if (_previousGameState == "Level4Start" || _previousGameState == "Level4") {
            GameManager.Instance.SetGameState(GameState.Level4);
        }
        if (_previousGameState == "Level5Start" || _previousGameState == "Level5") {
            GameManager.Instance.SetGameState(GameState.Level5);
        }
        if (_previousGameState == "Level6Start" || _previousGameState == "Level6") {
            GameManager.Instance.SetGameState(GameState.Level6);
        }
        if (_previousGameState == "Level7Start" || _previousGameState == "Level7") {
            GameManager.Instance.SetGameState(GameState.Level7);
        }
        AudioListener.pause = false;
        PauseMenu.SetActive(false);
    }

    //This will make sure the tutorial texts are disabled on any level that isn't the Tutorial
    //The call for this function is sent from GameManager
    public static void DisableTutText() {
        if (TutorialTexts != null) {
            foreach (var text in TutorialTexts) {
                text.SetActive(false);
            }
        }
    }
    //Disable the centre tutorial text - called from GameManager
    public static void DisableCentreText() {
        CentreTutorialText.SetActive(false);
    }

    //Frida's dialogue. Most calls come from GameManager
    //It would have been more efficient to make the Dialogue into its own class and worked out that way
    //Which really is true for most larger functions like this. Hindsight is 10/10.
    public static void ChangeDialogue(int x) {
        DialogueText.SetActive(true);
        switch (x) {
            case 1:
                DialogueText.GetComponent<Text>().text =
                    "We have to get our units within range of the towers to capture them!";
                break;

            case 2:
                DialogueText.GetComponent<Text>().text = 
                    "We should capture more towers as they give us resources!";
                break;

            case 3:
                DialogueText.GetComponent<Text>().text = 
                    "Good work! Multiple towers gives our missiles a damage multiplier!";
                break;

            case 4:
                DialogueText.GetComponent<Text>().text =
                    "It doesn't look like they have managed activate their towers yet!";
                break;

            case 5:
                DialogueText.GetComponent<Text>().text =
                    "We should aim to take control over the central areas!";
                break;

            case 6:
                DialogueText.GetComponent<Text>().text = 
                    "Oh no, the enemy has aimed their towers against our wall!";
                break;

            case 7:
                DialogueText.GetComponent<Text>().text = 
                    "I think we're nearing the end of the desert!";
                break;

            case 8:
                DialogueText.GetComponent<Text>().text =
                    "There's grass over there! We are closing in on the enemy cities!";
                break;

            case 9:
                DialogueText.GetComponent<Text>().text =
                    "The enemy are using their new Jeep! We have to make sure we're not overrun!";
                break;

            case 10:
                DialogueText.GetComponent<Text>().text =
                    "Move our Units quickly and get those towers taken!";
                break;

            case 11:
                DialogueText.GetComponent<Text>().text =
                    "We've taken down their wall! Now move in!";
                break;

            case 12:
                DialogueText.GetComponent<Text>().text =
                    "They're taking back a tower! Defend it!";
                break;

            case 13:
                DialogueText.GetComponent<Text>().text =
                    "Good work on defending that tower!";
                break;

            default:
                DialogueText.SetActive(false);
                break;
        }
    }
    //If the Player wins, the game will remove all current missiles in the air and make the turrets stop shootin.
    //It will then set the level to finished, set the winner, turn off the sound, change the dialogue
    // and then play the fireworks. After 6 seconds, it will show the win menu.
    //The camera will pan to the Enemy wall to show it being destroyed
    IEnumerator WinConditionMet() {
        _previousGameState = GameManager.CurrentGameState;
        GameObject[] missilesToClear = GameObject.FindGameObjectsWithTag("WallMissile");
        foreach (GameObject missileGameObject in missilesToClear) {
            Destroy(missileGameObject);
        }
        LevelFinished = true;
        LevelWinner = "Player";       
        ChangeDialogue(11);
        yield return new WaitForSeconds(6);
        AudioListener.pause = true;
        WinMenu.SetActive(true);
    }
    //Same as WinCondition, except the winner is set to Enemy, and the Lose menu will show
    IEnumerator LoseConditionMet() {
        _previousGameState = GameManager.CurrentGameState;
        GameObject[] missilesToClear = GameObject.FindGameObjectsWithTag("WallMissile");
        foreach(GameObject missileGameObject in missilesToClear) {
            Destroy(missileGameObject);
        }
        LevelFinished = true;
        LevelWinner = "Enemy";       
        yield return new WaitForSeconds(6);
        AudioListener.pause = true;
        LoseMenu.SetActive(true);
    }

    //Will count the number of units currently in the scene and return a number that will be placed in the UI
    IEnumerator CheckUnitsInPlay() {
        while (LevelFinished == false) {
            GameObject[] unitsInPlay = GameObject.FindGameObjectsWithTag("Team1");
            int TanksInPlay = 0;
            int JeepsInPlay = 0;
            foreach (GameObject unit in unitsInPlay) {
                if (unit.name == "Tank_team1(Clone)") {
                    TanksInPlay += 1;
                }
                if (unit.name == "RebelJeep(Clone)") {
                    JeepsInPlay += 1;
                }
            }
            TankCounter.GetComponent<Text>().text = TanksInPlay.ToString();
            JeepCounter.GetComponent<Text>().text = JeepsInPlay.ToString();
            //Wait for fixed update before updating the units every time to make sure all other functions are done running.
            yield return new WaitForFixedUpdate();
        }
    }

    //Function called from the TurretController that will, if at the right gamestate,
    //count the number of player-owned towers and change the dialogue.
    //After 10 seconds, change the dialogue again to explain why the Enemy towers don't fire.
    public static IEnumerator CheckTurretCountTutorial() {
        if (GameManager.CurrentGameState == "Tutorial3") {
            GameObject[] currentTurrets = GameObject.FindGameObjectsWithTag("Tower");
            int playerControlled = 0;
            foreach (GameObject turret in currentTurrets) {
                if (turret.GetComponent<TurretController>().ControlHealth == 0) {
                    playerControlled += 1;
                }
            }
            if (playerControlled == 2) {
                GameManager.Instance.SetGameState(GameState.Tutorial4);
                yield return new WaitForSeconds(10);
                ChangeDialogue(4);
            }
        }
    }
}
