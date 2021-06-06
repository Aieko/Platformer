/*using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(D_BlockState))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as D_BlockState;

        myScript.blockTime = EditorGUILayout.FloatField("Block time", myScript.blockTime);

        myScript.hitParticle = EditorGUILayout.ObjectField("Hit Particle", myScript.hitParticle, typeof(GameObject), true);

        myScript.shouldConterAttack = EditorGUILayout.Toggle("Should Counter Attack", myScript.shouldConterAttack);

        myScript.shouldStay = EditorGUILayout.Toggle("Should Stay", myScript.shouldStay);

        using (new EditorGUI.DisabledScope(myScript.shouldStay))
        {
            myScript.speedWhileBlock = EditorGUILayout.FloatField("Walk Speed", myScript.speedWhileBlock);
        }

     
      

        
        /*
        using (new EditorGUI.DisabledScope(!myScript.shouldConterAttack))
        {
            myScript.speedWhileBlock = EditorGUILayout.FloatField("Counter Attack Time After Block", myScript.counterAttackTimeAfterBlock);
        }

        


        //how to disable
        myScript.disableBool = GUILayout.Toggle(myScript.disableBool, "Disable Fields");

        using (new EditorGUI.DisabledScope(myScript.disableBool))
        {
            myScript.someColor = EditorGUILayout.ColorField("Color", myScript.someColor);
            myScript.someString = EditorGUILayout.TextField("Text", myScript.someString);
            myScript.someNumber = EditorGUILayout.IntField("Number", myScript.someNumber);
        }
    }
}
*/