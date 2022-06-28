using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landscape : MonoBehaviour
{
    public static List<Vector3> landscapeOrigins = new List<Vector3>();
    public static List<Vector3> invalidLandscapePositions = new List<Vector3>();
    public static List<float> landscapeSizes = new List<float>();

    System.Random rnd = new System.Random();

    CheckPositions checkpos = new CheckPositions();

    bool rockBoundsData = false;
    Object[] rocks;
    int numberofrocktypes = 6;
    List<GameObject> listofrocks = new List<GameObject>();
    public int numberOfRockAttempts = 100;
    GameObject rock = null;

    bool treeBoundsData = false;
    Object[] trees;
    int numberoftreetypes = 4;
    List<GameObject> listoftrees = new List<GameObject>();
    public int numberOfTreeAttempts = 100;
    GameObject tree = null;

    Object[] snowmans;
    int numberofsnowmantypes = 0;
    List<GameObject> listofsnowman = new List<GameObject>();
    public int numberOfSnowmanAttempts = 100;

    bool snowballBoundsData = false;
    Object[] snowballs;
    int numberofsnowballtypes = 0;
    List<GameObject> listofsnowball = new List<GameObject>();
    public int numberOfSnowballAttempts = 100;
    GameObject snowball = null;




    public void Awake()
    {
        rocks = Resources.LoadAll("Landscape/Rocks", typeof(GameObject));
        for (int i = 0; i < numberofrocktypes; i++)
        {
            //cast objects to gameobjects and add to list
            listofrocks.Add((GameObject)rocks[i]);
        }

        trees = Resources.LoadAll("Landscape/Trees", typeof(GameObject));
        for (int i = 0; i < numberoftreetypes; i++)
        {
            //cast objects to gameobjects and add to list
            listoftrees.Add((GameObject)trees[i]);
        }

        snowmans = Resources.LoadAll("Landscape/Snowman", typeof(GameObject));
        for (int i = 0; i < numberofsnowmantypes; i++)
        {
            //cast objects to gameobjects and add to list
            listofsnowman.Add((GameObject)snowmans[i]);
        }

        snowballs = Resources.LoadAll("Landscape/Misc", typeof(GameObject));
        for (int i = 0; i < numberofsnowballtypes; i++)
        {
            //cast objects to gameobjects and add to list
            listofsnowball.Add((GameObject)snowballs[i]);
        }
    }

    public void Create_Rockscape_Rock(Vector3 origin, float areasizex, float areasizez, int numberOfRocks)
    {
        //keep track of the number attempts we've had to find a valid location for a rock
        int rockAttempts = 0;

        //create objects in order to get the bounds
        if (rockBoundsData == false)
        {
            rock = (GameObject)Instantiate(rocks[0], new Vector3(-10000, -10000, -10000), transform.rotation);
            invalidLandscapePositions.Add(new Vector3(-10000, -10000, -10000));
            landscapeSizes.Add(rock.GetComponent<Renderer>().bounds.size.x);
            rockBoundsData = true;
        }

        bool validPositionFound = false;

        //Find random location within the area
        while (validPositionFound == false)
        {
            if (rockAttempts > numberOfRockAttempts)
            {
                Debug.Log("Failed to find space for rock within the bounds of the settlement area after " + numberOfRockAttempts + " attempts");
                return;
            }
            for (int i = 0; i < numberOfRocks; i++)
            {
                int rocktypeChosen = rnd.Next(0, numberofrocktypes);

                float randomx = Random.Range(origin.x - areasizex, origin.x + areasizex);
                float randomz = Random.Range(origin.z - areasizez, origin.z + areasizez);

                Vector3 potentialRockPosition = new Vector3(randomx, origin.y, randomz);

                //get random rotation
                float rockRotation = Random.Range(0, 360);

                if (checkpos.CheckValidPosition(potentialRockPosition, invalidLandscapePositions, landscapeSizes, rock.GetComponent<Renderer>().bounds.size.x) == true)
                {
                    Instantiate(rocks[rocktypeChosen], new Vector3(randomx, origin.y, randomz), transform.rotation * Quaternion.Euler(-90f, rockRotation, 0f));
                    invalidLandscapePositions.Add(potentialRockPosition);
                    landscapeSizes.Add(rock.GetComponent<Renderer>().bounds.size.x);
                    validPositionFound = true;
                }
                else if (checkpos.CheckValidPosition(potentialRockPosition, invalidLandscapePositions, landscapeSizes, rock.GetComponent<Renderer>().bounds.size.x) == false)
                {
                    rockAttempts++;
                }
            }
        }
    }

    

    public void Create_Trees(Vector3 origin, float sizex, float sizez, int numberOfTrees)
    {
        //keep track of the number attempts we've had to find a valid location for a rock
        int treeAttempts = 0;

        //create objects in order to get the bounds
        if (treeBoundsData == false)
        {
            tree = (GameObject)Instantiate(trees[0], new Vector3(-10000, -10000, -10000), transform.rotation);
            invalidLandscapePositions.Add(new Vector3(-10000, -10000, -10000));
            landscapeSizes.Add(tree.GetComponent<Renderer>().bounds.size.x);
            treeBoundsData = true;
        }

        bool validPositionFound = false;

        //Find random location within the area
        while (validPositionFound == false)
        {
            if (treeAttempts > numberOfTreeAttempts)
            {
                Debug.Log("Failed to find space for tree within the bounds of the settlement area after " + numberOfTreeAttempts + " attempts");
                return;
            }
            for (int i = 0; i < numberOfTrees; i++)
            {
                int treetypeChosen = rnd.Next(0, numberoftreetypes);

                float randomx = Random.Range(origin.x - sizex, origin.x + sizex);
                float randomz = Random.Range(origin.z - sizez, origin.z + sizez);

                Vector3 potentialTreePosition = new Vector3(randomx, origin.y, randomz);

                //get random rotation
                float treeRotation = Random.Range(0, 360);

                if (checkpos.CheckValidPosition(potentialTreePosition, invalidLandscapePositions, landscapeSizes, tree.GetComponent<Renderer>().bounds.size.x) == true)
                {
                    Instantiate(trees[treetypeChosen], new Vector3(randomx, origin.y, randomz), transform.rotation * Quaternion.Euler(-90f, treeRotation, 0f));
                    invalidLandscapePositions.Add(potentialTreePosition);
                    landscapeSizes.Add(tree.GetComponent<Renderer>().bounds.size.x);
                    validPositionFound = true;
                }
                else if (checkpos.CheckValidPosition(potentialTreePosition, invalidLandscapePositions, landscapeSizes, tree.GetComponent<Renderer>().bounds.size.x) == false)
                {
                    treeAttempts++;
                }
            }
        }

    }

    public void Create_Snowman(Vector3 origin, float sizex, float sizez, int numberOfSnowman)
    {
        //keep track of the number attempts we've had to find a valid location for a rock
        int snowmanAttempts = 0;


        bool validPositionFound = false;

        //Find random location within the area
        while (validPositionFound == false)
        {
            if (snowmanAttempts > numberOfSnowmanAttempts)
            {
                Debug.Log("Failed to find space for snowman within the bounds of the settlement area after " + numberOfSnowmanAttempts + " attempts");
                return;
            }
            for (int i = 0; i < numberOfSnowman; i++)
            {

                float randomx = Random.Range(origin.x - sizex, origin.x + sizex);
                float randomz = Random.Range(origin.z - sizez, origin.z + sizez);

                Vector3 potentialSnowmanPosition = new Vector3(randomx, origin.y, randomz);

                float snowmanRotation = Random.Range(0, 360);

                if (checkpos.CheckValidPosition(potentialSnowmanPosition, invalidLandscapePositions, landscapeSizes, tree.GetComponent<Renderer>().bounds.size.x) == true)
                {
                    Instantiate(snowmans[0], new Vector3(randomx, origin.y, randomz), transform.rotation * Quaternion.Euler(0f, snowmanRotation, 0f));
                    invalidLandscapePositions.Add(potentialSnowmanPosition);
                    landscapeSizes.Add(tree.GetComponent<Renderer>().bounds.size.x);
                    validPositionFound = true;
                }
                else if (checkpos.CheckValidPosition(potentialSnowmanPosition, invalidLandscapePositions, landscapeSizes, tree.GetComponent<Renderer>().bounds.size.x) == false)
                {
                    snowmanAttempts++;
                }
            }
        }
    }

    public void Create_Snowball(Vector3 origin, float sizex, float sizez, int numberOfSnowballs)
    {
        //keep track of the number attempts we've had to find a valid location for a rock
        int snowballAttempts = 0;

        //create objects in order to get the bounds
        if (snowballBoundsData == false)
        {
            snowball = (GameObject)Instantiate(snowballs[0], new Vector3(-10000, -10000, -10000), transform.rotation);
            invalidLandscapePositions.Add(new Vector3(-10000, -10000, -10000));
            landscapeSizes.Add(snowball.GetComponent<Renderer>().bounds.size.x);
            snowballBoundsData = true;
        }

        bool validPositionFound = false;

        //Find random location within the area
        while (validPositionFound == false)
        {
            if (snowballAttempts > numberOfSnowballAttempts)
            {
                Debug.Log("Failed to find space for snowball within the bounds of the settlement area after " + numberOfSnowballAttempts + " attempts");
                return;
            }
            for (int i = 0; i < numberOfSnowballs; i++)
            {

                float randomx = Random.Range(origin.x - sizex, origin.x + sizex);
                float randomz = Random.Range(origin.z - sizez, origin.z + sizez);

                Vector3 potentialSnowballPosition = new Vector3(randomx, origin.y, randomz);

                if (checkpos.CheckValidPosition(potentialSnowballPosition, invalidLandscapePositions, landscapeSizes, snowball.GetComponent<Renderer>().bounds.size.x) == true)
                {
                    Instantiate(snowballs[0], new Vector3(randomx, origin.y, randomz), transform.rotation * Quaternion.Euler(-90f, 0f, 0f));
                    invalidLandscapePositions.Add(potentialSnowballPosition);
                    landscapeSizes.Add(snowball.GetComponent<Renderer>().bounds.size.x);
                    validPositionFound = true;
                }
                else if (checkpos.CheckValidPosition(potentialSnowballPosition, invalidLandscapePositions, landscapeSizes, snowball.GetComponent<Renderer>().bounds.size.x) == false)
                {
                    snowballAttempts++;
                }
            }
        }
    }

    public void FindLandscapeOrigin(List<float> lowerx, List<float> upperx, float areasize, float landscapeSize)
    {
        Vector3 potentialOrigin = new Vector3();
        int attempts = 0;
        float potentialOriginx = Random.Range(-areasize, areasize);
        float potentialOriginz = Random.Range(-areasize, areasize);

        potentialOrigin.y = -0.6f;
        potentialOrigin.z = potentialOriginz;

        for (int i = 0; i < lowerx.Count; i++)
            {
                //check through the lower and upper lists for the settlements to find a valid origin
                if (potentialOrigin.x <= upperx[i] + landscapeSize + 5 && potentialOrigin.x >= lowerx[i] - landscapeSize - 5)
                {
                    potentialOrigin.x = Random.Range(-areasize, areasize);
                    i = 0;
                    attempts++;
                }
                if(i==lowerx.Count-1)
                {
                    //check the origin isn't in another landscape
                    potentialOrigin.x = potentialOriginx;                    
                    if (checkpos.CheckValidPosition(potentialOrigin, invalidLandscapePositions, landscapeSize, landscapeSize) == true)
                    {
                       // Debug.Log("Lowerx= " + lowerx[i]);
                       // Debug.Log("Upperx= " + upperx[i]);
                       // Debug.Log("LandscapeSize= " + landscapeSize);
                       landscapeOrigins.Add(potentialOrigin);
                       //int count = landscapeOrigins.Count;
                       //Debug.Log("A successful origin was found at " + landscapeOrigins[count - 1].x + " x value");
                        break;
                    }
                    else
                    {
                    //if we check all settlements coords and then fail the landscape check, we need to re-check all the settlements
                        i = 0;
                    }
                }
                if(attempts == 100)
                {
                    Debug.Log("Max number of attempts reached");
                    break;
                }             
            }
        

    }
}