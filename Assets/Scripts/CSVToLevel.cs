﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;


public class CSVToLevel : MonoBehaviour
{ 

    // Use this for initialization
    void Start()
    {

        //LoadLevel();
        //Debug.Log(items);
        //InterpLevel(items);
    }

    static int LOWER_GRID_OFFSET = 0;
    static int UPPER_GRID_OFFSET = 7;

    private string LEVEL_DIRECTORY { get { return Application.dataPath + "/level/level"; } }

    private string[] items;

    public GameObject Tile;         // "1"

    [System.Serializable]
    public class WorldObject
    {
        public GameObject obj;
        public float extraY;

        public WorldObject()
        {
            
        }
    }

    public WorldObject[] objects = new WorldObject[14];
    public WorldObject[] wallObjects = new WorldObject[2];


    private const char bomb = 'b';
    private const char boulder = 'o';
    private const char breakable = 'B';
    private const char button = 'v';
    private const char lift = '^';
    private const char liftTop = '_';
    private const char pitfall = 'u';
    private const char pushable = '>';
    private const char switchItem = '/';
    private const char water = '-';
    private const char cornerTile = 'c';
    private const char centerTile = '1';
    private const char leftSideTile = 'l';
    private const char rightSideTile = 'r';

    private const char goalTile = 'g';

    private const char north = 'N';
    private const char east = 'E';
    private const char south = 'S';
    private const char west = 'W';

    private const char door = 'd';
    private const char wall = 'w';

    private string tileRequiringItems = "1boBv^>/clr";
    private string itemsString = "boBv^_u>/-";
    private string wallString = "NESWwd";
    private string tileTypes = "lrc1";

    //EmptyObjects to hold grids
    private GameObject upperGrid;
    private GameObject lowerGrid;
    private GameObject folder;


    public void LoadLevel(int level)
    {
        //Setting up files structure
        upperGrid = new GameObject("UpperGrid" + level);
        lowerGrid = new GameObject("LowerGrid" + level);
        folder = new GameObject("tiles");
        folder.transform.SetParent(upperGrid.transform);
        folder = new GameObject("tiles");
        folder.transform.SetParent(lowerGrid.transform);
        folder = new GameObject("walls");
        folder.transform.SetParent(upperGrid.transform);
        folder = new GameObject("walls");
        folder.transform.SetParent(lowerGrid.transform);
        folder = new GameObject("things");
        folder.transform.SetParent(upperGrid.transform);
        folder = new GameObject("things");
        folder.transform.SetParent(lowerGrid.transform);


        string levelText = System.IO.File.ReadAllText(LEVEL_DIRECTORY + level.ToString() + ".csv");
        char[] commaSeparator = new char[] { ',', '\n', '\r' };
        items = levelText.Split(commaSeparator, System.StringSplitOptions.RemoveEmptyEntries);

        InterpLevel(items);
    }
    
