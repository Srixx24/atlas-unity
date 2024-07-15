using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    private Timer timerScript;
    private bool timerStarted = false;
    private bool playerContact = false;

    private void Start()
    {
        timerScript = GetComponent<Timer>();
        timerScript.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartTimer();
            playerContact = false;
        }
    }
    private void StartTimer()
    {
        if (!timerStarted)
        {
            timerScript.enabled = true;
            timerStarted = true;
        }
    }
}
