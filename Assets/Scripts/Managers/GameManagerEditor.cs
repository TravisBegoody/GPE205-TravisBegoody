using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private int lastSeed;
    public override void OnInspectorGUI()
    {
        GameManager gameMan = (GameManager)target;

        if (DrawDefaultInspector())
        {
            if(gameMan.seed != lastSeed)
            {
                gameMan.StartUpdate();
            }
            gameMan.seed = lastSeed;
        }
        if (GUILayout.Button("Generate Map"))
        {
            gameMan.StartUpdate();
        }
        if (GUILayout.Button("Destroy Map"))
        {
            gameMan.Map.DestroyMap();
        }
    }
}
