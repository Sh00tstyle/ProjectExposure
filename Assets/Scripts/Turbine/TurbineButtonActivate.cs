﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineButtonActivate : MonoBehaviour {

    private bool _active; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "TurbineButton")
        {
            Debug.Log("Button is pressed");
            _active = true; 
        }
    }

    

    public bool Active
    {
        get { return _active; }
    }
}