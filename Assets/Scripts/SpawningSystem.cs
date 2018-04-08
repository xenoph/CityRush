using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;

public class SpawningSystem : MonoBehaviour {

    //Unit GameObjects
    public GameObject EnemyUnit;
    public GameObject EnemyJeep;
    public GameObject UnitJeep;
    public GameObject UnitTank;

    //UI Elements
    public GameObject SliderJeep;
    public GameObject SliderTank;

    private float timeToSpawnJeep;
    private float timeToSpawnTank;
    [HideInInspector] public bool SpawningJeep;
    [HideInInspector] public bool SpawningTank;

    //[HideInInspector] public float Resources;

    //Timer variables
    public float minutes;
    public float seconds;
    public string counterTimerString;
    public GameObject CounterText;

	public float spawnEnemyTimer;
	public float enemySpawnLimit = 2f;

    //Tobesorted
    public AudioSource Music;
    private List<GameObject> EnemyUnits;

    void Start() {
        Music.PlayDelayed(3);
        EnemyUnits = new List<GameObject>(2);
        EnemyUnits.Add(EnemyUnit);
        EnemyUnits.Add(EnemyJeep);
    }

    void Update() {


        if (MenuScript.LevelFinished == false) {
            CheckUnitLimit();
            SpawningFriendlySystem();
        }

        if (Input.GetKeyUp(KeyCode.T)) {
            SpawnUnitTank();
        }
        if (Input.GetKeyUp(KeyCode.J)) {
            SpawnUnitJeep();
        }

        if(GameManager.TimeToFinishLevel >= 0.1f) {
			
            GameManager.TimeToFinishLevel -= Time.deltaTime;

            minutes = Mathf.FloorToInt(GameManager.TimeToFinishLevel / 60F);
            seconds = Mathf.FloorToInt(GameManager.TimeToFinishLevel - minutes * 60);

            counterTimerString = string.Format("{00:00} : {01:00}", minutes, seconds);

            CounterText.GetComponent<Text>().text = counterTimerString;
		}
    }

    //This will check how many enemy units are currently in the scene, and not allow spawning of new units
    //if it reaches the threshold set by the GameManager
    void CheckUnitLimit() {
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Team2");
        int enemyCount = enemyUnits.Count();
        if (enemyCount >= GameManager.EnemyUnitLimit) {
            return;
        }
        else {
            SpawnEnemyUnit();
        }
    }

    void SpawnEnemyUnit() {
        //If the game is on the tutorial or before the jeep has been added, instantiate the enemy tank
        GameManager.EnemySpawnLimit += Time.deltaTime;
        if(GameManager.EnemySpawnLimit >= GameManager.EnemySpawnRate) {
            if (GameManager.CurrentGameState == "Tutorial1" || GameManager.CurrentGameState == "Tutorial2" ||
                GameManager.CurrentGameState == "Tutorial3" || GameManager.CurrentGameState == "Tutorial4" ||
                GameManager.CurrentGameState == "Level1Start" || GameManager.CurrentGameState == "Level1" ||
                GameManager.CurrentGameState == "Level2Start" || GameManager.CurrentGameState == "Level2") {
                Instantiate(EnemyUnit, GameManager.EnemyCurrentSpawnPosition, Quaternion.identity);
            }
            else {
                //Else, randomise between the tank and jeep.
                int randUnit = Random.Range(0, EnemyUnits.Count);
                GameObject unitToSpawn = EnemyUnits[randUnit];
                Instantiate(unitToSpawn, GameManager.EnemyCurrentSpawnPosition, Quaternion.identity);
                if (unitToSpawn.name == "ArmyJeep") {
                    //Inform the player through dialogue when the jeep is first added
                    if (GameManager.CurrentGameState == "Level3Start" || GameManager.CurrentGameState == "Level3") {
                        MenuScript.ChangeDialogue(9);
                    }
                    //Jeeps will decrease the spawn time on the next unit
                    GameManager.EnemySpawnLimit = 2f;
                    return;
                }
            }
            GameManager.EnemySpawnLimit = 0f;
        }
    }

    public void SpawnUnitTank() {
        CheckTutLevel();
        if (!SpawningTank) {
            if (GameManager.Resources < GameManager.TankCost) {
                return;
            }
            //Spawning tank is set to true to begin process on spawning, trough the spawningSystem()
            SpawningTank = true;
            GameManager.Resources -= GameManager.TankCost;
            //Setting timer to spawn with Time.time that means (time since game started in float) + the time to spawn new unit
            timeToSpawnTank = Time.time + 2f;
        }
    }

    public void SpawnUnitJeep() {
        CheckTutLevel();
        if (!SpawningJeep) {
            if (GameManager.Resources < GameManager.JeepCost) {
                return;
            }
            //Spawning jeep is set to true to begin process on spawning, trough the spawningSystem()
            SpawningJeep = true;
            GameManager.Resources -= GameManager.JeepCost;
            //Setting timer to spawn with Time.time that means (time since game started in float) + the time to spawn new unit
            timeToSpawnJeep = Time.time + 2f;
        }
    }

    public void SpawningFriendlySystem() {
        if (SpawningJeep) {
            float timeUSpawn = timeToSpawnJeep - Time.time;
            SliderJeep.GetComponent<Slider>().value = timeUSpawn;

            if (Time.time >= timeToSpawnJeep) {
                Instantiate(UnitJeep, GameManager.FriendlyCurrentSpawnPosition, Quaternion.identity);
                SpawningJeep = false;
            }
        }

        if (SpawningTank) {
            float timeUSpawn = timeToSpawnTank - Time.time;
            SliderTank.GetComponent<Slider>().value = timeUSpawn;

            if (Time.time >= timeToSpawnTank) {
                Instantiate(UnitTank, GameManager.FriendlyCurrentSpawnPosition, Quaternion.identity);
                SpawningTank = false;
            }
        }
    }

    void CheckTutLevel() {
        if (GameManager.CurrentGameState == "Tutorial1") {
            GameManager.Instance.SetGameState(GameState.Tutorial2);
        }
    }
}