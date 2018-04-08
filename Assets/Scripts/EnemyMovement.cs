using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour {

    public List<Transform> Targets;
    public Transform SelectedTarget;
    public float InRangeDistance;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private float _targetUnitRange = 100f;

    private void Start() {
        _navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        SelectedTarget = null;
        Targets = new List<Transform>();
        //Repeat the function that creates arrays and add targets to the list every 2 seconds, it will initiate 1 second after unit is spawned
        InvokeRepeating("AddTargetsToList", 1, 2);
    }

    private void Update() {
        //If the target is a tower and it has reached 200 ControlHealth (so enemy controlled), clear the target
        if (SelectedTarget != null) {
            if (SelectedTarget.tag == "Tower" && SelectedTarget.GetComponent<TurretController>().ControlHealth >= 200) {
                SelectedTarget = null;
            }
        }
        BestTarget();
        //Possibly integrate the clearing of the list somewhere else later
        ClearList();
    }

    void FixedUpdate() {
        MoveToTarget();
    }

    public void AddTargetsToList() {
        //Create an array of towers that the Player controls and add them to the target list - does nothing if the target list currently contain the target
        GameObject[] towersInList = GameObject.FindGameObjectsWithTag("Tower");
        foreach(GameObject tower in towersInList) {
            if(tower.GetComponent<TurretController>().ControlHealth < 200) {
                if (!Targets.Contains(tower.transform)) {
                    AddTarget(tower.transform);
                }
            }
        }
        //Create an array of Player units and add them to the target list, unless the target is already there
        GameObject[] unitsInList = GameObject.FindGameObjectsWithTag("Team1");
        foreach(GameObject unit in unitsInList) {
            if(!Targets.Contains(unit.transform)) {
                AddTarget(unit.transform);
            }
        }
        //Clear the arrays
        Array.Clear(towersInList, 0, towersInList.Length);
        Array.Clear(unitsInList, 0, unitsInList.Length);
    }

    public void AddTarget(Transform target) {
        Targets.Add(target);
    }

    public Transform BestTarget() {
        float closestDistanceSqr = Mathf.Infinity;
        //Find the units current position
        Vector3 currentPosition = transform.position;
        //Iterate through the Targets list to find the best target
        foreach (Transform potentialTarget in Targets) {
            if (potentialTarget) {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                //First check if there is a Player unit within a certain distance
                if (potentialTarget.tag == "Team1") {
                    float distanceToUnit = Vector3.Distance(potentialTarget.transform.position, currentPosition);
                    if (distanceToUnit < _targetUnitRange) {
                        if (SelectedTarget.tag == "Team1") {
                            //Compare the Player units within distance if there are more than one
                            float tempDistance = Vector3.Distance(SelectedTarget.transform.position, currentPosition);
                            float newDistance = Vector3.Distance(potentialTarget.transform.position, currentPosition);
                            if (newDistance < tempDistance) {
                                SelectedTarget = potentialTarget;
                            }
                        }
                        else if (!SelectedTarget || SelectedTarget.tag == "Tower") {
                            SelectedTarget = potentialTarget;
                        }
                    }
                    return SelectedTarget;
                }
                //Go through and check the distance to all Towers that are currently not in Enemy control,
                // and choose the closest one.
                if (dSqrToTarget < closestDistanceSqr && potentialTarget.tag == "Tower" &&
                    potentialTarget.GetComponent<TurretController>().ControlHealth < 200) {
                    //If there's currently no selected target, first target in the list is selected
                    if (SelectedTarget == null) {
                        SelectedTarget = potentialTarget;
                    }
                    else {
                        //Check if any potential targets are closer than the first one that was found in the list
                        float initialDistance = Vector3.Distance(SelectedTarget.transform.position, transform.position);
                        float potentialDistance = Vector3.Distance(potentialTarget.transform.position,
                            transform.position);
                        if (potentialDistance < initialDistance) {
                            SelectedTarget = potentialTarget;
                        }
                    }
                }
                //Lastly, if there are no units within target distance, and all towers belong to the Enemy, find a Player unit anywhere on the map
                //This needs to be rewritten so it will pick the closest and not just the first.
                else if (dSqrToTarget < closestDistanceSqr && potentialTarget.tag == "Team1") {
                    SelectedTarget = potentialTarget;
                }
            }
        }
        return SelectedTarget;
    }

    public void MoveToTarget() {
        //Check if the tutorial has moved past the initial steps - for later levels the tutorial manager needs to be disabled
        if (GameManager.CurrentGameState == "Tutorial2") {
            _navMeshAgent.SetDestination(new Vector3(-47f, -0.2f, -57f));
        }
        //Move the unit towards the current target
        if (SelectedTarget) {
            if (SelectedTarget.tag == "Tower") {
                float distanceToTarget = Vector3.Distance(transform.position, SelectedTarget.transform.position);
                if (distanceToTarget > InRangeDistance) {
                    _navMeshAgent.Resume();
                    _navMeshAgent.SetDestination(SelectedTarget.transform.position);
                }
                if (distanceToTarget < InRangeDistance) {
                    _navMeshAgent.velocity = Vector3.zero;
                    _navMeshAgent.Stop();
                }
            }

            if (SelectedTarget.tag == "Team1") {
                _navMeshAgent.SetDestination(SelectedTarget.transform.position);
            }
        }
    }

    private void ClearList() {
        //Clear the list if the unit currently has a target
        if(SelectedTarget) {
            Targets.Clear();
        }
    }
}