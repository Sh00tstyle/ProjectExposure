﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSript : MonoBehaviour {

    public VacuumArea vacuumArea;
    public LayerMask suckableLayer; 
    private BoxCollider grabCollider; 
    public float suckSpeed;
    private float suckTime; 

	// Use this for initialization
	void Start () {
        grabCollider = GetComponent<BoxCollider>(); 
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            suckTime = 0; 
        }
		if (Input.GetKey(KeyCode.F))
        {
            Suck(); 
        }
        if (OVRInput.Get(OVRInput.Button.One))
        {
            Suck(); 
        }
        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            suckTime = 0; 
        }
	}

    private void Suck()
    {
        suckTime += 0.25f * Time.deltaTime;
       // print(suckTime); 
        if(suckTime > 1)
        {
            suckTime = 0; 
        }
        if(suckTime < 0)
        {
            suckTime = 1; 
        }

        for (int i = 0; i < vacuumArea.suckableObjectsList.Count; i++)
        {
            print(vacuumArea.suckableObjectsList[i].position);
            print(transform.position);
            Vector3 suckDir = ( transform.position - vacuumArea.suckableObjectsList[i].position ).normalized;
            //Vector3 suckDist = vacuumArea.suckableObjectsList[i].position - transform.position; 
            vacuumArea.suckableObjectsList[i].position += suckDir / 10; 
        }
        CheckToSuck(); 
    }

    private void CheckToSuck()
    {
        Vector3 center = transform.position + grabCollider.center;
        Vector3 halfExtends = grabCollider.bounds.extents;
        Collider[] grabbableObjects = Physics.OverlapBox(center, halfExtends, Quaternion.identity,suckableLayer);
        print(grabbableObjects.Length); 
        for (int i = 0; i < grabbableObjects.Length; i++)
        {
            Destroy(grabbableObjects[i].gameObject);
        }
    }
}