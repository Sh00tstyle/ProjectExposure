﻿using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : MonoBehaviour
{

    [SerializeField]
    [EventRef]
    private string _activateSound;
    [SerializeField]
    [EventRef]
    private string _errorSound;

    [SerializeField]
    [EventRef]
    private string _turbineSound;

    [SerializeField]
    private GameObject _button;

    private Animator _anim;
    private bool _trashCleaned;
    private bool _cableConnected;

    // Use this for initialization
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        Debug.Log("I still need to have an sound that shows that I work, give me my voice!!");

        if (_trashCleaned && _cableConnected)
        {
            _anim.SetBool("enabled", true);
            RuntimeManager.PlayOneShot(_activateSound, _button.transform.position);
            RuntimeManager.PlayOneShot(_turbineSound, transform.position);
        }
        /* else
         * display "Turbine is not fixed" Image on Console
        * 
        * 
        */
        else
        {
            Debug.Log("Turbine is not fixed, display that on the console");
            RuntimeManager.PlayOneShot(_errorSound, _button.transform.position);
        }


    }


    public void CableConnected()
    {
        _cableConnected = true;
    }

    public void TrashCleaned()
    {
        _trashCleaned = true;
    }
}
