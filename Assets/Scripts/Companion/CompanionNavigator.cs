﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionNavigator : MonoBehaviour {

    private NavMeshAgent _navAgent;
    private Rigidbody _rigidbody;

    public void Awake() {
        _navAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = true;
    }

    //set destination for the navmesh agent
    public void SetDestination(Vector3 destination) {
        _navAgent.SetDestination(destination);
    }

    //enable or disable the navmesh agent
    public void SetAgentStatus(bool status) {
        _navAgent.enabled = status;
    }

    //push into the given direction and force
    public void Push(Vector3 direction, float force) {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(direction.normalized * force);
        _rigidbody.isKinematic = true;
    }
}