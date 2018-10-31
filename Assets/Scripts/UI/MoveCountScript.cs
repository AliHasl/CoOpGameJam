using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCountScript : MonoBehaviour {

    private Text stepCountNo;
    GameManager m_gameManager;
    GameObject knight;
    GameObject ghost;
    private int knightSteps;
    private int ghostSteps;
    private int totalStepsAllowed;

	// Use this for initialization
	void Start () {

        m_gameManager = GameObject.FindObjectOfType<GameManager>();
        
        knight = GameObject.Find("Knight(Clone)");
        ghost = GameObject.Find("Ghost(Clone)");

        totalStepsAllowed = m_gameManager.getStepsForLevel();
        knightSteps = 0;
        ghostSteps = 0;
        stepCountNo = GetComponent<Text>();
        
    }
	
	
	public void Update () {

        knightSteps = knight.GetComponent<KnightMovTileBased>().getSteps();
        ghostSteps = ghost.GetComponent<GhostMovTileBased>().getSteps();

        stepCountNo.text = (knightSteps + ghostSteps).ToString() + " / " + totalStepsAllowed.ToString();
    }
}
