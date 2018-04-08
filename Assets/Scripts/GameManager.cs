using UnityEngine;

public enum GameState
{
    NullState,
    StartMenu,
    PauseMenu,
    EndLevelMenu,
    Tutorial1,
    Tutorial2,
    Tutorial3,
    Tutorial4,
    Level1Start,
    Level1,
    Level2Start,
    Level2,
    Level3Start,
    Level3,
    Level4Start,
    Level4,
    Level5Start,
    Level5,
    Level6Start,
    Level6,
    Level7Start,
    Level7
}

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    public static float TimeToFinishLevel;
    public static string CurrentGameState;

    //Health variables
    public static float EnemyTankHealth;
    public static float EnemyJeepHealth;
    public static float FriendlyTankHealth;
    public static float FriendlyJeepHealth;
    public static float EnemyWallHealth;
    public static float FriendlyWallHealth;

    //Damage variables
    public static float EnemyTankMissileDamage;
    public static float EnemyJeepMissileDamage;
    public static float FriendlyMissileDamage;
    public static float FriendlyJeepDamage;
    public static float TowerMissileDamage;
    public static int TowerDamageMultiplier;

    //Spawn variables
    public static Vector3 FriendlyCurrentSpawnPosition;
    public static Vector3 SpawnToPositionFriendly;
    public static Vector3 EnemyCurrentSpawnPosition;
    public static float EnemySpawnLimit;
    public static int EnemyUnitLimit;
    public static float EnemySpawnRate;
    public static int JeepCost;
    public static int TankCost;

    //Resource variables
    public static float Resources;
    public static float IncrementingResources;
    public static float IncrementialTimeLimit = 0f;
    public static float IncrementialTimeRate;
    public static float TowerResourceGain;
    public static float TowerResourceTimer;

    //Tower target variables
    public static Vector3 TowerLookFriendly;
    public static Vector3 TowerLookEnemy;

    //Camera restrictions
    public static float CameraUpperLimit;
    public static float CameraLowerLimit;
    public static float CameraLeftLimit;
    public static float CameraRightLimit;
    public static float CameraMaxToClamp;
    public static Vector3 FriendlyWallCamera;
    public static Vector3 EnemyWallCamera;

    //private GameState _currentGameState;
    public static GameManager _instance;

    static public bool isActive
    {
        get { return _instance != null; }
    }

    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    protected GameManager()
    {
    }

    //Singleton pattern implementation
    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Object.FindObjectOfType(typeof (GameManager)) as GameManager;

            if (_instance == null)
            {
                GameObject go = new GameObject("_gamemanager");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        if (OnStateChange != null)
        {
            OnStateChange();
        }

        switch (gameState)
        {
            case GameState.PauseMenu:
                Time.timeScale = 0f;
                CurrentGameState = "PauseMenu";

                break;

            case GameState.Tutorial1:
                Time.timeScale = 0f;
                CurrentGameState = "Tutorial1";

                //Generic level variables
                Resources = 200f;
                TimeToFinishLevel = 240f;
                IncrementingResources = 6;
                IncrementialTimeRate = 2f;
                EnemyWallHealth = 300f;
                FriendlyWallHealth = 300f;

                //Spawning variables
                EnemyCurrentSpawnPosition = new Vector3(0f, 1.64f, -155f);
                SpawnToPositionFriendly = new Vector3(-0.2f, -0.9f, 16f);
                FriendlyCurrentSpawnPosition = new Vector3(0f, 1.64f, 63.3f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 10f;
                EnemyUnitLimit = 2;

                //Variables for the towers
                TowerDamageMultiplier = 0;
                TowerLookEnemy = new Vector3(-1.6f, 4.3f, -171f);
                TowerLookFriendly = new Vector3(1.1f, 6.6f, 37.3f);
                TowerResourceGain = 5f;
                TowerMissileDamage = 1f;
                TowerResourceTimer = 4f;

                //Unit variables              
                EnemyTankHealth = 50f;
                EnemyTankMissileDamage = 4f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 100f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 30f;
                FriendlyJeepDamage = 2f;

                //Camera movement limits
                CameraUpperLimit = -50f;
                CameraLowerLimit = 85f;
                CameraLeftLimit = 30f;
                CameraRightLimit = -30f;
                CameraMaxToClamp = 1f;
                FriendlyWallCamera = new Vector3(0, 130, 71);
                EnemyWallCamera = new Vector3(0, 130, -71);

                break;

            case GameState.Tutorial2:
                CurrentGameState = "Tutorial2";
                MenuScript.ChangeDialogue(1);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;
                break;

            case GameState.Tutorial3:
                CurrentGameState = "Tutorial3";
                MenuScript.ChangeDialogue(2);
                Time.timeScale = 1f;
                MenuScript.TutorialStones.SetActive(false);
                EnemyUnitLimit = 3;

                break;

            case GameState.Tutorial4:
                CurrentGameState = "Tutorial4";
                MenuScript.ChangeDialogue(3);
                Time.timeScale = 1f;

                break;

            case GameState.Level1Start:
                CurrentGameState = "Level1Start";
                MenuScript.DisableTutText();
                MenuScript.ChangeDialogue(5);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;

                //Generic level variables
                Resources = 200f;
                IncrementingResources = 5;
                IncrementialTimeRate = 2f;
                TimeToFinishLevel = 300f;
                EnemyWallHealth = 500f;
                FriendlyWallHealth = 500f;

                //Spawning variables
                EnemyUnitLimit = 6;
                FriendlyCurrentSpawnPosition = new Vector3(90f, 1.64f, 255f);
                SpawnToPositionFriendly = new Vector3(90f, 1.64f, 230f);
                EnemyCurrentSpawnPosition = new Vector3(90f, 1.64f, -90f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 10f;

                //Tower variables
                TowerDamageMultiplier = 0;
                TowerMissileDamage = 2f;
                TowerLookFriendly = new Vector3(84f, 11f, 270f);
                TowerLookEnemy = new Vector3(84f, 11f, -111f);
                TowerResourceGain = 2f;
                TowerResourceTimer = 2f;

                //Unit variables
                EnemyTankHealth = 65f;
                EnemyTankMissileDamage = 6f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 30f;
                FriendlyJeepDamage = 2f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 100f;

                //Camera variables                
                CameraUpperLimit = -260f;
                CameraLowerLimit = 50f;
                CameraLeftLimit = 80f;
                CameraRightLimit = -90f;
                CameraMaxToClamp = 3.5f;
                FriendlyWallCamera = new Vector3(-5, 0, 25);
                EnemyWallCamera = new Vector3(-6, 0, -258);

                break;

            case GameState.Level1:
                CurrentGameState = "Level1";
                Time.timeScale = 1f;

                break;

            case GameState.Level2Start:
                CurrentGameState = "Level2Start";
                MenuScript.DisableTutText();
                MenuScript.ChangeDialogue(7);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;

                //Generic level variables
                Resources = 300f;
                IncrementingResources = 5;
                IncrementialTimeRate = 2f;
                TimeToFinishLevel = 300f;
                EnemyWallHealth = 600f;
                FriendlyWallHealth = 500f;

                //Spawning variables
                EnemyUnitLimit = 7;
                FriendlyCurrentSpawnPosition = new Vector3(0f, 1.64f, 185f);
                SpawnToPositionFriendly = new Vector3(0f, 1.64f, 170f);
                EnemyCurrentSpawnPosition = new Vector3(0f, 1.64f, -170f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 8f;

                //Tower variables
                TowerDamageMultiplier = 0;
                TowerMissileDamage = 2f;
                TowerLookFriendly = new Vector3(0f, 8f, 194f);
                TowerLookEnemy = new Vector3(0f, 8f, -185f);
                TowerResourceGain = 3f;
                TowerResourceTimer = 3f;

                //Unit variables
                EnemyTankHealth = 70f;
                EnemyTankMissileDamage = 7f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 30f;
                FriendlyJeepDamage = 2f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 100f;

                //Camera variables                
                CameraUpperLimit = -295f;
                CameraLowerLimit = -6f;
                CameraLeftLimit = 83f;
                CameraRightLimit = -90f;
                CameraMaxToClamp = 3.5f;
                FriendlyWallCamera = new Vector3(-5, 0, 25);
                EnemyWallCamera = new Vector3(-6, 0, -258);

                break;

            case GameState.Level2:
                CurrentGameState = "Level2";
                Time.timeScale = 1.0f;

                break;

            case GameState.Level3Start:
                CurrentGameState = "Level3Start";
                MenuScript.DisableTutText();
                MenuScript.ChangeDialogue(8);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;

                //Generic level variables
                Resources = 300f;
                IncrementingResources = 5;
                IncrementialTimeRate = 1.8f;
                TimeToFinishLevel = 300f;
                EnemyWallHealth = 750f;
                FriendlyWallHealth = 600f;

                //Spawning variables
                EnemyUnitLimit = 8;
                FriendlyCurrentSpawnPosition = new Vector3(90f, 1.64f, 255f);
                SpawnToPositionFriendly = new Vector3(90f, 1.64f, 230f);
                EnemyCurrentSpawnPosition = new Vector3(90f, 1.64f, -90f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 8f;

                //Tower variables
                TowerDamageMultiplier = 0;
                TowerMissileDamage = 2.5f;
                TowerLookFriendly = new Vector3(84f, 11f, 270f);
                TowerLookEnemy = new Vector3(84f, 11f, -111f);
                TowerResourceGain = 3f;
                TowerResourceTimer = 2f;

                //Unit variables
                EnemyTankHealth = 75f;
                EnemyJeepHealth = 40f;
                EnemyJeepMissileDamage = 4f;
                EnemyTankMissileDamage = 8f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 30f;
                FriendlyJeepDamage = 2f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 105f;

                //Camera variables                
                CameraUpperLimit = -220f;
                CameraLowerLimit = 35f;
                CameraLeftLimit = 80f;
                CameraRightLimit = -90f;
                CameraMaxToClamp = 3.5f;
                FriendlyWallCamera = new Vector3(-5, 0, 25);
                EnemyWallCamera = new Vector3(-6, 0, -258);

                break;

            case GameState.Level3:
                CurrentGameState = "Level3";
                Time.timeScale = 1.0f;

                break;

            case GameState.Level4Start:
                CurrentGameState = "Level4Start";
                MenuScript.DisableTutText();
                MenuScript.ChangeDialogue(10);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;

                //Generic level variables
                Resources = 300f;
                IncrementingResources = 5;
                IncrementialTimeRate = 1.8f;
                TimeToFinishLevel = 300f;
                EnemyWallHealth = 750f;
                FriendlyWallHealth = 600f;

                //Spawning variables
                EnemyUnitLimit = 8;
                FriendlyCurrentSpawnPosition = new Vector3(90f, 1.64f, 255f);
                SpawnToPositionFriendly = new Vector3(90f, 1.64f, 230f);
                EnemyCurrentSpawnPosition = new Vector3(90f, 1.64f, -90f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 8f;

                //Tower variables
                TowerDamageMultiplier = 0;
                TowerMissileDamage = 2.5f;
                TowerLookFriendly = new Vector3(84f, 11f, 270f);
                TowerLookEnemy = new Vector3(84f, 11f, -111f);
                TowerResourceGain = 3f;
                TowerResourceTimer = 2f;

                //Unit variables
                EnemyTankHealth = 75f;
                EnemyTankMissileDamage = 8f;
                EnemyJeepHealth = 45f;
                EnemyJeepMissileDamage = 5f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 35f;
                FriendlyJeepDamage = 2f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 105f;

                //Camera variables                
                CameraUpperLimit = -220f;
                CameraLowerLimit = 35f;
                CameraLeftLimit = 80f;
                CameraRightLimit = -90f;
                CameraMaxToClamp = 3.5f;
                FriendlyWallCamera = new Vector3(-5, 0, 25);
                EnemyWallCamera = new Vector3(-6, 0, -258);

                break;

            case GameState.Level4:
                CurrentGameState = "Level4";
                Time.timeScale = 1.0f;

                break;

            case GameState.Level5Start:
                CurrentGameState = "Level5Start";
                MenuScript.DisableTutText();
                MenuScript.ChangeDialogue(10);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;

                //Generic level variables
                Resources = 300f;
                IncrementingResources = 5;
                IncrementialTimeRate = 1.8f;
                TimeToFinishLevel = 300f;
                EnemyWallHealth = 800f;
                FriendlyWallHealth = 600f;

                //Spawning variables
                EnemyUnitLimit = 8;
                FriendlyCurrentSpawnPosition = new Vector3(-83f, 1.64f, 30f);
                SpawnToPositionFriendly = new Vector3(90f, 1.64f, 230f);
                EnemyCurrentSpawnPosition = new Vector3(-83f, 1.64f, -332f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 8f;

                //Tower variables
                TowerDamageMultiplier = 0;
                TowerMissileDamage = 2.5f;
                TowerLookFriendly = new Vector3(-83f, 11f, 40f);
                TowerLookEnemy = new Vector3(-83f, 11f, -342.5f);
                TowerResourceGain = 3f;
                TowerResourceTimer = 2f;

                //Unit variables
                EnemyTankHealth = 80f;
                EnemyTankMissileDamage = 9f;
                EnemyJeepHealth = 50f;
                EnemyJeepMissileDamage = 6f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 35f;
                FriendlyJeepDamage = 2f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 105f;

                //Camera variables                
                CameraUpperLimit = -255f;
                CameraLowerLimit = -22f;
                CameraLeftLimit = 80f;
                CameraRightLimit = -90f;
                CameraMaxToClamp = 3.5f;
                FriendlyWallCamera = new Vector3(-5, 0, 25);
                EnemyWallCamera = new Vector3(-6, 0, -258);

                break;

            case GameState.Level5:
                CurrentGameState = "Level5";
                Time.timeScale = 1.0f;

                break;

            case GameState.Level6Start:
                CurrentGameState = "Level6Start";
                MenuScript.DisableTutText();
                MenuScript.ChangeDialogue(10);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;

                //Generic level variables
                Resources = 300f;
                IncrementingResources = 6;
                IncrementialTimeRate = 1.8f;
                TimeToFinishLevel = 300f;
                EnemyWallHealth = 150f;
                FriendlyWallHealth = 150f;

                //Spawning variables
                EnemyUnitLimit = 9;
                FriendlyCurrentSpawnPosition = new Vector3(-83f, 1.64f, 30f);
                SpawnToPositionFriendly = new Vector3(90f, 1.64f, 230f);
                EnemyCurrentSpawnPosition = new Vector3(-83f, 1.64f, -332f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 7f;

                //Tower variables
                TowerDamageMultiplier = 0;
                TowerMissileDamage = 3f;
                TowerLookFriendly = new Vector3(-83f, 11f, 40f);
                TowerLookEnemy = new Vector3(-83f, 11f, -342.5f);
                TowerResourceGain = 3f;
                TowerResourceTimer = 2f;

                //Unit variables
                EnemyTankHealth = 90f;
                EnemyTankMissileDamage = 9f;
                EnemyJeepHealth = 50f;
                EnemyJeepMissileDamage = 6f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 35f;
                FriendlyJeepDamage = 3f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 110f;

                //Camera variables                
                CameraUpperLimit = -190f;
                CameraLowerLimit = -5f;
                CameraLeftLimit = 80f;
                CameraRightLimit = -90f;
                CameraMaxToClamp = 3.5f;
                FriendlyWallCamera = new Vector3(-5, 0, 25);
                EnemyWallCamera = new Vector3(-6, 0, -258);

                break;

            case GameState.Level6:
                CurrentGameState = "Level6";
                Time.timeScale = 1.0f;

                break;

            case GameState.Level7Start:
                CurrentGameState = "Level7Start";
                MenuScript.DisableTutText();
                MenuScript.ChangeDialogue(10);
                MenuScript.DisableCentreText();
                Time.timeScale = 1f;

                //Generic level variables
                Resources = 300f;
                IncrementingResources = 7;
                IncrementialTimeRate = 1.5f;
                TimeToFinishLevel = 300f;
                EnemyWallHealth = 800f;
                FriendlyWallHealth = 600f;

                //Spawning variables
                EnemyUnitLimit = 9;
                FriendlyCurrentSpawnPosition = new Vector3(-83f, 1.64f, 30f);
                SpawnToPositionFriendly = new Vector3(90f, 1.64f, 230f);
                EnemyCurrentSpawnPosition = new Vector3(-83f, 1.64f, -332f);
                EnemySpawnLimit = 0f;
                EnemySpawnRate = 7f;

                //Tower variables
                TowerDamageMultiplier = 0;
                TowerMissileDamage = 3f;
                TowerLookFriendly = new Vector3(-83f, 11f, 33f);
                TowerLookEnemy = new Vector3(-83f, 11f, -342.5f);
                TowerResourceGain = 3f;
                TowerResourceTimer = 2f;

                //Unit variables
                EnemyTankHealth = 95f;
                EnemyTankMissileDamage = 10f;
                EnemyJeepHealth = 50f;
                EnemyJeepMissileDamage = 6f;
                FriendlyMissileDamage = 10f;
                FriendlyJeepHealth = 45f;
                FriendlyJeepDamage = 4f;
                JeepCost = 50;
                TankCost = 100;
                FriendlyTankHealth = 110f;

                //Camera variables                
                CameraUpperLimit = -230f;
                CameraLowerLimit = -5f;
                CameraLeftLimit = 80f;
                CameraRightLimit = -90f;
                CameraMaxToClamp = 3.5f;
                FriendlyWallCamera = new Vector3(-5, 0, 25);
                EnemyWallCamera = new Vector3(-6, 0, -240);

                break;

            case GameState.Level7:
                CurrentGameState = "Level7";
                Time.timeScale = 1.0f;

                break;
        }
    }

    public void Update()
    {
        UpdateResources();
    }
    //Updates the resources continuously based on current level.
    private void UpdateResources()
    {
        IncrementialTimeLimit += Time.deltaTime;
        if (IncrementialTimeLimit >= IncrementialTimeRate)
        {
            Resources += IncrementingResources;
            IncrementialTimeLimit = 0f;
        }
    }
}