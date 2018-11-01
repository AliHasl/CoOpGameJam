using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCountScript : MonoBehaviour {

    private Text stepCountNo;
    GameManager m_gameManager;

    private int stepsTaken;
    
    private int totalStepsAllowed;

	// Use this for initialization
	void Start () {

        m_gameManager = GameObject.FindObjectOfType<GameManager>();
        

       // totalStepsAllowed = m_gameManager.getStepsForLevel();
        
        stepCountNo = GetComponent<Text>();

        stepsTaken = 0;
        
    }

    public void StartNewLevel(int level)
    {
        stepsTaken = 0;

        //ABSTRACT METHOD
        //totalStepsAllowed = m_gameManager.GetComponents<GameManager>().getStepsAllowed(level);
    }
	
	
	public void Update () {

        //ABSTRACT CLASS
        //stepsTaken = m_gameManager.GetComponent<GameManager>().StepsTaken();

        stepCountNo.text = stepsTaken.ToString() + " / " + totalStepsAllowed.ToString();
    }
}