    public void transitionToLevel(int level)
    {
        GameObject upperGridNew1 = null;
        GameObject lowerGridNew1 = null;

        GameObject upperGridOld1 = null;
        GameObject lowerGridOld1 = null;

        GameObject upperGridNew2 = null;
        GameObject lowerGridNew2 = null;

        GameObject upperGridOld2 = null;
        GameObject lowerGridOld2 = null;

        LoadLevel(level);

        if(level > 1)
        {
            upperGridOld1 = GameObject.Find("UpperGrid" + (level - 1));
            upperGridNew1 = new GameObject();
            upperGridNew1.transform.position = new Vector3(upperGridOld1.transform.position.x, upperGridOld1.transform.position.y + 22, upperGridOld1.transform.position.z);

            lowerGridOld1 = GameObject.Find("LowerGrid" + (level - 1));
            lowerGridNew1 = new GameObject();
            lowerGridNew1.transform.position = new Vector3(lowerGridOld1.transform.position.x, lowerGridOld1.transform.position.y + 22, lowerGridOld1.transform.position.z);

            upperGridOld1.AddComponent<MoveBetweenTwoPoints>();
            upperGridOld1.GetComponent<MoveBetweenTwoPoints>().setValues(0.5f, upperGridOld1, upperGridNew1);
            lowerGridOld1.AddComponent<MoveBetweenTwoPoints>();
            lowerGridOld1.GetComponent<MoveBetweenTwoPoints>().setValues(0.5f, lowerGridOld1, lowerGridNew1);
        }

        upperGridOld2 = GameObject.Find("UpperGrid" + level);
        upperGridNew2 = new GameObject();
        upperGridNew2.transform.position = new Vector3(upperGridOld2.transform.position.x, upperGridOld2.transform.position.y + 15, upperGridOld2.transform.position.z);

        lowerGridOld2 = GameObject.Find("LowerGrid" + level);
        lowerGridNew2 = new GameObject();
        lowerGridNew2.transform.position = new Vector3(lowerGridOld2.transform.position.x, lowerGridOld2.transform.position.y + 15, lowerGridOld2.transform.position.z);

        upperGridOld2.AddComponent<MoveBetweenTwoPoints>();
        upperGridOld2.GetComponent<MoveBetweenTwoPoints>().setValues(0.5f, upperGridOld2, upperGridNew2);
        lowerGridOld2.AddComponent<MoveBetweenTwoPoints>();
        lowerGridOld2.GetComponent<MoveBetweenTwoPoints>().setValues(0.5f, lowerGridOld2, lowerGridNew2);

       

        if (level > 1)
        {
            StartCoroutine( waitBeforeDestroyLevel(level - 1));
        }
    }

    IEnumerator waitBeforeDestroyLevel(int level)
    {
        yield return new WaitForSecondsRealtime(5);
        Debug.Log("Destroy" + level);
        destroyLevel(level);
    }

    public void destroyLevel(int a)
    {
        Object.Destroy(GameObject.Find("UpperGrid" + a));
        Object.Destroy(GameObject.Find("LowerGrid" + a));
    }

    public void InterpLevel(string[] levelText)
    {
        
        /*for(int i = 0; i < levelText.Length; i++)
        {
            Debug.Log(levelText[i]);
        }*/
        
        GameObject tempObj = null;

        int y = UPPER_GRID_OFFSET - 15;
        int z = 9;
        int x = 0;

        foreach (string a in levelText)
        {
            //Debug.Log("Checking " + a + " at " + x + ", " + y + ", " + z);

            

            //Spawn tiles.
            if (tileRequiringItems.Contains(a.Substring(0, 1)))
            {
                char[] tileType = { getTileType(x, z, a) };
                tempObj = Object.Instantiate(getType(tileType), new Vector3(x, y, z), getTileRotation(tileType));
                setGridParent(tempObj);
                tempObj.name = "" + getTypeString(tileType) + "(" + x + "," + z + "," + y + ")";
            }

            //Spawn objects.
            if (itemsString.Contains(a.Substring(0, 1)))
            {
                tempObj = Object.Instantiate(getType(a.ToCharArray()), new Vector3(x, y + getExtraY(a.ToCharArray()), z), new Quaternion());
                setGridParent(tempObj);
                //tempObj.transform.position = new Vector3(x, tempObj.transform.position.y + getExtraY(a.ToCharArray()), z);
                tempObj.name = "" + getTypeString(a.ToCharArray()) + "(" + x + "," + z + "," + y + ")";

                //Debug.Log("Instantiating object at " + x + ", " + y + ", " + z);
            }

            //Spawn walls.
            if (a.Length > 2)
            {
                foreach (char c in a.Substring(1).ToCharArray())
                {
                    if (wallString.Contains(c.ToString()))
                    {
                        for (int iter = 1; iter < a.Length; iter += 2)
                        {
                            float rotation = getWallRotation(a.Substring(iter, 1).ToCharArray());
                            Debug.Log(rotation);

                            Quaternion finalRotation;
                            if (a.Substring(iter + 1, 1) == door.ToString())
                            {
                                finalRotation = Quaternion.Euler(90, rotation, 0);
                            }
                            else
                            {
                                finalRotation = Quaternion.Euler(0, rotation, 0);
                            }


                            tempObj = Object.Instantiate(getType(a.Substring(iter + 1, 1).ToCharArray()),
                                new Vector3(x + getWallXMovement(a.Substring(iter, 1).ToCharArray()), y + getExtraY(a.Substring(iter + 1, 1).ToCharArray()), z + getWallZMovement(a.Substring(iter, 1).ToCharArray())),
                                finalRotation);
                            setGridParent(tempObj);
                            tempObj.name = "Wall(" + x + "," + z + "," + y + ")";

                            Debug.Log("Instantiating wall at " + x + ", " + y + ", " + z);


                        }

                        break;
                    }
                }
            }


            //Grid movement.
            x++;
            if (x == 10)
            {
                x = 0;
                z--;
            }
            if (z == -1)
            {
                y = LOWER_GRID_OFFSET - 15;
                x = 0;
                z = 9;
            }

        }
    }

