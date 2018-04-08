using UnityEngine;
using System.Collections;

public class missileController : MonoBehaviour {

    float _startTime;
    public float Damage;
    public GameObject Explosion;

    [HideInInspector] public float Duration = 2f;

    void Start() {
        //Set the damage on the missile according to the game manager
        if (gameObject.tag == "CPmissile") {
            Damage = GameManager.TowerMissileDamage;
        }
        if (gameObject.tag == "Team1missile") {
            Damage = GameManager.FriendlyMissileDamage;
        }
        if (gameObject.tag == "Team2missile") {
            Damage = GameManager.EnemyTankMissileDamage;
        }
        if (gameObject.tag == "JeepMissile") {
            Damage = GameManager.FriendlyJeepDamage;
        }
        if (gameObject.tag == "ArmyJeepMissile") {
            Damage = GameManager.EnemyJeepMissileDamage;
        }
        if (gameObject.tag == "WallMissile") {
            Damage = 1f;
        }
        _startTime = Time.time;
    }

    void FixedUpdate() {
        //If the missile miss the target, destroy it after a certain time limits
        if (Time.time >= _startTime + Duration && gameObject.tag != "WallMissile") {
            Destroy(gameObject);
        }
    }

    //Checks if it hits the target, then destroys the target (doesn't work somehow) and the missile.
    void OnTriggerEnter(Collider other) {
        if (this.transform.tag == "Team2missile" || transform.tag == "CPmissile" || transform.tag == "ArmyJeepMissile") {
            if (other.gameObject.tag == "Team1") {
                DoHit(other.gameObject);
                //Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject); //Destroyes the missile after triggering the collider
            }
        }
        if (this.transform.tag == "Team1missile" || transform.tag == "CPmissile" || transform.tag == "JeepMissile") {
            if (other.gameObject.tag == "Team2") {
                DoHit(other.gameObject);
                //Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

        if (transform.tag == "WallMissile") {
            if (other.gameObject.tag == "EnemyWall" || other.gameObject.tag == "FriendlyWall") {
                DoHit(other.gameObject);
                Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    //Destroys the missile if it goes out of camera view. Simples.
    void OnBecameInvisible() {
        Destroy(gameObject);
    }

    private void DoHit(GameObject thisObject) {
        if (thisObject.GetComponent<HealthStatus>()) {
            if (thisObject.tag == "EnemyWall" && gameObject.tag == "WallMissile") {
                //Change both the health script and GameManager health variable. Surely this can be fixed? Works for now.
                thisObject.GetComponent<HealthStatus>().Health -= (1*GameManager.TowerDamageMultiplier);
                GameManager.EnemyWallHealth -= (1*GameManager.TowerDamageMultiplier);
            }
            else if (thisObject.tag == "FriendlyWall" && gameObject.tag == "WallMissile") {
                thisObject.GetComponent<HealthStatus>().Health -= 1;
                GameManager.FriendlyWallHealth -= 1;
            }
            else {
                thisObject.GetComponent<HealthStatus>().Health -= Damage;
            }

        }
    }
}

