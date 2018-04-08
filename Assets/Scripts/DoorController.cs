using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public GameObject doorLeft;
	public GameObject doorRight;
	public bool unitInside;




	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	


	}

	void OnTriggerEnter(Collider other) {

		if(other.tag == "Team1" && !unitInside){
			doorLeft.GetComponent<Animator>().Play("DoorOpen");
			doorRight.GetComponent<Animator>().Play("DoorOpen");
			unitInside = true;
		}

	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Team1" && unitInside){
			doorLeft.GetComponent<Animator>().Play("DoorClose");
			doorRight.GetComponent<Animator>().Play("DoorClose");
			unitInside = false;
		}
	}
}
