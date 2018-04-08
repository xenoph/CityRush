using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretController : MonoBehaviour {
    public int ControlHealth;

    public string TargetTeam;
    public float AttackDistance = 400;

    public GameObject ControlPointOutline;
    public GameObject ControlPoint;
    public GameObject TankMissile;
    public GameObject WallMissile;
    public float MissileSpeed;

    public Texture OutlineBlue;
    public Texture OutlineRed;
    public GameObject CPointText;

    private bool _team2Control = false;
    private bool _team1Control = false;

    public Transform[] TankShellSpawner;
    private bool _turretShooting;

    public float Timer = 5f;
    private float _resourceLimit;
    private float _captureTimer;
    private float _captureRate = 0.1f;
    private bool _isCapturing;
    public float SomeTime;

    public GameObject TurretWeapon;
    public bool MultiplierAdded;
    public AudioSource TurretGunFire1;
    public AudioSource TurretGunFire2;

    private bool _countIsChecked;
    private bool _enemyIsTakingOver;

    public string TowerTriggerTag;

    private List<GameObject> _insideTrigger;

    void Start() {
        _insideTrigger = new List<GameObject>();
        FindClosest();
        InvokeRepeating("CheckUnitCount", 1, 1);
    }

    void Update() {
        if(_isCapturing == true && Time.time - _captureTimer > _captureRate) {
            if(TowerTriggerTag == "Team1") {
                if(ControlHealth <= 200 && !(ControlHealth <= 0)) {
                    ControlHealth -= 1;
                    _captureTimer = Time.time;
                    CPointText.GetComponent<TextMesh>().text = ControlHealth.ToString();
                }
            }

            if(TowerTriggerTag == "Team2") {
                if(ControlHealth >= 0 && !(ControlHealth >= 200)) {
                    ControlHealth += 1;
                    _captureTimer = Time.time;
                    CPointText.GetComponent<TextMesh>().text = ControlHealth.ToString();
                }
            }
            _insideTrigger.RemoveAll(item => item == null);
        }

        ControlPointChanger();

        if(_team1Control) {
            _resourceLimit += Time.deltaTime;
            if(_resourceLimit > GameManager.TowerResourceTimer) {
                GameManager.Resources += GameManager.TowerResourceGain;
                _resourceLimit = 0f;
            }
        }

        if(ControlHealth == 0) {
            if (_enemyIsTakingOver) {
                _enemyIsTakingOver = false;
                MenuScript.ChangeDialogue(13);
            }
            _team1Control = true;
            _team2Control = false;
            TargetTeam = "Team2";

            if(GameManager.CurrentGameState == "Tutorial2") {
                GameManager.Instance.SetGameState(GameState.Tutorial3);
            }

            if(GameManager.CurrentGameState == "Tutorial3") {
                if(_countIsChecked == false) {
                    StartCoroutine(MenuScript.CheckTurretCountTutorial());
                    _countIsChecked = true;
                }
            }

            if(MultiplierAdded == false) {
                GameManager.TowerDamageMultiplier += 1;
                MultiplierAdded = true;
            }
        }

        if(ControlHealth == 200) {
            _team1Control = false;
            _team2Control = true;
            TargetTeam = "Team1";

            if(MultiplierAdded) {
                GameManager.TowerDamageMultiplier -= 1;
                MultiplierAdded = false;
            }
            if(GameManager.CurrentGameState == "Level1Start" || GameManager.CurrentGameState == "Level1") {
                if (MenuScript.EnemyIsFiring == false) {
                    MenuScript.ChangeDialogue(6);
                    MenuScript.EnemyIsFiring = true;
                }
            }
        }

        if(MenuScript.LevelFinished == false) {
            GameObject target = FindClosest();
            if(target != null) {
                Vector3 diff = target.transform.position - transform.position;
                float curDistance = diff.sqrMagnitude;

                if(curDistance < AttackDistance) {
                    TurretWeapon.transform.LookAt(target.transform.position);

                    if(SomeTime < Time.time) {
                        SomeTime = Time.time + 0.06f;
                        if(TargetTeam == "Team1" && _team2Control && ControlHealth > 0) {
                            if(Time.time > Timer) {
                                Timer = Time.time + 0.5f;
                                ShootTurret(TankMissile, TankShellSpawner, MissileSpeed);
                            }
                        }

                        if(TargetTeam == "Team2" && _team1Control && ControlHealth >= 0) {
                            if(Time.time > Timer) {
                                Timer = Time.time + 0.5f;
                                ShootTurret(TankMissile, TankShellSpawner, MissileSpeed);
                            }
                        }
                    }
                }
            }
        }

        if(MenuScript.LevelFinished == false) {
            if(UnitInsideRange() == false) {
                if(ControlHealth >= 200 || ControlHealth <= 0) {

                    if(TargetTeam == "Team2") {
                        TurretWeapon.transform.LookAt(GameManager.TowerLookEnemy);

                        if(Time.time > Timer) {
                            Timer = Time.time + 1.5f;

                            if(_turretShooting == false) {
                                GameObject newMissile =
                                    Instantiate(WallMissile, TankShellSpawner[0].transform.position, Quaternion.identity)
                                        as
                                        GameObject;
                                newMissile.GetComponent<Rigidbody>().velocity = (TurretWeapon.transform.forward *
                                                                                 MissileSpeed);
                                _turretShooting = true;
                            } else {
                                GameObject newMissile =
                                    Instantiate(WallMissile, TankShellSpawner[1].transform.position, Quaternion.identity)
                                        as
                                        GameObject;
                                newMissile.GetComponent<Rigidbody>().velocity = (TurretWeapon.transform.forward *
                                                                                 MissileSpeed);
                                _turretShooting = false;
                            }
                        }
                    }

                    if(GameManager.CurrentGameState != "Tutorial1" && GameManager.CurrentGameState != "Tutorial2" &&
                        GameManager.CurrentGameState != "Tutorial3" && GameManager.CurrentGameState != "Tutorial4") {
                        if(TargetTeam == "Team1") {
                            TurretWeapon.transform.LookAt(GameManager.TowerLookFriendly);
                            if(Time.time > Timer) {
                                Timer = Time.time + 1.5f;

                                if(_turretShooting == false) {
                                    GameObject newMissile =
                                        Instantiate(WallMissile, TankShellSpawner[0].transform.position,
                                            Quaternion.identity)
                                            as
                                            GameObject;
                                    newMissile.GetComponent<Rigidbody>().velocity = (TurretWeapon.transform.forward *
                                                                                     MissileSpeed);
                                    _turretShooting = true;
                                } else {
                                    GameObject newMissile =
                                        Instantiate(WallMissile, TankShellSpawner[1].transform.position,
                                            Quaternion.identity)
                                            as
                                            GameObject;
                                    newMissile.GetComponent<Rigidbody>().velocity = (TurretWeapon.transform.forward *
                                                                                     MissileSpeed);
                                    _turretShooting = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        StartCoroutine(CheckForEnemyTakeover());
    }

    public void ControlPointChanger() {
        if(ControlHealth == 0) {
            ControlPoint.GetComponent<Renderer>().material.color = Color.white;
            ControlPoint.GetComponent<Renderer>().material.mainTexture = OutlineBlue;
        }


        if(ControlHealth == 100) {
            ControlPoint.GetComponent<Renderer>().material.color = Color.black;
        }


        if(ControlHealth == 200) {
            ControlPoint.GetComponent<Renderer>().material.color = Color.white;
            ControlPoint.GetComponent<Renderer>().material.mainTexture = OutlineRed;
        }
    }

    //Find Closest Enemy Function!!!
    GameObject FindClosest() {
        GameObject[] gos;
        if(TargetTeam != "") {
            gos = GameObject.FindGameObjectsWithTag(TargetTeam);
            if(gos.Length > 0) {
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;
                foreach(GameObject go in gos) {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;

                    if(curDistance < distance) {
                        closest = go;
                        distance = curDistance;
                    }
                }
                return closest;
            }
        }
        return null;
    }

    void ShootTurret(GameObject missilePrefab, Transform[] turrets, float missileSpeed) {
        for(int i = 0 ; i < turrets.Length ; i++) {
            GameObject newMissile =
                Instantiate(missilePrefab, turrets[i].transform.position, Quaternion.identity) as GameObject;
            if(newMissile != null)
                newMissile.GetComponent<Rigidbody>().velocity = (TurretWeapon.transform.forward * missileSpeed);
            if(missilePrefab.name == "TankShot") {
                if(newMissile != null) newMissile.tag = (transform.parent.gameObject.tag + "missile");
            }
        }
    }

    bool UnitInsideRange() {
        GameObject unit = FindClosest();
        bool unitInside = false;
        if(unit != null) {
            Vector3 diff = unit.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < AttackDistance) {
                unitInside = true;
            }
        }
        return unitInside;
    }

    void CheckUnitCount() {
        _insideTrigger.RemoveAll(item => item == null);
        int friendlyCount = 0;
        int enemyCount = 0;

        foreach(GameObject other in _insideTrigger) {
            if(other.tag == "Team1") {
                friendlyCount += 1;
            }
            if(other.tag == "Team2") {
                enemyCount += 1;
            }
        }

        if(friendlyCount > enemyCount) {
            TowerTriggerTag = "Team1";
            _isCapturing = true;
            _captureTimer = Time.time - _captureRate;
        }
        if(enemyCount > friendlyCount) {
            TowerTriggerTag = "Team2";
            _isCapturing = true;
            _captureTimer = Time.time - _captureRate;
        }
        if(enemyCount == friendlyCount) {
            TowerTriggerTag = null;
            _isCapturing = false;
        }
    }

    void OnTriggerEnter(Collider other) {
        if(_insideTrigger.Contains(other.gameObject)) {
            return;
        }
        if(other.tag != "Team1" && other.tag != "Team2") {
            return;
        }
        if(other.tag == "Team1" || other.tag == "Team2") {
            _insideTrigger.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        if(_insideTrigger.Contains(other.gameObject)) {
            _insideTrigger.Remove(other.gameObject);
        }
    }

    IEnumerator CheckForEnemyTakeover() {
        int initialValue = ControlHealth;
        if (ControlHealth == 0) {
            yield return new WaitForSeconds(2);
            if (ControlHealth > initialValue) {
                MenuScript.ChangeDialogue(12);
                _enemyIsTakingOver = true;
            }
        }
    }
}