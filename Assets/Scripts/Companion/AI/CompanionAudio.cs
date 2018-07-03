﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class CompanionAudio : MonoBehaviour {

    public Transform watchAnchor;
    public float talkingRadius;

    public AudioClip callFeedbackAudio;

    [EventRef]
    public string callFeedbackEvent;

    [EventRef]
    public string motorSound;

    private EventInstance[] _audioTracks;
    private EventInstance _callFeedbackInstance;
    private OVRHapticsClip _notificationHapticsClip;
    private OVRHapticsClip _callHapticsClip;

    private bool _startedPlaying;

    //notification samples
    private byte[] notificationSamples = {
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            1, 1, 1, 1, 1,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255,
            255, 255, 255, 255, 255
    };

    public void Awake() {
        _audioTracks = new EventInstance[2];

        _notificationHapticsClip = new OVRHapticsClip(notificationSamples, notificationSamples.Length);

        if (callFeedbackAudio != null) _callHapticsClip = new OVRHapticsClip(callFeedbackAudio);
        if (callFeedbackEvent != "") _callFeedbackInstance = RuntimeManager.CreateInstance(callFeedbackEvent);

        _startedPlaying = false;

        SetClip(motorSound, AudioSourceType.Effects);
        PlayAudioSource(AudioSourceType.Effects);
    }

    public void Update() {
        AdjustVoiceLineSource();

        _audioTracks[(int)AudioSourceType.Effects].set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
    }

    private void AdjustVoiceLineSource() {
        if (GetPlaybackState(AudioSourceType.Voice) != PLAYBACK_STATE.PLAYING) return;

        Vector3 deltaVec = transform.position - watchAnchor.position;

        if(deltaVec.magnitude > talkingRadius) {
            //out of range
            _audioTracks[(int)AudioSourceType.Voice].set3DAttributes(RuntimeUtils.To3DAttributes(watchAnchor.position));
        } else {
            //in range
            _audioTracks[(int)AudioSourceType.Voice].set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        }
    }

    public void PlayCallFeedback() {
        //play sound and vibration at the same time in a seperate channel
        if (callFeedbackEvent != "") _callFeedbackInstance.start();
        if (callFeedbackAudio != null) OVRHaptics.RightChannel.Preempt(_callHapticsClip);
    }

    //play notification vibration on left controller
    public void PlayNotificationHaptic() {
        OVRHaptics.LeftChannel.Preempt(_notificationHapticsClip); 
    }

    //returns true if the event clip was valid, returns false otherwise
    public bool SetClip(string eventPath, AudioSourceType sourceType) {
        if (eventPath == "") return false;

        _audioTracks[(int)sourceType] = RuntimeManager.CreateInstance(eventPath);
        _audioTracks[(int)sourceType].set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

        return true;
    }

    public IEnumerator PlayAudioSourceWithHaptic(AudioSourceType sourceType) {
        if (sourceType == AudioSourceType.Voice) PlayNotificationHaptic(); //will need some time between that

        yield return new WaitForSeconds(0.5f); //delay before playing the sound

        PlayAudioSource(sourceType);
    }

    public void PlayAudioSource(AudioSourceType sourceType) {
        _startedPlaying = true;
        _audioTracks[(int)sourceType].start();
    }

    public void StopAudioSource(AudioSourceType sourceType) {
        _startedPlaying = false;
        _audioTracks[(int)sourceType].stop(STOP_MODE.IMMEDIATE);
    }

    public PLAYBACK_STATE GetPlaybackState(AudioSourceType sourceType) {
        PLAYBACK_STATE result;
        _audioTracks[(int)sourceType].getPlaybackState(out result);

        return result;
    }

    public bool GetStartedPlaying() {
        return _startedPlaying;
    }

    public void ResetStartedPlaying() {
        _startedPlaying = false;
    }

}
