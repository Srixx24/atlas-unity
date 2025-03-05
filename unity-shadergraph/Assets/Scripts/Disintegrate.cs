using UnityEngine;

public class Disintegrate : MonoBehaviour
{
    private Material material;
    private bool hideObject;
    public float dissolveSpeed = 2f;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hideObject = !hideObject;
        }
        if(hideObject)
        {
            material.SetFloat("_Dissolve_Amount", Mathf.MoveTowards(material.GetFloat("_Dissolve_Amount"), 1.2f, dissolveSpeed * Time.deltaTime));
        }
        else
        {
            material.SetFloat("_Dissolve_Amount", Mathf.MoveTowards(material.GetFloat("_Dissolve_Amount"), -0.2f, dissolveSpeed * Time.deltaTime));
        }
    }
}
