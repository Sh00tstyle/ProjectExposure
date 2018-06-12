﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingGlobal : MonoBehaviour {

    public GameObject goalIndicator;
    public GameObject fish;
    public int fishCount; 

    private BoxCollider _fishTank;
    public static Vector3 goal;
    private int[] _fishies;

	// Use this for initialization
	void Start () {
        _fishTank = GetComponent<BoxCollider>();
        _fishies = new int[fishCount];

        for (int i = 0; i < _fishies.Length; i++)
        {
            Instantiate(fish, new Vector3(Random.Range(_fishTank.bounds.min.x, _fishTank.bounds.max.x), Random.Range(_fishTank.bounds.min.y, _fishTank.bounds.max.y), Random.Range(_fishTank.bounds.min.z, _fishTank.bounds.max.z)), Quaternion.Euler(new Vector3(-90,0,0))); 
        }

        goal = new Vector3(Random.Range(_fishTank.bounds.min.x, _fishTank.bounds.max.x), Random.Range(_fishTank.bounds.min.y, _fishTank.bounds.max.y), Random.Range(_fishTank.bounds.min.z, _fishTank.bounds.max.z));
        StartCoroutine("FindNewGoalPosWithDelay", 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if(goalIndicator != null)
        goalIndicator.transform.position = goal; 
	}

    IEnumerator FindNewGoalPosWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            NewGoalPos(); 
        }
    }

    private void NewGoalPos()
    {
        goal = new Vector3(Random.Range(_fishTank.bounds.min.x, _fishTank.bounds.max.x), Random.Range(_fishTank.bounds.min.y, _fishTank.bounds.max.y), Random.Range(_fishTank.bounds.min.z, _fishTank.bounds.max.z));
    }


    public Vector3 Goal
    {
        get { return goal; }
    }
}
