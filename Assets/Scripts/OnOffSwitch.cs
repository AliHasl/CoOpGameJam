using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSwitch : MonoBehaviour {

    private bool turnedOn;

    public GameObject effector;

	// Use this for initialization
	void Start () {
        turnedOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void flipSwitch()
    {
         turnedOn = !turnedOn;
        if (turnedOn)
        {
            effector.SendMessage("Effect");
        }
    }
}
