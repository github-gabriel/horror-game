using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Flashlight : NetworkBehaviour
{
    public GameObject flashlight;

    public AudioSource turnOn;
    public AudioSource turnOff;

    private bool active;
    private bool activeSync;

    void Start()
    {
        active = false;
        flashlight.SetActive(active);
    }
    void Update()
    {
        if(!active && Input.GetButtonDown("F"))
        {
            active = !active;
        }
        else if (active && Input.GetButtonDown("F"))
        {
            active = !active;
        }

        activeSync = active;
        
        flashlight.SetActive(active);
    }
}
