using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public Animator animator;
    public GameObject CutsceneCamera;
    public GameObject MainCamera;
    public GameObject TimerCanvas;
    public GameObject Player;
    private PlayerController playerController;

    private int currentLevel = 1; // Default to Level 1

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = Player.GetComponent<PlayerController>();
    }

    public void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName($"Intro0{currentLevel}"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                MainCamera.SetActive(true);
                Player.GetComponent<PlayerController>().enabled = true;
                TimerCanvas.SetActive(true);
                CutsceneCamera.SetActive(false);
            }
        }
    }
}