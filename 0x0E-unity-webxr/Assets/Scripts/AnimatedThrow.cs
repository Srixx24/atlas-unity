using System.Collections;
using UnityEngine; 

public class AnimatedThrow : MonoBehaviour
{    
    public Animator animator;
    public GameObject MainCamera;
    public GameObject ThrowCam;


    void Start()
    {
        animator = GetComponent<Animator>();
        ThrowCam.SetActive(false);
    }

    public void StartThrow()
    {
        // Activate ThrowCam and start the animation
        if (MainCamera != null)
            MainCamera.SetActive(false);

        if (ThrowCam != null)
            ThrowCam.SetActive(true);

        StartCoroutine(WaitForAnimation());     
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(4.5f);
        ReturnToPlay();
    }

    public void ReturnToPlay()
    {
        ThrowCam.SetActive(false);
        MainCamera.SetActive(true);
    }
}