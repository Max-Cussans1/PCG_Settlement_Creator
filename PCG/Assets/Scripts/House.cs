using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    //get reference to the settlement script to access checkvalidposition methods and lists of valid positions
    CheckPositions checkpos = new CheckPositions();

    System.Random rnd = new System.Random();

    int numberOfHouseTypes = 2;
    public static List<GameObject> listofhouses = new List<GameObject>();
    Object[] houses;
    public int numberOfHouseAttempts = 100;
    GameObject house = null;
    bool boundsData = false;

    GameObject gobjHouse = null;

    //load models into list of gameobjects so they can be spawned
    void Awake ()
    {
        houses = Resources.LoadAll("Settlement/Houses", typeof(GameObject));
        for (int i = 0; i < numberOfHouseTypes; i++)
        {
            //cast objects to gameobjects and add to list
            listofhouses.Add((GameObject)houses[i]);
        }
    }
	
    public void CreateHouse(Vector3 origin, float areasizex, float areasizez)
    {
        //keep track of the house type chosen
        int housetypeChosen = rnd.Next(0, numberOfHouseTypes);
        //keep track of the number attempts we've had to find a valid location for a house
        int houseAttempts = 0;

        //create objects in order to get the bounds
        if (boundsData == false)
        {
            house = (GameObject)Instantiate(houses[0], new Vector3(-10000, -10000, -10000), transform.rotation);
            CheckPositions.invalidBuildingPositions.Add(new Vector3(-10000, -10000, -10000));
            CheckPositions.buildingSizes.Add(house.GetComponent<Renderer>().bounds.size.x);
            boundsData = true;
        }

        bool validPositionFound = false;

        //Find random location within the area
        while (validPositionFound == false)
        {
            if (houseAttempts > numberOfHouseAttempts)
            {
                Debug.Log("Failed to find space for house within the bounds of the settlement area after " + numberOfHouseAttempts + " attempts");
                return;
            }
            float randomx = Random.Range(origin.x - areasizex, origin.x + areasizex);
            float randomz = Random.Range(origin.z - areasizez, origin.z + areasizez);

            Vector3 potentialHousePosition = new Vector3(randomx, 0, randomz);

            //get random rotation
            float houseRotation = Random.Range(0,360);
            float correctionRotation = 0f;
            float correctionPositionY = 0.3f;
            if(housetypeChosen == 1)
            {
                correctionRotation = -90f;
                correctionPositionY = -0.6f;
            }

            if (checkpos.CheckValidPosition(potentialHousePosition, CheckPositions.invalidBuildingPositions, CheckPositions.buildingSizes, house.GetComponent<Renderer>().bounds.size.x) == true)
            {
                gobjHouse = (GameObject)Instantiate(houses[housetypeChosen], new Vector3(randomx, correctionPositionY, randomz), transform.rotation * Quaternion.Euler(correctionRotation, houseRotation, 0f));
                gobjHouse.tag = "settlement";
                CheckPositions.invalidBuildingPositions.Add(potentialHousePosition);
                CheckPositions.buildingSizes.Add(house.GetComponent<Renderer>().bounds.size.x);
                validPositionFound = true;
            }
            else if (checkpos.CheckValidPosition(potentialHousePosition, CheckPositions.invalidBuildingPositions, CheckPositions.buildingSizes, house.GetComponent<Renderer>().bounds.size.x) == false)
            {
                houseAttempts++;
            }
        }

    }
}
