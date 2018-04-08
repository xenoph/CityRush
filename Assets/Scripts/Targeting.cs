using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeting : MonoBehaviour {

    public bool haveFoundTarget;
    public GameObject target;
    public string targetTag = null;
    //public float radius;

    void Update() {
        //Checks every frame if the unit has found a frame, if so set the haveFoundTarget state as true
        if(FindClosestEnemy() == null) {
            haveFoundTarget = false;
        }
        else {
            haveFoundTarget = true;
            
        }
    }

    //Function that will find the closest enemy to the unit
    public GameObject FindClosestEnemy() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closestUnit = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach(GameObject go in gos) {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance) {
                closestUnit = go;
                distance = curDistance;
            }
        }
        return closestUnit;
    }
}