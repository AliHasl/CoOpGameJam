using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : MonoBehaviour {

    bool m_grounded;
    bool m_moveForward;
    Vector3 movePosition;
    private Transform m_GroundCheck;
    private float k_GroundedRadius;
    private Rigidbody m_rigidBody;

    bool transitionOver;
    

    // Use this for initialization
    void Start () {
        m_grounded = true;
        m_moveForward = false;
        movePosition = gameObject.transform.position;
        m_GroundCheck = transform.Find("GroundCheck");
        k_GroundedRadius = 0.2f;
        m_rigidBody = GetComponent<Rigidbody>();
        transitionOver = false;
    }

    // Update is called once per frame
    void Update()
    {


        //movePosition = gameObject.transform.position += Vector3.forward;



        if (transitionOver) {
        if (Vector3.Distance(gameObject.transform.position, movePosition) > 0.2 && m_grounded)
        {
            if (m_rigidBody.velocity.y == 0)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, movePosition, 0.2f);

            }
        }
    }
    }
        

	

    private void FixedUpdate()
    {
        m_grounded = false;
        Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_grounded = true;
                

                //Check for location on grid
                //int playerPositionX = (colliders[i].name[5] - 48);
                //int playerPositionY = (colliders[1].name[7] - 48);
                //Debug.Log("Player X = " + playerPositionX + "Player Y = " + playerPositionY);
                //Debug.Log("Grounded " + colliders[i]);
            }
        }

        if (m_grounded == false)
        {
            //Debug.Log("Not on ground");
        }

        Debug.Log("Grounded = " + m_grounded);

    }


    void MoveForward()
    {
        Debug.Log("Can Move forward" + blockDetection(Vector3.forward));
        if (blockDetection(Vector3.forward))
        {
            
            movePosition = gameObject.transform.position += Vector3.forward;
        }
        /*
        movePosition = gameObject.transform.position += Vector3.forward;
        if (Vector3.Distance(gameObject.transform.position, movePosition) > 0.0 && m_grounded)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, movePosition, 0.2f);
        }
        */


        //transform.position += Vector3.forward;
    }
    
    void MoveBack()
    {
        if (blockDetection(Vector3.back))
        {
            movePosition = gameObject.transform.position += Vector3.back;
        }
    }

    void MoveLeft()
    {
        if (blockDetection(Vector3.left))
        {
            movePosition = gameObject.transform.position += Vector3.left;
        }
    }

    void MoveRight()
    {
        if (blockDetection(Vector3.right))
        {
            movePosition = gameObject.transform.position += Vector3.right;
        }
    }

    private bool blockDetection(Vector3 target)
    {
        RaycastHit[] hit = Physics.RaycastAll(gameObject.transform.position, target, 1.0f);

        //No obstructions
        if (hit.Length == 0)
        {
            Debug.Log("No Obstructions");
            return true;
        }



        
        return false;
    }


    void HitByRay()
    {
        Debug.Log("I was hit by a ray");
        transform.position += Vector3.forward; 
    }

    public void endTransition() {
    }

}
