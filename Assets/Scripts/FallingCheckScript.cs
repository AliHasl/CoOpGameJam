using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCheckScript : MonoBehaviour {

    Rigidbody m_rigidBody;
    GameManager gameManager;
    private bool groundCheck;

	// Use this for initialization
	void Start () {
        m_rigidBody = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        
        if(gameObject.transform.name == "Knight(Clone)")
        {
            groundCheck = GetComponent<KnightMovTileBased>().getGroundCheck();
        }
        else
        {
            groundCheck = GetComponent<GhostMovTileBased>().getGroundCheck();
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (gameObject.transform.name == "Knight(Clone)")
        {
            groundCheck = GetComponent<KnightMovTileBased>().getGroundCheck();
        }
        else
        {
            groundCheck = GetComponent<GhostMovTileBased>().getGroundCheck();
        }

        if(!groundCheck && m_rigidBody.velocity.y < -3.0f)
        {
            
            gameManager.fallingMsgGameOver();
        }
    }
}
