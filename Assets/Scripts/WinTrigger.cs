using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    public Timer timerScript;
    public Text timerText;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopTimer();
            TextUpdate();
        }
    }

    private void StopTimer()
    {
        timerScript.enabled = false;
    }

    private void TextUpdate()
    {
        timerText.fontSize = 100;
        timerText.color = Color.green;
    }
}
