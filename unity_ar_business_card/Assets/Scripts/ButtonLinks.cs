using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLinks : MonoBehaviour
{
    public void Github()
    {
        Application.OpenURL("https://github.com/Srixx24");
    }

    public void Facebook()
    {
        Application.OpenURL("https://www.facebook.com/EpicNinjaMaster/");
    }

    public void Linkedin()
    {
        Application.OpenURL("https://www.linkedin.com/in/jackielovins/");
    }

    public void Gmail()
    {
        Application.OpenURL("mailto:jacolynlovins@gmail.com");
    }
}
