using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    
    

        // Declare Vector 2 for X and Y input
        private Vector3 Detect_An_Input;
        public float speed = 5;
        public float jump_strength = 300;

        // Declare variable of type Rigidbody2D
        private Rigidbody my_rigid_body;
        private Transform m_GroundCheck;
       

        // Sets variables for how the groundCheck is handled.
        const float k_GroundedRadius = .2f;
        [SerializeField] private LayerMask m_WhatIsGround;
        private bool m_Grounded;
        private bool m_FacingRight = true;

        // Declares playerNumber 0 to be the first player to be controlled
        public int playerNumber = 0;

        private Animator m_Anim;

        // Use this for initialization
        void Start()
        {
            my_rigid_body = GetComponent<Rigidbody>();
            m_GroundCheck = transform.Find("GroundCheck");
            m_Anim = GetComponent<Animator>();
            //m_player = GetComponent<Player_Switch>();        	
        }

        // Update is called once per frame
        void Update()
        {

            // Every frame unity checks if a button is pressed down and provides horizontal movement.
            Detect_An_Input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Allows Jumping if grounded.
            if (Input.GetButtonDown("Jump") && m_Grounded)
            {
                Jump();
            }

            /*
            if (Detect_An_Input.x > 0 && !m_FacingRight)
            {
                Flip();

            }
            else if (Detect_An_Input.x < 0 && m_FacingRight)
            {
                Flip();
            }

            if (Detect_An_Input.x > 0 || Detect_An_Input.x < 0)
            {
                m_Anim.SetBool("playerRun", true);
            }
            else
                m_Anim.SetBool("playerRun", false);
                */
        }


        void FixedUpdate()
        {

            // Horizontal movement
            m_Grounded = false;
            my_rigid_body.velocity = new Vector3(Detect_An_Input.x * speed, 0, Detect_An_Input.y * speed);

            // Sets overlap circle to detect whether groundCheck is in contact with the ground.
            //Collider[] colliders = Physics.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            /*
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                }

            }
            */
        }

        void Jump()
        {
            // Jump, Requires a ground check.
            my_rigid_body.AddForce(new Vector2(0, jump_strength));
        }

        private void Flip()
        {
            m_FacingRight = !m_FacingRight;

            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }


