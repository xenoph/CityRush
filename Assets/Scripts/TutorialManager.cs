using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	public int tutorialStep;
	//public GameObject buildTankHighlight;
	//public GameObject UITimerHighlight;
	public GameObject greyUI;

	public GameObject rocks;


	//public GameObject TutTextTimer;
	//public GameObject TutTextTank;
	public GameObject TutTextMain;

	public GameObject Turret;
	public bool Tut2 = false;
	public bool Tut3 = false;


	public GameObject Dialogue_01;
	public GameObject Dialogue_Text;
	public Texture Dialogue_02;
	public Texture Dialogue_03;
	public Texture Dialogue_BG;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(tutorialStep == 0){
			//Dialogue_01.GetComponent<RawImage>().enabled = false;
			//Dialogue_Text.GetComponent<Text>().enabled = false;
		}

		if(tutorialStep == 4){
			Dialogue_Text.GetComponent<Text>().enabled = true;
			Dialogue_Text.GetComponent<Text>().text = "You get a Damage multiplier when you control more turrets!";
			Dialogue_01.GetComponent<RawImage>().texture = Dialogue_BG;
		}

		if(tutorialStep == 3){
            print("Tut3");
			rocks.SetActive(false);
			Dialogue_01.GetComponent<RawImage>().enabled = true;
			Dialogue_01.GetComponent<RawImage>().texture = Dialogue_02;
		}

		if(tutorialStep == 2){
			Dialogue_01.GetComponent<RawImage>().enabled = false;
			Dialogue_Text.GetComponent<Text>().enabled = false;
		}

		if(tutorialStep == 1){
			Dialogue_01.GetComponent<RawImage>().enabled = true;
			Dialogue_Text.GetComponent<Text>().enabled = true;
		}

		if(Turret.GetComponent<TurretController>().TargetTeam == "Team2" && Tut3 == false){
			tutorialStep = 3;
			Tut3 = true;
		}

		if(Turret.GetComponent<TurretController>().ControlHealth < 200 && Tut2 == false){
			tutorialStep = 2;
			//UITimerHighlight.GetComponent<RawImage>().enabled = false;
			//TutTextTimer.GetComponent<Text>().enabled = false;
			Tut2 = true;
		}

		//Start of the game, wait for the player to press build tank
		if(tutorialStep == 0){
			Time.timeScale = 0f;
			//UITimerHighlight.GetComponent<RawImage>().enabled = false;
			//TutTextTimer.GetComponent<Text>().enabled = false;
		}

		//Set time scale to 1 and disable all Tut ui elements, and add Tut timer.
		if(tutorialStep == 1){
			Time.timeScale = 1.0f;

			TutTextMain.GetComponent<Text>().enabled = false;
			greyUI.GetComponent<RawImage>().enabled = false;

			//TutTextTank.GetComponent<Text>().enabled = false;
			//buildTankHighlight.GetComponent<RawImage>().enabled = false;

			//UITimerHighlight.GetComponent<RawImage>().enabled = true;
			//TutTextTimer.GetComponent<Text>().enabled = true;
        }
	}
}
