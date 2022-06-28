using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPositions
{
    public static List<Vector3> invalidBuildingPositions = new List<Vector3>();
    public static List<float> buildingSizes = new List<float>();

    public bool CheckValidPosition(Vector3 position, List<Vector3> invalidPositions, List<float> sizeOfMesh, float sizeOfThisMesh)
    {
        bool isValid = false;
        for (int i = 0; i < invalidPositions.Count; i++)
        {
            if (Vector3.Distance(position, invalidPositions[i]) < sizeOfMesh[i] * 2 || Vector3.Distance(position, invalidPositions[i]) < sizeOfThisMesh * 2)
            {
                return isValid;
            }
        }
        isValid = true;
        return isValid;
    }

    public bool CheckValidPosition(Vector3 position, List<Vector3> invalidPositions, float sizeOfMesh, float sizeOfThisMesh)
    {
        bool isValid = false;
        for (int i = 0; i < invalidPositions.Count; i++)
        {
            if (Vector3.Distance(position, invalidPositions[i]) < sizeOfMesh * 2 || Vector3.Distance(position, invalidPositions[i]) < sizeOfThisMesh * 2)
            {
                return isValid;
            }
        }
        isValid = true;
        return isValid;
    }
}