    private GameObject getType(char[] a)
    {
        switch (a[0])
        {
            case bomb:
                return objects[0].obj;
            case boulder:
                return objects[1].obj;
            case breakable:
                return objects[2].obj;
            case button:
                return objects[3].obj;
            case lift:
                return objects[4].obj;
            case pushable:
                return objects[5].obj;
            case switchItem:
                return objects[6].obj;
            case pitfall:
                return objects[7].obj;
            case liftTop:
                return objects[8].obj;
            case water:
                return objects[9].obj;
            case centerTile:
                return objects[10].obj;
            case cornerTile:
                return objects[11].obj;
            case leftSideTile:
                return objects[12].obj;
            case rightSideTile:
                return objects[13].obj;
            case door:
                return wallObjects[0].obj;
            case wall:
                return wallObjects[1].obj;
        }

        return null;
    }


    //Repetition functions
    private string getTypeString(char[] a)
    {
        switch (a[0])
        {
            case bomb:
                return "Bomb";
            case boulder:
                return "Boulder";
            case breakable:
                return "Breakable";
            case button:
                return "Button";
            case lift:
                return "Lift";
            case pushable:
                return "Pushable";
            case switchItem:
                return "SwitchItem";
            case pitfall:
                return "Pitfall";
            case liftTop:
                return "LiftTop";
            case water:
                return "Water";
            case centerTile:
                return "CenterTile";
            case cornerTile:
                return "CornerTile";
            case leftSideTile:
                return "LeftTile";
            case rightSideTile:
                return "RightTile";
            case door:
                return "Door";
            case wall:
                return "Wall";
        }

        return null;
    }

    private float getExtraY(char[] a)
    {
        switch (a[0])
        {
            case bomb:
                return objects[0].extraY;
            case boulder:
                return objects[1].extraY;
            case breakable:
                return objects[2].extraY;
            case button:
                return objects[3].extraY;
            case lift:
                return objects[4].extraY;
            case pushable:
                return objects[5].extraY;
            case switchItem:
                return objects[6].extraY;
            case pitfall:
                return objects[7].extraY;
            case liftTop:
                return objects[8].extraY;
            case water:
                return objects[9].extraY;
            case centerTile:
                return objects[10].extraY;
            case cornerTile:
                return objects[11].extraY;
            case leftSideTile:
                return objects[12].extraY;
            case rightSideTile:
                return objects[13].extraY;
            case door:
                return wallObjects[0].extraY;
            case wall:
                return wallObjects[1].extraY;
        }

        return 0.0f;
    }

    private float getWallRotation(char[] a)
    {
        switch (a[0])
        {
            case north:
                return 0.0f;
            case east:
                return 90.0f;
            case south:
                return 180.0f;
            case west:
                return 270.0f;
        }

        return 0;
    }

    private float getWallXMovement(char[] a)
    {
        switch (a[0])
        {
            case north:
                return 0.0f;
            case east:
                return 0.45f;
            case south:
                return 0.0f;
            case west:
                return -0.45f;

        }

        return 0.0f;
    }

