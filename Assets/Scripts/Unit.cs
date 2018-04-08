using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public bool selected = false;
    public GameObject outline;

    public float speed = 5;
    public float stopDistanceOffset = 0.5f;

    public float rotateSpeed = 5;
    public Vector3 rotationDest;
    public float rotationOffset = 2;
    public bool destinationSet = false;

    public bool unitSpawned = false;
    public float time;

    void Start() {
        outline.GetComponent<Renderer>().enabled = false;
    }

    void Update() {

        if(Movement.Selected == gameObject) {
            selected = true;
            outline.GetComponent<Renderer>().enabled = true;
        }

        if(GetComponent<Renderer>().isVisible && Input.GetMouseButton(0)) {

            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);

            camPos.y = CameraOperator.InvertMouseY(camPos.y);
            selected = CameraOperator.selection.Contains(camPos, true);

            if(selected)
                outline.GetComponent<Renderer>().enabled = true;
            else
                outline.GetComponent<Renderer>().enabled = false;
        }

        
        //If the unit is spawned, move it to the Rally point
        if(!unitSpawned && gameObject.tag == "Team1") {
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(Movement.RallyPoint.transform.position);
            unitSpawned = true;
            destinationSet = true;
        }
    }
    
    void FixedUpdate() {
        if (selected && Input.GetMouseButtonUp(1)) {
            Vector3 destination = CameraOperator.GetDestination();
            if (destination != Vector3.zero) {
                //Debug.Log(destination);
                gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
                gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(destination);
                destinationSet = true;
            }
        }
    }
}
