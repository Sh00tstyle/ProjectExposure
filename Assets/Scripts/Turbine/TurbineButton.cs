﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class TurbineButton : MonoBehaviour {

    public SpringJoint _springJoint;

    [SerializeField]
    private UnityEvent _turbineButtonEvent;

    private TubineButtonActivate _turbineButtonPressed; 

	// Use this for initialization
	void Start () {
        _turbineButtonPressed = GetComponentInChildren<TubineButtonActivate>(); 		
    }
	
	// Update is called once per frame
	void Update () {
	}

   private void OnCollisionStay(Collision other)
    {
        if (other.collider.tag == "Player")
        {
            Debug.Log("Is being Pressed");
            if (_turbineButtonPressed.Active)
            {
                _turbineButtonEvent.Invoke();
                Debug.Log("Invoke Event");
            }
        }
    }


}