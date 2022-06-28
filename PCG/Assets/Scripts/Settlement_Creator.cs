using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Settlement_Creator : MonoBehaviour
{
    System.Random rnd = new System.Random();
    CheckPositions checkpos = new CheckPositions();

    GameObject[] generatedObjects;

    [Header("Settlement Settings")]
    [Tooltip("The total area to try and generate settlements")]
    [Range(150, 600)]
    public float areaToSpawnSettlements = 150;

    [Tooltip("Number of settlements to try and generate within the area")]
    public int numberOfSettlements = 5;

    [Tooltip("The total area between each settlement")]
    public float settlementSpacing = 30;


    [Tooltip("Number of skyscrapers to try and generate for each settlement")]
    [Range(1, 15)]
    public int numberOfSkyscrapers = 10;

    [Tooltip("Number of houses to try and generate for each settlement")]
    [Range(5, 20)]
    public int numberOfHouses = 5;

    [Header("Settlement Dimensions")]
    [Range(10, 30)]
    public float settlementSizex = 25f;
    [Range(10, 30)]
    public float settlementSizez = 25f;


    


    public static List<Vector3> settlementOrigins = new List<Vector3>();
    public static List<float> settlementSizeXs = new List<float>();
    public static List<float> settlementSizeZs = new List<float>();

    int settlementAttempts = 0;


    public static List<float> settlementLowerXs = new List<float>();
    public static List<float> settlementUpperXs = new List<float>();

    //house obj
    GameObject houseobj = null;
    House house;

    //skyscraper obj
    GameObject ssobj = null;
    Skyscraper skyscraper;

    // Use this for initialization
    void Start()
    {
        houseobj = GameObject.Find("HouseObject");
        house = houseobj.AddComponent<House>();

        ssobj = GameObject.Find("SkyscraperObject");
        skyscraper = ssobj.AddComponent<Skyscraper>();

        //GameObject lsobj = GameObject.Find("Landscape_Manager");
        //Landscape landscape = lsobj.AddComponent<Landscape>();

        FindSettlementPositions();

        for (int i = 0; i < settlementOrigins.Count; i++)
        {
            for (int j = 0; j < numberOfHouses; j++)
            {
                house.CreateHouse(settlementOrigins[i], settlementSizeXs[i], settlementSizeZs[i]);
            }

            for (int j = 0; j < numberOfSkyscrapers; j++)
            {
                skyscraper.CreateSkyscraper(settlementOrigins[i], settlementSizeXs[i], settlementSizeZs[i]);
            }

            //Debug.Log(skyscraper.numberToSpawn + " Skyscrapers created for settlement " + i);
            //Debug.Log(house.numberToSpawn + " Houses created for settlement " + i);
            //Debug.Log("Settlement Origin:" + settlementOrigins[i].x + ", " + settlementOrigins[i].z);
            Vector3 origin = new Vector3(settlementOrigins[i].x, -0.55f, settlementOrigins[i].z);
        }

        //Landscapes WIP

      // for (int i = 0; i < 5; i++)
      // {
      //     float areaSizeX = rnd.Next(10, 15);
      //     float areaSizeZ = rnd.Next(10, 15);
      //     int numberOfRocks = rnd.Next(5, 10);
      //     int numberOfTrees = rnd.Next(5, 10);
      //     
      //
      //     int originListSize = Landscape.landscapeOrigins.Count;
      //     landscape.FindLandscapeOrigin(settlementLowerXs, settlementUpperXs, 250f, 15);
      //
      //      //if we fail to find space for the landscape the list will not have increased
      //      if (i+1 == Landscape.landscapeOrigins.Count)
      //      {
      //          landscape.Create_Rockscape_Rock(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, numberOfRocks);
      //          landscape.Create_Trees(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, numberOfTrees);
      //          int snowFeature = rnd.Next(0, 2);
      //          if (snowFeature == 0)
      //          {
      //              landscape.Create_Snowman(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, 1);
      //          }
      //          else
      //          {
      //              landscape.Create_Snowball(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, 1);
      //          }
      //      }
      // }

    }

    public void GenerateSettlements()
    {
        //clear the invalid building positions as everything is getting deleted
        CheckPositions.invalidBuildingPositions.Clear();
        //destroy all generated objects
        generatedObjects = GameObject.FindGameObjectsWithTag("settlement");
        foreach (GameObject obj in generatedObjects)
        {
            Destroy(obj);
        }
        Array.Clear(generatedObjects, 0, generatedObjects.Length);

        //GameObject lsobj = GameObject.Find("Landscape_Manager");
        //Landscape landscape = lsobj.AddComponent<Landscape>();

        FindSettlementPositions();

        for (int i = 0; i < settlementOrigins.Count; i++)
        {
            for (int j = 0; j < numberOfHouses; j++)
            {
                house.CreateHouse(settlementOrigins[i], settlementSizeXs[i], settlementSizeZs[i]);
            }

            for (int j = 0; j < numberOfSkyscrapers; j++)
            {
                skyscraper.CreateSkyscraper(settlementOrigins[i], settlementSizeXs[i], settlementSizeZs[i]);
            }

            //Debug.Log(skyscraper.numberToSpawn + " Skyscrapers created for settlement " + i);
            //Debug.Log(house.numberToSpawn + " Houses created for settlement " + i);
            //Debug.Log("Settlement Origin:" + settlementOrigins[i].x + ", " + settlementOrigins[i].z);
            Vector3 origin = new Vector3(settlementOrigins[i].x, -0.55f, settlementOrigins[i].z);
        }

        //Landscapes WIP

        //for (int i = 0; i < 5; i++)
        //{
        //    float areaSizeX = rnd.Next(10, 15);
        //    float areaSizeZ = rnd.Next(10, 15);
        //    int numberOfRocks = rnd.Next(5, 10);
        //    int numberOfTrees = rnd.Next(5, 10);
        //
        //
        //    int originListSize = Landscape.landscapeOrigins.Count;
        //    landscape.FindLandscapeOrigin(settlementLowerXs, settlementUpperXs, 250f, 15);
        //
        //    //if we fail to find space for the landscape the list will not have increased
        //    if (i + 1 == Landscape.landscapeOrigins.Count)
        //    {
        //        landscape.Create_Rockscape_Rock(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, numberOfRocks);
        //        landscape.Create_Trees(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, numberOfTrees);
        //        int snowFeature = rnd.Next(0, 2);
        //        if (snowFeature == 0)
        //        {
        //            landscape.Create_Snowman(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, 1);
        //        }
        //        else
        //        {
        //            landscape.Create_Snowball(Landscape.landscapeOrigins[i], areaSizeX, areaSizeZ, 1);
        //        }
        //    }
        //}
    }

    void FindSettlementPositions()
    {
        settlementOrigins.Clear();
        settlementLowerXs.Clear();
        settlementUpperXs.Clear();
        settlementSizeXs.Clear();
        settlementSizeZs.Clear();

        settlementAttempts = 0;
        int settlementsSpawned = 0;

        for (int i = 0; i < numberOfSettlements; i++)
        {
            Vector3 settlementOrigin = new Vector3(UnityEngine.Random.Range(-areaToSpawnSettlements, areaToSpawnSettlements), 0, UnityEngine.Random.Range(-areaToSpawnSettlements, areaToSpawnSettlements));

            if (checkpos.CheckValidPosition(settlementOrigin, settlementOrigins, settlementSpacing, settlementSpacing) == true)
            {
                settlementOrigins.Add(settlementOrigin);
                settlementsSpawned++;
                //can unhash if we want to randomise settlement size
                //settlementSizex = Random.Range(25, 30);
                settlementSizeXs.Add(settlementSizex);
                //can unhash if we want to randomise settlement size
                //settlementSizez = Random.Range(25, 30);
                settlementSizeZs.Add(settlementSizez);
                Debug.Log("Origin added:" + settlementOrigin.x + ", " + settlementOrigin.z + " with width" + settlementSizex + " and length" + settlementSizez);
                settlementLowerXs.Add(settlementOrigin.x - settlementSizex);
                settlementUpperXs.Add(settlementOrigin.x + settlementSizex);
            }
            else if (checkpos.CheckValidPosition(settlementOrigin, settlementOrigins, settlementSpacing, settlementSpacing) == false)
            {
                i--;
                settlementAttempts++;
            }
            if (settlementAttempts > 100)
            {
                Debug.Log("Failed to find origin for settlement with given spacing after 100 attempts. Found " + settlementsSpawned + "origin points");
                break;
            }
        }
    }
}
        
