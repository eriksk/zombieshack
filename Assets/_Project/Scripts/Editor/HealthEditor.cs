using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Deal 1 damage (test)"))
		{
			(target as Health).Deal(1, Vector3.zero, Vector3.zero);
		}
	}
}

