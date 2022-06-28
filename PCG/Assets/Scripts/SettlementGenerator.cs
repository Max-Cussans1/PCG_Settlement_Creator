using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Settlement_Creator))]
public class SettlementGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        Settlement_Creator settlementCreator = (Settlement_Creator)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate Settlements"))
        {
            settlementCreator.GenerateSettlements();
        }
    }
}
