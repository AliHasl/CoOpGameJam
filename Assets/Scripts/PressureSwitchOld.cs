using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitchOld : MonoBehaviour {

    Collider m_collider;
    Color originalColor;

	// Use this for initialization
	void Start () {
        m_collider = GetComponent<BoxCollider>();
        originalColor = gameObject.GetComponent<MeshRenderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}

    int OnTriggerStay(Collider other)
    {

        Debug.Log("OnSwitch");
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        return 1;
    }

    int OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = originalColor;
        Debug.Log("left Switch");
        return 0;
    }

}
