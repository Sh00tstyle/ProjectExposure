﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaypoint : MonoBehaviour {

    public TutorialButtons tutorialButton;

    [FMODUnity.EventRef]
    public string tutorialVoiceline;

    private bool _active;
    private TutorialArea _tutorialArea;

    public void Awake() {
        _active = false;
    }

    public void Update() {
        if (!_active) return;

        if(CheckForButtonPress()) {
            _tutorialArea.ActivateNextWaypoint(); //this one gets disabled and the next one enabled
        }
    }

    //check if the desired tutorial was completed (most likely needs more interations)
    public bool CheckForButtonPress() {
        switch(tutorialButton) {
            case TutorialButtons.Teleport:
                if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5f) {
                    return true;
                }
                
                break;

            case TutorialButtons.RightGrab:
                if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5f && _tutorialArea.vacuum.vacuumGrabber.IsGrabbing() && _tutorialArea.vacuum.vacuumGrabber.InVacuumMode()) {
                    return true;
                }

                break;

            case TutorialButtons.Vacuum:
                //if the trigger is pressed while the vacuum is in the player's hand
                if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f && _tutorialArea.vacuum.GetVacuumState() == VacuumState.Player) {
                    return true;
                }

                break;

            case TutorialButtons.CallCompanion:
                //call companion while still remaining in the tutorial state?
                if (OVRInput.GetDown(OVRInput.Button.One)) {
                    return true;
                }

                break;

            case TutorialButtons.FullTutorial:
                //might bug out cause the full button tutorial will activate itself
                if (OVRInput.GetDown(OVRInput.Button.Three)) {
                    return true;
                }

                break;

            default:
                if(_tutorialArea.companionAudio.GetStartedPlaying() && _tutorialArea.companionAudio.GetPlaybackState(AudioSourceType.Voice) == FMOD.Studio.PLAYBACK_STATE.STOPPED) {
                    //if the voiceline is over
                    return true;
                }
                break;
        }

        return false;
    }

    public void Deactivate() {
        _active = false;

        _tutorialArea.buttonsTutorial.SetButtonTutorial(tutorialButton, false); //disable tutorial
    }

    public void Activate(TutorialArea tutorialArea) {
        _active = true;

        _tutorialArea = tutorialArea; //set reference when activated
        _tutorialArea.buttonsTutorial.SetButtonTutorial(tutorialButton, true); //enable tutorial

        // set clip and play voiceline
        _tutorialArea.companionAudio.StopAudioSource(AudioSourceType.Voice);
        _tutorialArea.companionAudio.SetClip(tutorialVoiceline, AudioSourceType.Voice);
        StartCoroutine(_tutorialArea.companionAudio.PlayAudioSourceWithHaptic(AudioSourceType.Voice)); //apply vibration
    }
}