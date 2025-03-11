using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAlter : MonoBehaviour
{
    public GameObject particles;
    public GameObject Light;
    public GameObject Gate;
    public bool isOn = false;
    public bool HasAnimated = false;


    public GameObject ActivationEffect;
    public GameObject ActivationSound;

    public ColorRestoration2D colourRestoration;

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            if (!HasAnimated)
            {
                Instantiate(ActivationEffect, transform.position, Quaternion.identity);
                HasAnimated = true;
                colourRestoration.phase += 1;
                if (Gate.activeSelf == false) {
                    Gate.SetActive(true);
                } else {
                    Gate.SetActive(false);
                }
                Instantiate(ActivationSound, transform.position, Quaternion.identity);
            }
            particles.SetActive(true);
            Light.SetActive(true);

            
        }
        else
        {
            particles.SetActive(false);
            Light.SetActive(false);
            Gate.SetActive(false);
        }
    }
}
