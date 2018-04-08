using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public AudioSource missileSound;
    public GameObject tankMissile;
    public float missileSpeed;
    public float reloadTime = 1f;
    public float shotDuration = 1f;

    private float fireTime = 0;


    TurretScript turretScript;
    Targeting targetScript;

    void Start() {
        turretScript = GetComponent<TurretScript>();
        targetScript = GetComponent<Targeting>();
    }

    public void FireMissile() {
        if (targetScript.haveFoundTarget == true) {
            //Checks if it can shoot according to the time set for reloading.
            if (Time.time > fireTime) {
                fireTime = Time.time + reloadTime;

                //There's a better way of doing this, but my eyes hurt. Find out the closest target, check range, and fire.
                //Repeating code is bad.
                GameObject target = targetScript.FindClosestEnemy();
                if (target != null) {
                    Vector3 diff = target.transform.position - transform.position;
                    float curDistance = diff.sqrMagnitude;

                    if (MenuScript.LevelFinished == false) {
                        if (curDistance < turretScript.attackDistance) {
                            GameObject newMissile =
                                Instantiate(tankMissile, transform.Find("TankShellSpawner").transform.position,
                                    Quaternion.identity) as GameObject;
                            newMissile.GetComponent<Rigidbody>().velocity = (transform.forward*missileSpeed);
                            if (transform.parent.name == "RebelJeep(Clone)") {
                                newMissile.tag = ("JeepMissile");
                            }
                            else if (transform.parent.name == "ArmyJeep(Clone)") {
                                newMissile.tag = ("ArmyJeepMissile");
                            }
                            else {
                                newMissile.tag = (transform.parent.gameObject.tag + "missile");
                            }
                            //.velocity also works. Probably better than AddForce. Still shoot upwards - problem with the targetting/turret?

                            //You add the duration of the missile to the turret it is attached to, but the destruction of the missile happens in the missile script.
                            //That way it can easily be changed depending on what type of turret is being used - more useful when this is made into a new script.
                            missileController missileScript = newMissile.GetComponent<missileController>();
                            if (missileScript.Duration <= 0) {
                                missileScript.Duration = shotDuration;
                            }

                            if (missileSound && transform.parent.name == "Tank_team1(Clone)" ||
                                transform.parent.name == "Tank_Enemy(Clone)") {
                                GetComponent<AudioSource>().volume = Random.Range(0.6f, 7);
                                GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                                GetComponent<AudioSource>().Play();
                            }
                            if (missileSound && transform.parent.name == "RebelJeep(Clone)" ||
                                transform.parent.name == "ArmyJeep(Clone)") {
                                GetComponent<AudioSource>().volume = Random.Range(0.1f, 0.2f);
                                GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
                                GetComponent<AudioSource>().Play();
                            }
                        }
                    }
                }
            }
        }
    }
}