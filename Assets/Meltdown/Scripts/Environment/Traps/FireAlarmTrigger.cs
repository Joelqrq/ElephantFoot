#pragma warning disable CS0649
using System.Collections;
using UnityEngine;

public class FireAlarmTrigger : Trigger
{
    [SerializeField] private float timeToTrigger;
    [SerializeField] private float timeTillDisable;
    [SerializeField] private AudioClip sensorAudio;
    [SerializeField] private AudioClip alarmAudio;
    private AudioSource audioSource;
    private float tempTriggerTime;

    protected void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sensorAudio;
       
    }

    protected override void HasEnter() {
        tempTriggerTime = timeToTrigger;
    }

    protected override void HasStay() {

        if (tempTriggerTime > 0f) {
            tempTriggerTime -= Time.deltaTime;
            if(!audioSource.isPlaying) {
                audioSource.Play();
            }
            if (tempTriggerTime < 0f) {
                audioSource.clip = alarmAudio;
                audioSource.Play();
                StartCoroutine(DisableTrigger());
                InvokeTriggerEvent();
            }
        }
    }

    protected override void HasExit() {
        if(audioSource.clip == sensorAudio)
            audioSource.Stop();
    }

    private IEnumerator DisableTrigger() {
        yield return new WaitForSeconds(timeTillDisable);
        InvokeTriggerEvent();
        audioSource.Stop();
        tempTriggerTime = timeToTrigger;
    }
}
