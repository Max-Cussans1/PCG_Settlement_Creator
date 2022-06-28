using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skyscraper : MonoBehaviour
{
    //get reference to the settlement script to access checkvalidposition methods and lists of valid positions
    CheckPositions checkpos = new CheckPositions();

    System.Random rnd = new System.Random();
    bool boundsData = false;
    GameObject ssbase = null;
    GameObject ssmidsection = null;
   
    //track attempts to find valid positions, if we hit too many we can stop trying
    int baseAttempts = 0;
    //number of attempts to stop at
    int numberOfSkyscraperAttempts = 100;

    //types of skyscraper bases we have loaded in the project and variables to check the position and an array of objects to store the types since Resources.LoadAll returns Object[]
    int numberofbases = 2;
    Vector3 basePosition;
    Object[] bases;
    public static List<GameObject> listofbases = new List<GameObject>();

    int numberofmidsections = 2;
    int numberofmidsections_tospawn;
    Object[] midsections;
    public static List<GameObject> listofmidsections = new List<GameObject>();

    int numberofroofs = 2;
    Object[] roofs;
    public static List<GameObject> listofroofs = new List<GameObject>();

    [Header("Skyscraper Settings")]
    //variables we can change in inspector to set the range of height of skyscrapers (ranges advisory to not too adversely affect performance)
    [Tooltip("The minimum height the skyscrapers will spawn with")]
    [Range(2, 9)]
    public int minimumHeight = 2;
    [Tooltip("The maximum height the skyscrapers will spawn with")]
    [Range(2, 9)]
    public int maximumHeight = 9;


    //variables we can change in inspector to set the range of windows we will try and spawn on each skyscraper (ranges advisory to not too adversely affect performance)
    [Tooltip("The minimum number of windows to try and generate on each skyscraper (may spawn less on smaller skyscrapers if there isn't room)")]
    [Range(1, 30)]
    public int minimumNumberOfWindows = 10;
    [Tooltip("The maximum number of windows to try and generate on each skyscraper (may spawn less on smaller skyscrapers if there isn't room)")]
    [Range(1, 30)]
    public int maximumNumberOfWindows = 30;

    int numberofwindows = 1;
    int numberofwindows_tospawn;
    Object[] windows;
    public static List<GameObject> listofwindows;
    List<Vector3> invalidWindowPositions = new List<Vector3>();

    //gameobjects to store the objects as we instantiate them
    GameObject gobjBase = null;
    GameObject gobjMidsection = null;
    GameObject gobjRoof = null;
    GameObject gobjWindow = null;

    //load all the models into gameobject lists
    void Awake()
    {
        bases = Resources.LoadAll("Settlement/Skyscraper/Bases", typeof(GameObject));
        for (int i = 0; i < numberofbases; i++)
        {
            //cast objects to gameobjects and add to list
            listofbases.Add((GameObject)bases[i]);            
        }

        midsections = Resources.LoadAll("Settlement/Skyscraper/Midsections", typeof(GameObject));
        for (int i = 0; i < numberofmidsections; i++)
        {
            //cast objects to gameobjects and add to list
            listofmidsections.Add((GameObject)midsections[i]);
        }

        roofs = Resources.LoadAll("Settlement/Skyscraper/Roofs", typeof(GameObject));
        for (int i = 0; i < numberofroofs; i++)
        {
            //cast objects to gameobjects and add to list
            listofroofs.Add((GameObject)roofs[i]);
        }

        windows = Resources.LoadAll("Settlement/Skyscraper/Windows", typeof(GameObject));
        for (int i = 0; i < numberofwindows - 1; i++)
        {
            //cast objects to gameobjects and add to list
            listofwindows.Add((GameObject)windows[i]);
        }   
    }

    //create skyscraper facing 1 of 4 directions with random amount of windows
    public void CreateSkyscraper(Vector3 origin, float areasizex, float areasizez)
    {
        baseAttempts = 0;        
        //keep track of the base type chosen
        int basetypeChosen = rnd.Next(0, numberofbases);

        //create objects in order to get the bounds
        if (boundsData == false)
        {
            ssbase = (GameObject)Instantiate(bases[basetypeChosen], new Vector3(-10000, -10000, -10000), transform.rotation);
            CheckPositions.invalidBuildingPositions.Add(new Vector3(-10000, -10000, -10000));
            CheckPositions.buildingSizes.Add(ssbase.GetComponent<Renderer>().bounds.size.x);
            ssmidsection = (GameObject)Instantiate(midsections[basetypeChosen], new Vector3(-10000, -10000, -10000), transform.rotation);
            boundsData = true;
        }

        //track attempts to find valid positions, if we hit too many we can stop trying
        int windowAttempts = 0;

        float randomxWindow = 0f;
        float randomyWindow = 0f;
        float randomzWindow = 0f;
        Vector3 potentialWindowPosition;

        bool validPositionFound = false;

        //Find random location within the area
        while (validPositionFound == false)
        {
            if(baseAttempts > numberOfSkyscraperAttempts)
            {
                Debug.Log("Failed to find space for skyscraper within the bounds of the settlement area after " + numberOfSkyscraperAttempts + " attempts");
                return;
            }
            float randomx = Random.Range(origin.x - areasizex, origin.x + areasizex);
            float randomz = Random.Range(origin.z - areasizez, origin.z + areasizez);

            Vector3 potentialBasePosition = new Vector3(randomx, 0, randomz);

            float baseRotation = 0f;
            int bRotation = rnd.Next(0, 4);
            switch (bRotation)
            {
                case 0:
                    baseRotation = 0f;
                    break;
                case 1:
                    baseRotation = 90f;
                    break;
                case 2:
                    baseRotation = 180f;
                    break;
                case 3:
                    baseRotation = 270f;
                    break;
            }

            //if its a valid position (not already occupied) spawn the skyscraper base
            if (checkpos.CheckValidPosition(potentialBasePosition, CheckPositions.invalidBuildingPositions, CheckPositions.buildingSizes, ssbase.GetComponent<Renderer>().bounds.size.x) == true)
            {
                gobjBase = (GameObject)Instantiate(bases[basetypeChosen], basePosition = new Vector3(randomx, 0, randomz), transform.rotation * Quaternion.Euler(0f, baseRotation, 0f));
                gobjBase.tag = "settlement";
                CheckPositions.invalidBuildingPositions.Add(potentialBasePosition);
                CheckPositions.buildingSizes.Add(ssbase.GetComponent<Renderer>().bounds.size.x);
                validPositionFound = true;
            }
            //if not, add to the attempts and start the loop again to try and find a valid position
            else
            {
                baseAttempts++;
            }
        }

        //if user error in inspector, use minimum
        if(minimumHeight > maximumHeight)
        {
            maximumHeight = minimumHeight;
        }
        //retrieve the height to spawn the number of midsections
        numberofmidsections_tospawn = rnd.Next(minimumHeight,maximumHeight);
        //get the upper bound of base y to spawn the midsection on top of the base
        float heighttospawn = ssbase.GetComponent<Renderer>().bounds.size.y;
        float lowerx_window = (basePosition.x - (0.5f * ssbase.GetComponent<Renderer>().bounds.size.x)) + 0.5f;
        float upperx_window = (basePosition.x + (0.5f * ssbase.GetComponent<Renderer>().bounds.size.x)) - 0.5f;

        float lowerz_window = (basePosition.z - (0.5f * ssbase.GetComponent<Renderer>().bounds.size.z)) + 0.5f;
        float upperz_window = (basePosition.z + (0.5f * ssbase.GetComponent<Renderer>().bounds.size.z)) - 0.5f;

        //we can spawn windows above the base
        float lowery_window = heighttospawn;

        //get random rotation for midsections
        float midsectionRotation = 0f;
        int msRotation = rnd.Next(0, 4);
        switch (msRotation)
        {
            case 0:
                midsectionRotation = 0f;
                break;
            case 1:
                midsectionRotation = 90f;
                break;
            case 2:
                midsectionRotation = 180f;
                break;
            case 3:
                midsectionRotation = 270f;
                break;
        }

        for (int i = 0; i < numberofmidsections_tospawn + 1; i++)
        {
            //choose same type as the base
            gobjMidsection = (GameObject)Instantiate(midsections[basetypeChosen], new Vector3(basePosition.x, heighttospawn, basePosition.z), transform.rotation * Quaternion.Euler(0f, midsectionRotation, 0f));
            gobjMidsection.tag = "settlement";
            heighttospawn += ssmidsection.GetComponent<Renderer>().bounds.size.y;
        }
        //we can spawn windows below the roof
        float uppery_window = heighttospawn - 1.5f;

        float correctionRotation = 0f;
        if(basetypeChosen == 0)
        {
            correctionRotation = -90f;
        }

        gobjRoof = (GameObject)Instantiate(roofs[basetypeChosen], new Vector3(basePosition.x, heighttospawn, basePosition.z), transform.rotation * Quaternion.Euler(correctionRotation, 0f, 0f));
        gobjRoof.tag = "settlement";

        //if user error in inspector, use minimum
        if (minimumNumberOfWindows > maximumNumberOfWindows)
        {
            maximumNumberOfWindows = minimumNumberOfWindows;
        }
        //create windows between the range we have specified in inspector
        numberofwindows_tospawn = rnd.Next(minimumNumberOfWindows,maximumNumberOfWindows);

        //if it is the correct type of skyscraper to spawn windows
        if(basetypeChosen == 0)
        {
            for (int i = 0; i < numberofwindows_tospawn; i++)
            {
                //if we fail to find a valid position after 100 attempts we stop trying
                if (windowAttempts > 100)
                {
                    return;
                }

                //which side the window will be on
                int r = rnd.Next(0, 4);
                switch (r)
                {
                    case 0:
                        randomxWindow = Random.Range(lowerx_window, upperx_window);
                        randomyWindow = Random.Range(lowery_window, uppery_window);

                        potentialWindowPosition = new Vector3(randomxWindow, randomyWindow, lowerz_window - 0.5f);

                        //use CheckValidPosition method to see if the mesh will be too close to another window that's already been spawned
                        if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == true)
                        {
                            gobjWindow = (GameObject)Instantiate(windows[basetypeChosen], new Vector3(randomxWindow, randomyWindow, lowerz_window - 0.5f), transform.rotation * Quaternion.Euler(0f, 90f, 0f));
                            gobjWindow.tag = "settlement";
                            invalidWindowPositions.Add(potentialWindowPosition);
                        }
                        else if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == false)
                        {
                            i--;
                            windowAttempts++;
                            break;
                        }
                        break;

                    case 1:
                        randomyWindow = Random.Range(lowery_window, uppery_window);
                        randomzWindow = Random.Range(lowerz_window, upperz_window);

                        potentialWindowPosition = new Vector3(lowerx_window - 0.6f, randomyWindow, randomzWindow);

                        //use CheckValidPosition method to see if the mesh will be too close to another window that's already been spawned
                        if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == true)
                        {
                            gobjWindow = (GameObject)Instantiate(windows[basetypeChosen], new Vector3(lowerx_window - 0.6f, randomyWindow, randomzWindow), transform.rotation);
                            gobjWindow.tag = "settlement";
                            invalidWindowPositions.Add(potentialWindowPosition);
                        }
                        else if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == false)
                        {
                            i--;
                            windowAttempts++;
                            break;
                        }
                        break;

                    case 2:
                        randomxWindow = Random.Range(lowerx_window, upperx_window);
                        randomyWindow = Random.Range(lowery_window, uppery_window);

                        potentialWindowPosition = new Vector3(randomxWindow, randomyWindow, upperz_window + 0.6f);

                        //use CheckValidPosition method to see if the mesh will be too close to another window that's already been spawned
                        if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == true)
                        {
                            gobjWindow = (GameObject)Instantiate(windows[basetypeChosen], new Vector3(randomxWindow, randomyWindow, upperz_window + 0.6f), transform.rotation * Quaternion.Euler(0f, 90f, 0f));
                            gobjWindow.tag = "settlement";
                            invalidWindowPositions.Add(potentialWindowPosition);
                        }
                        else if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == false)
                        {
                            i--;
                            windowAttempts++;
                            break;
                        }
                        break;

                    case 3:
                        randomyWindow = Random.Range(lowery_window, uppery_window);
                        randomzWindow = Random.Range(lowerz_window, upperz_window);

                        potentialWindowPosition = new Vector3(upperx_window + 0.5f, randomyWindow, randomzWindow);

                        //use CheckValidPosition method to see if the mesh will be too close to another window that's already been spawned
                        if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == true)
                        {
                            gobjWindow = (GameObject)Instantiate(windows[basetypeChosen], new Vector3(upperx_window + 0.5f, randomyWindow, randomzWindow), transform.rotation);
                            gobjWindow.tag = "settlement";
                            invalidWindowPositions.Add(potentialWindowPosition);
                        }
                        else if (checkpos.CheckValidPosition(potentialWindowPosition, invalidWindowPositions, 0.5f, 0.5f) == false)
                        {
                            i--;
                            windowAttempts++;
                            break;
                        }
                        break;
                }
            }
        }
    }
}