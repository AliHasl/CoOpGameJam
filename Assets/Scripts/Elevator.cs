using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    Vector3 startPosition;
    Vector3 endPosition;
    bool goingUp;
    Rigidbody m_rigidBody;

	// Use this for initialization
	void Start () {
        startPosition = gameObject.transform.position;
        endPosition = gameObject.transform.position + Vector3.up;
        goingUp = true;
        m_rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if(goingUp && Vector3.Distance(gameObject.transform.position, endPosition) < 0.01F)
        {
            goingUp = false;
        }

        if (!goingUp && Vector3.Distance(gameObject.transform.position, startPosition) < 0.01F)
        {
            goingUp = true;
        }

        if (goingUp)
        {
            if (Vector3.Distance(gameObject.transform.position, endPosition) > 0.0f)
            {
                //m_rigidBody.AddForce(Vector3.up, ForceMode.Force);
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endPosition, 0.02f);
            }
           
        }
        if(!goingUp)
        {
            if (Vector3.Distance(gameObject.transform.position, startPosition) > 0.0f)
            {
                //m_rigidBody.AddForce(Vector3.down, ForceMode.Force);
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, startPosition, 0.02f);
                
            }
           
            /*
            if (gameObject.transform.position == startPosition)
            {
                goingUp = true;
            }
            */
        }
		
	}
}
