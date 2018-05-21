﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//helper component for keeping track of the objectives
public class CompanionObjectiveTracker : MonoBehaviour {

    public Transform objectiveLog;

    private List<CompanionObjective> _mainObjectives;
    private List<CompanionObjective> _sideObjectives;

    private CompanionObjective _currentObjective;

    private int _activeMainObjectiveIndex;

    public void Awake() {
        if (objectiveLog == null) Debug.LogWarning("WARNING: ObjectiveLog reference in CompanionObjectiveTracker is missing!");

        _mainObjectives = new List<CompanionObjective>();
        _sideObjectives = new List<CompanionObjective>();

        _activeMainObjectiveIndex = 0;

        CompanionObjective[] allObjectives = objectiveLog.GetComponentsInChildren<CompanionObjective>();

        //sorting out objectives
        for(int i = 0; i < allObjectives.Length; i++) {
            if (allObjectives[i].objectiveType == CompanionObjectiveType.Main) _mainObjectives.Add(allObjectives[i]);
            else if (allObjectives[i].objectiveType == CompanionObjectiveType.Side) _sideObjectives.Add(allObjectives[i]);
        }

        _currentObjective = _mainObjectives[_activeMainObjectiveIndex];
    }

    public void SetCurrentObjective(CompanionObjective objective) {
        _currentObjective = objective;
    }

    public CompanionObjective GetCurrentObjective() {
        return _currentObjective;
    }

    public CompanionObjective GetNextMainObjective() {
        return _mainObjectives[_activeMainObjectiveIndex];
    }

    public CompanionObjective GetClosestSideObjective() {
        CompanionObjective closestObjective = null;
        float prevMagnitude = 0f;

        //using sqrMagnitude for better performance, since we are only comparing them against each other
        for (int i = 0; i < _sideObjectives.Count; i++) {
            if (_sideObjectives[i].IsCompleted()) continue;

            float magnitude = (_sideObjectives[i].transform.position - transform.position).sqrMagnitude;

            if(closestObjective == null || magnitude < prevMagnitude) {
                closestObjective = _sideObjectives[i];
                prevMagnitude = magnitude;
            }
        }

        return closestObjective;
    }

    public float GetObjectiveDistance(CompanionObjective objective) {
        return (objective.transform.position - transform.position).magnitude;
    }
}
