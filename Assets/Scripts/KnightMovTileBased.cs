using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovTileBased : MonoBehaviour {

    private Rigidbody my_rigid_body;
    private Animator m_Anim;
    private Vector3 Detect_An_Input;
    private Vector3 currentPos;
    private Vector3 pos;
    private float speed;

    //GroundCheck for falling
    private Transform m_GroundCheck;
    private float k_GroundedRadius;
    private bool m_grounded;

    //Check for the next move
    private int playerPositionX;
    private int playerPositionY;

    //Counter for the number of steps taken
    private int stepCount;

    Quaternion m_quaternion;

    // Use this for initialization
    void Start() {
        my_rigid_body = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animator>();
        m_GroundCheck = transform.Find("GroundCheck");
        pos = gameObject.transform.position;
        k_GroundedRadius = 0.2f;

        //Keeping track of player position
        playerPositionX = 0;
        playerPositionY = 0;

        //Initialize Stepcount to 0
        stepCount = 0;

    }

    // Update is called once per frame
    void Update() {
        

        
        Vector3 currentPos = gameObject.transform.position;
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (blockDetection(Vector3.forward)){
                transform.forward = Vector3.forward;
                pos += Vector3.forward;
                stepCount++;
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (blockDetection(Vector3.back))
            {
                transform.forward = Vector3.back;
                pos += Vector3.back;
                stepCount++;
            }
            //gameObject.transform.position = pos;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (blockDetection(Vector3.left))
            {
                transform.forward = Vector3.left;
                pos += Vector3.left;
                stepCount++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (blockDetection(Vector3.right))
            {
                transform.forward = Vector3.right;
                pos += Vector3.right;
                stepCount++;
            }
            
        }


        
        //Lerp from one location to the next

        if (Vector3.Distance(gameObject.transform.position, pos) > 0.2f && m_grounded)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, 0.2f);
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
                playerPositionX = (colliders[i].name[5] - 48);
                playerPositionY = (colliders[1].name[7] - 48);
                //Debug.Log("Player X = " + playerPositionX + "Player Y = " + playerPositionY);
                //Debug.Log("Grounded " + colliders[i]);
            }
        }

        if(m_grounded == false)
        {
            //Debug.Log("Not on ground");
        }


    }


    private void rotateToFace(Vector3 face)
    {

        if (face == Vector3.forward)
        {
            while (Vector3.Angle(transform.forward, Vector3.forward) > 0.0f)
            {
                transform.forward = Vector3.Lerp(transform.forward, Vector3.forward, Time.deltaTime * 0.01f);
            }
        }
        if (face == Vector3.back)
        {
            while (Vector3.Angle(transform.forward, Vector3.back) > 0.0f)
            {
                transform.forward = Vector3.Lerp(transform.forward, Vector3.back, Time.deltaTime * 0.01f);
            }
        }
        if(face == Vector3.left)
        {
            while (Vector3.Angle(transform.forward, Vector3.left) > 0.0f)
            {
                transform.forward = Vector3.Lerp(transform.forward, Vector3.left, Time.deltaTime);
            }
        }
        else
        {
            while (Vector3.Angle(transform.forward, Vector3.right) > 0.0f)
            {
                transform.forward = Vector3.Lerp(transform.forward, Vector3.right, Time.deltaTime);
            }
        }
    }

    private bool blockDetection(Vector3 target)
    {
        RaycastHit[] hit = Physics.RaycastAll(gameObject.transform.position, target, 1.0f);//, transform.forward, 1.0f);

        //No obstructions
        if (hit.Length == 0)
        {
            return true;
        }


        for (int i = 0; i < hit.Length; i++)
        {
            //Pushable block
            if(hit[i].collider.tag == "pushableBlock")
            {
                Debug.Log("push it");
                if (gameObject.transform.forward == Vector3.forward)
                {
                    hit[i].transform.SendMessage("MoveForward");
                }
                else if (gameObject.transform.forward == Vector3.back)
                {
                    hit[i].transform.SendMessage("MoveBack");
                }
                else if (gameObject.transform.forward == Vector3.left)
                {
                    hit[i].transform.SendMessage("MoveLeft");
                }
                else if(gameObject.transform.forward == Vector3.right)
                {
                    hit[i].transform.SendMessage("MoveRight");
                }

                return true;
            }

            
        }
        return false;
    }


}



