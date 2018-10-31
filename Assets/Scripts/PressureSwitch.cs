using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour {

    Collider m_collider;
    Color originalColor;
    public GameObject[] target;

	// Use this for initialization
	void Start () {
        m_collider = GetComponent<BoxCollider>();
        originalColor = gameObject.GetComponent<MeshRenderer>().material.color;
        target = GameObject.FindGameObjectsWithTag("Door1");
	}
	
	// Update is called once per frame
	void Update () {

        
		
	}

    int OnTriggerStay(Collider other)
    {

        Debug.Log("OnSwitch");
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        for (int i = 0; i < target.Length; i++) {
            Destroy(target[i]);
        }
        return 1;
    }

    int OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = originalColor;
        Debug.Log("left Switch");
        return 0;
    }

}
