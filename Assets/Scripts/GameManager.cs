using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private CSVToLevel levelLoader;                         //Store a reference to our BoardManager which will set up the level.
    private int level = 0;                                  //Current level number
    private bool[] swapped = { false, false, false, false, false, false };

    public GameObject knight;
    public GameObject ghost;

    private bool knightOnGoalTile = false;
    private bool ghostOnGoalTile = false;

    private Camera mainCamera;

    private int steps = 0;

    private int[] maxSteps = { 63, 66, 73, 99, 40, 40 };
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
    const float UPPER_LEVEL_SPAWN_HEIGHT = 7.75f;

    //Initializes the game for each level.
    void InitGame()
    {
        //Spawn the goal and spawn tiles.
        levelLoader.goalAndSpawnTiles();

        levelOperations();
    }

    private void levelOperations()
    {
        level = 1;

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        levelLoader.transitionToLevel(level);
        steps = 0;

        //Spawn players
        spawnPlayer(knight, -2, LOWER_LEVEL_SPAWN_HEIGHT, 5);
        spawnPlayer(ghost, -2, UPPER_LEVEL_SPAWN_HEIGHT, 5);
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
        Debug.Log("Knight on goal tile");
        knightOnGoalTile = b;
        goalsReached();
    }

    public void setGhostOnGoalTile(bool b)
    {
        Debug.Log("Ghost on goal tile");
        ghostOnGoalTile = b;
        goalsReached();
    }

    private void goalsReached()
    {
        if(knightOnGoalTile && ghostOnGoalTile)
        {
            Debug.Log("Knight and ghost on tile");
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
        Debug.Log("Swapping tiles");
        Transform[] persistentObjects = GameObject.Find("PersistentTiles").GetComponentsInChildren<Transform>();


        if (!swapped[level - 1])
        {
            foreach (Transform obj in persistentObjects)
            {
                if (obj.gameObject.tag == "goalTile")
                {
                    obj.gameObject.tag = "spawnTile";
                }
                else if (obj.gameObject.tag == "spawnTile")
                {
                    obj.gameObject.tag = "goalTile";
                }

            }
            swapped[level - 1] = true;
        }

        setGhostOnGoalTile(false);
        setKnightOnGoalTile(false);
            
        
    }

    private void disablePlayersMovement()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in players)
        {
            if(obj.name.Contains("Knight"))
            {
                obj.GetComponent<KnightMovTileBased>().disableMovement();
            }
            else if (obj.name.Contains("Ghost"))
            {
                obj.GetComponent<GhostMovTileBased>().disableMovement();
            }
        }

    }

    private void enablePlayersMovement()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in players)
        {
            if (obj.name.Contains("Knight"))
            {
                obj.GetComponent<KnightMovTileBased>().enableMovement();
            }
            else if (obj.name.Contains("Ghost"))
            {
                obj.GetComponent<GhostMovTileBased>().enableMovement();
            }
        }
    }

    private void resetPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in players)
        {
            Destroy(obj);
        }

        spawnPlayer(knight, -2, LOWER_LEVEL_SPAWN_HEIGHT, 5);
        spawnPlayer(ghost, -2, UPPER_LEVEL_SPAWN_HEIGHT, 5);
    }

    private void GameOver()
    {
        resetGoalAndSpawn();
        resetPlayers();
        resetUI();
        for(int x = 0; x < swapped.Length; x++)
        {
            swapped[x] = false;
        }

        levelOperations();
    }

    private void resetGoalAndSpawn()
    {
        if(level % 2 == 0 && swapped[level - 1])
        {
            switchSpawnAndGoal();
        }
    }

    private void resetUI()
    {

    }

    

    //Update is called every frame.
    void Update()
    {

    }

}