using UnityEngine;
using System.Collections;

public class HealthStatus : MonoBehaviour {

	public float Health;
    public GameObject BrokenEnemyWall;
    public GameObject BrokenFriendlyWall;
    public GameObject TankExplosionParticles;
    public GameObject WallExplosionParticle;

    void Start() {
        //Start by setting the health of units and walls according to the settings in the GameManager
        if (gameObject.tag == "EnemyWall") {
            Health = GameManager.EnemyWallHealth;
        }

        if (gameObject.tag == "FriendlyWall") {
            Health = GameManager.FriendlyWallHealth;
        }

        if (gameObject.name == "Tank_team1(Clone)") {
            Health = GameManager.FriendlyTankHealth;
        }

        if (gameObject.name == "Tank_Enemy(Clone)") {
            Health = GameManager.EnemyTankHealth;
        }

        if (gameObject.name == "RebelJeep(Clone)") {
            Health = GameManager.FriendlyJeepHealth;
        }

        if (gameObject.name == "ArmyJeep(Clone)") {
            Health = GameManager.EnemyJeepHealth;
        }
    }

	void Update () {
        //Destroy the gameobject if it reaches 0 or less health, and it's not a wall
		if ( Health <= 0  && (gameObject.tag != "EnemyWall" && gameObject.tag != "FriendlyWall")) {
			Destroy(gameObject);

		    //if (gameObject.name == "Tank_team1(Clone)" || gameObject.name == "Tank_Enemy(Clone)") {
		        if (TankExplosionParticles) {
		            Instantiate(TankExplosionParticles, transform.position, Quaternion.identity);
		        }
		    //}
		}
        //If the GameObject is a wall, destroy it and instantiate the wall animation
	    if (Health <= 0 && gameObject.tag == "EnemyWall") {
	        Destroy(gameObject);
	        Instantiate(BrokenEnemyWall, transform.position, Quaternion.identity);
            Instantiate(WallExplosionParticle, transform.position, Quaternion.identity);
        }

	    if (Health <= 0 && gameObject.tag == "FriendlyWall") {
	        Destroy(gameObject);
	        Instantiate(BrokenFriendlyWall, transform.position, Quaternion.identity);
	        Instantiate(WallExplosionParticle, transform.position, Quaternion.identity);
	    }

	}
}
