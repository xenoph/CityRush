using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class Movement : MonoBehaviour {

    public static GameObject Selected;
    public static GameObject PlayerSelectedTarget;
    public static GameObject RallyPoint;
    public GameObject Marker;

    void Awake() {
        RallyPoint = GameObject.Find("RallyPoint");
    }

    void Update() {
        //Check if the Player clicks his mouse and execute the appropriate function
        if (Input.GetButtonDown("Fire1")) {
            SelectUnit();
        }

        if (Input.GetButtonUp("Fire2")) {
            MouseEffect();
        }

        if(Input.GetKey("r") && Input.GetMouseButtonUp(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Ground") {
                RallyPoint.transform.position = hit.point;
            }
        }
    }

    //If the Player left-clicks on a friendly Unit, set that unit as Selected
    void SelectUnit() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.tag == "Team1") {
                Selected = hit.collider.gameObject;
            }
            //Clear the selection if the Player clicks anywhere that isn't a unit
            if (hit.collider.tag != "Team1") {
                Selected = null;
            }
        }
    }

    //If the Player right-clicks, place an effect on the ground where he is sending his units.
    //NOTE; add in a check on if there are any units selected, otherwise don't place the effect.
    void MouseEffect() {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 worldPos;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            worldPos = hit.point;
        }
        else {
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        }

        GameObject[] mouseEffectObjects = GameObject.FindGameObjectsWithTag("MouseEffect");
        if (hit.collider.tag == "Ground") {
            if(mouseEffectObjects.Length > 0) {
                for(int i = 0 ; i < mouseEffectObjects.Length ; i++) {
                    Destroy(mouseEffectObjects[i]);
                }
                Instantiate(Marker, worldPos, Quaternion.identity);
            } else {
                Instantiate(Marker, worldPos, Quaternion.identity);
            }
        }
    }
}