using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private CSVToLevel levelLoader;                         //Store a reference to our BoardManager which will set up the level.
    private int level = 0;                                  //Current level number

    public GameObject knight;
    public GameObject ghost;

    private bool knightOnGoalTile = false;
    private bool ghostOnGoalTile = false;

    private Camera mainCamera;

    private int steps = 0;

    private int[] maxSteps = { 30, 30, 30, 30 };
    public static int NUM_LEVELS = 6;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        levelLoader = GetComponent<CSVToLevel>();

        //Call the InitGame function to initialize the first level 
        InitGame();

        
    }

    const float LOWER_LEVEL_SPAWN_HEIGHT = 0.75f;
    const float UPPER_LEVEL_SPAWN_HEIGHT = 5.75f;

    //Initializes the game for each level.
    void InitGame()
    {
        StartCoroutine( levelOperations());
    }

    IEnumerator levelOperations()
    {
        //Spawn the goal and spawn tiles.
        levelLoader.goalAndSpawnTiles();

        level = 1;

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        levelLoader.transitionToLevel(level);
        steps = 0;

        



        //Spawn players
        spawnPlayer(knight, -2, LOWER_LEVEL_SPAWN_HEIGHT, 5);
        spawnPlayer(ghost, -2, UPPER_LEVEL_SPAWN_HEIGHT, 5);

        yield return new WaitForSeconds(10);

        level++;

        levelLoader.transitionToLevel(level);
        steps = 0;

        //levelLoader.LoadLevel(2);

    }


    //Spawns players to map
    void spawnPlayer(GameObject playerId, float x, float y, float z)
    {
        Instantiate(playerId, new Vector3(x, y, z), transform.rotation);
        
    }

    public void incrementSteps()
    {
        steps++;
        if (overStepCount(level))
        {
            GameOver();
        }
    }

    private bool overStepCount(int level)
    {
        return steps > maxSteps[level - 1];
    }

    public int getSteps()
    {
        return steps;
    }

    public int getStepsAllowed(int level)
    {
        return maxSteps[level - 1];
    }

    public void fallingMsgGameOver()
    {

    }

    public void setKnightOnGoalTile(bool b)
    {
        knightOnGoalTile = b;
        goalsReached();
    }

    public void setGhostOnGoalTile(bool b)
    {
        ghostOnGoalTile = b;
        goalsReached();
    }

    private void goalsReached()
    {
        if(knightOnGoalTile && ghostOnGoalTile)
        {
            if(level != 6)
            {
                level++;
                switchSpawnAndGoal();
                disablePlayersMovement();
                levelLoader.transitionToLevel(level);
                StartCoroutine(waitBeforeEnableMovement());
            }
            else if (level == 7)
            {
                GameOver();
            }
        }
    }

    IEnumerator waitBeforeEnableMovement()
    {
        yield return new WaitForSecondsRealtime(10);
        enablePlayersMovement();
    }

    private void switchSpawnAndGoal()
    {

    }

    private void disablePlayersMovement()
    {

    }

    private void enablePlayersMovement()
    {

    }

    private void resetPlayers()
    {

    }

    private void GameOver()
    {

    }

    private void resetGoalAndSpawn()
    {

    }

    private void resetUI()
    {

    }

    

    //Update is called every frame.
    void Update()
    {

    }

}