    private void setGridParent(GameObject thing)
    {
        if (thing.tag == "tile")
        {
            if (thing.transform.position.y >= UPPER_GRID_OFFSET - 1)
            {
                thing.transform.SetParent(upperGrid.transform.GetChild(0).transform);
            }
            else
            {
                thing.transform.SetParent(lowerGrid.transform.GetChild(0).transform);
            }
        }
        if (thing.tag == "wall")
        {
            if (thing.transform.position.y >= UPPER_GRID_OFFSET - 1)
            {
                thing.transform.SetParent(upperGrid.transform.GetChild(1).transform);
            }
            else
            {
                thing.transform.SetParent(lowerGrid.transform.GetChild(1).transform);
            }
        }
        if (thing.tag == "gameObject")
        {
            if (thing.transform.position.y >= UPPER_GRID_OFFSET - 1)
            {
                thing.transform.SetParent(upperGrid.transform.GetChild(2).transform);
            }
            else
            {
                thing.transform.SetParent(lowerGrid.transform.GetChild(2).transform);
            }
        }
    }

    private float getWallZMovement(char[] a)
    {
        switch (a[0])
        {
            case north:
                return 0.45f;
            case east:
                return 0.0f;
            case south:
                return -0.45f;
            case west:
                return 0.0f;
        }

        return 0.0f;
    }

    private char getTileType(int x, int z, string a)
    {
        if ((x == 0 && z == 0) || a.EndsWith("c"))
        {
            return cornerTile;
        }
        else if (x == 0 || (a.EndsWith("l")))
        {
            return leftSideTile;
        }
        else if (z == 0 || (a.EndsWith("r")))
        {
            return rightSideTile;
        }
        else
        {
            return centerTile;
        }
    }

    private Quaternion getTileRotation(char[] tileType)
    {
        switch (tileType[0])
        {
            case cornerTile:
                return Quaternion.Euler(-90, 0, 180);
            case leftSideTile:
                return Quaternion.Euler(-90, 0, 180);
            case rightSideTile:
                return Quaternion.Euler(-90, 0, 180);
            case centerTile:
                return Quaternion.Euler(-90, 0, 0);
        }

        return new Quaternion(); ;
    }

    public void goalAndSpawnTiles()
    {
        char[] leftSide = { leftSideTile };
        char[] corner = { cornerTile };
        char[] rightSide = { rightSideTile };
        char[] center = { centerTile };

        GameObject tempObj = null;

        GameObject parent = Object.Instantiate(new GameObject());
        parent.name = "PersistentTiles";


        tempObj = Object.Instantiate(getType(leftSide), new Vector3(-2, LOWER_GRID_OFFSET, 5), getTileRotation(leftSide));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform; 
        tempObj = Object.Instantiate(getType(corner), new Vector3(-2, LOWER_GRID_OFFSET, 4), getTileRotation(corner));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(center), new Vector3(-1, LOWER_GRID_OFFSET, 5), getTileRotation(center));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(rightSide), new Vector3(-1, LOWER_GRID_OFFSET, 4), getTileRotation(rightSide));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(center), new Vector3(10, LOWER_GRID_OFFSET, 5), getTileRotation(center));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(center), new Vector3(11, LOWER_GRID_OFFSET, 5), getTileRotation(center));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(rightSide), new Vector3(10, LOWER_GRID_OFFSET, 4), getTileRotation(rightSide));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(rightSide), new Vector3(11, LOWER_GRID_OFFSET, 4), getTileRotation(rightSide));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;

        tempObj = Object.Instantiate(getType(leftSide), new Vector3(-2, UPPER_GRID_OFFSET, 5), getTileRotation(leftSide));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(corner), new Vector3(-2, UPPER_GRID_OFFSET, 4), getTileRotation(corner));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(center), new Vector3(-1, UPPER_GRID_OFFSET, 5), getTileRotation(center));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(rightSide), new Vector3(-1, UPPER_GRID_OFFSET, 4), getTileRotation(rightSide));
        tempObj.tag = "spawnTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(center), new Vector3(10, UPPER_GRID_OFFSET, 5), getTileRotation(center));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(center), new Vector3(11, UPPER_GRID_OFFSET, 5), getTileRotation(center));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(rightSide), new Vector3(10, UPPER_GRID_OFFSET, 4), getTileRotation(rightSide));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;
        tempObj = Object.Instantiate(getType(rightSide), new Vector3(11, UPPER_GRID_OFFSET, 4), getTileRotation(rightSide));
        tempObj.tag = "goalTile";
        tempObj.transform.parent = parent.transform;
    }
}