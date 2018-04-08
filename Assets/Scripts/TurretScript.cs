using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {
	
	//public GameObject Target;
	public GameObject tank;
	public Transform TankShellSpawner;
	public float attackDistance = 400f;

    WeaponController weaponScript;
    Targeting targetScript;

    void Start () {
        weaponScript = GetComponent<WeaponController>();
        targetScript = GetComponent<Targeting>();
    }
	
	void Update () {
    //Makes the turret rotate and look towards the target
	TurretLookAtTarget();
    //Checks if the targetscript have found a target, if so tells the weaponscript to fire a missile
	if (targetScript.haveFoundTarget == true) {
            weaponScript.FireMissile();


        }
	}

	public void TurretLookAtTarget(){

		if(targetScript.haveFoundTarget == false){
			transform.rotation = Quaternion.identity;
		}else{
		//Finding Closest Enemy
		GameObject target = targetScript.FindClosestEnemy();

			//Getting The Distance Between Closest Enemy and your unit!!
			if(target != null){
			Vector3 diff = target.transform.position - transform.position;
			float curDistance = diff.sqrMagnitude;
			//Debug.Log(curDistance);

			//Checks if distance is under 700 from target, if it is, it will target the enemy with the turret!!
			if(curDistance < attackDistance){
			transform.LookAt(target.transform);
			}

			}else{
			
			}
		}
	}
}
