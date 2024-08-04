using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinTrigger : MonoBehaviour
{
    public Timer timerScript;
    public Text timerText;
    public GameObject winCanvas;
    public TextMeshProUGUI WinTime;
    public AudioSource victoryPianoAudioSource;
    public AudioSource backgroundMusicAudioSource;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopTimer();
            StopBackgroundMusic();
            ActivateWinCanvas();
        }
    }

    private void StopTimer()
    {
        if (timerScript != null)
        {
            timerScript.Win();
            
        }
    }

    private void ActivateWinCanvas()
    {
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
            if (timerScript != null)
            {
                if (WinTime != null)
                {
                    WinTime.text = timerScript.GetWinTime();
                }
            }
        }
    }

    private void StopBackgroundMusic()
    {
        if (backgroundMusicAudioSource != null)
        {
            backgroundMusicAudioSource.Stop();
        }
    }

    private void PlayVictoryPiano()
    {
        if (victoryPianoAudioSource != null)
        {
            victoryPianoAudioSource.Play();
        }
    }
}
