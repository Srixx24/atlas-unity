using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    private Animator animator;
    public GameObject CutsceneCamera;
    public GameObject MainCamera;
    public GameObject TimerCanvas;
    public GameObject Player;
    private PlayerController playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = Player.GetComponent<PlayerController>();
    }

    public void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Intro01"))
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