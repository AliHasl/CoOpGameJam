using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{

    bool m_grounded;
    bool m_moveForward;
    Vector3 movePosition;
    private Transform m_GroundCheck;
    private float k_GroundedRadius;

    //bools for movement direction
    bool recentlyMovedForward;
    bool recentlyMovedBack;
    bool recentlyMovedLeft;
    bool recentlyMovedRight;
    // Use this for initialization
    void Start()
    {
        m_grounded = true;
        m_moveForward = false;
        movePosition = gameObject.transform.position;
        m_GroundCheck = transform.Find("GroundCheck");
        k_GroundedRadius = 0.2f;
        recentlyMovedForward = false;
        recentlyMovedBack = false;
        recentlyMovedLeft = false;
        recentlyMovedRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        //movePosition = gameObject.transform.position += Vector3.forward;
        if (Vector3.Distance(gameObject.transform.position, movePosition) > 0.2 && m_grounded)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, movePosition, 0.2f);
        }


        else if (recentlyMovedForward && Vector3.Distance(gameObject.transform.position, movePosition) < 0.2f)
        {
            recentlyMovedForward = false;
            MoveForward();
        }

        else if (recentlyMovedBack && Vector3.Distance(gameObject.transform.position, movePosition) < 0.2f)
        {
            recentlyMovedBack = false;
            MoveBack();
        }

        else if (recentlyMovedLeft && Vector3.Distance(gameObject.transform.position, movePosition) < 0.2f)
        {
            recentlyMovedLeft = false;
            MoveLeft();
        }

       else if (recentlyMovedRight && Vector3.Distance(gameObject.transform.position, movePosition) < 0.2f)
        {
            recentlyMovedRight = false;
            MoveRight();
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
                if (colliders[i].gameObject.tag == "elevator")
                {
                    // new Vector3(0.0f, colliders[i].transform.position.y, 0.0f);
                    gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, colliders[i].transform.position.y, 0.0f);
                }

                //Check for location on grid
                //playerPositionX = (colliders[i].name[5] - 48);
                //playerPositionY = (colliders[1].name[7] - 48);
                //Debug.Log("Player X = " + playerPositionX + "Player Y = " + playerPositionY);
                //Debug.Log("Grounded " + colliders[i]);
            }
        }

        if (m_grounded == false)
        {
            //Debug.Log("Not on ground");
        }


    }


    void MoveForward()
    {
        Debug.Log("Can Move forward" + blockDetection(Vector3.forward));
        if (blockDetection(Vector3.forward))
        {

            movePosition = gameObject.transform.position += Vector3.forward;
            recentlyMovedForward = true;
      

        }
        
    }

    void MoveBack()
    {
        if (blockDetection(Vector3.back))
        {
            movePosition = gameObject.transform.position += Vector3.back;
            recentlyMovedBack = true;
        }
    }

    void MoveLeft()
    {
        if (blockDetection(Vector3.left))
        {
            movePosition = gameObject.transform.position += Vector3.left;
            recentlyMovedLeft = true;
        }
    }

    void MoveRight()
    {
        if (blockDetection(Vector3.right))
        {
            movePosition = gameObject.transform.position += Vector3.right;
            recentlyMovedRight = true;
        }
    }

    private bool blockDetection(Vector3 target)
    {
        RaycastHit[] hit = Physics.RaycastAll(gameObject.transform.position, target, 1.0f);//, transform.forward, 1.0f);

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

}
