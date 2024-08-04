using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public Animator animator;
    public GameObject CutsceneCamera;
    public GameObject MainCamera;
    public GameObject TimerText;
    public GameObject Player;
    private PlayerController playerController;

    private int currentLevel = 1;


    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = Player.GetComponent<PlayerController>();
        CutsceneCamera.SetActive(true);
        TimerText.SetActive(false);
    }

    public void Update()
    {
        EndCutScene();
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void EndCutScene()
    {
        if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName($"Intro0{currentLevel}"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                if (MainCamera != null)
                    MainCamera.SetActive(true);

                if (Player != null && Player.GetComponent<PlayerController>() != null)
                    Player.GetComponent<PlayerController>().enabled = true;

                if (TimerText != null)
                    TimerText.SetActive(true);

                if (CutsceneCamera != null)
                    CutsceneCamera.SetActive(false);
            }
        }
    }
}