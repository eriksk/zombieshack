using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RagdollController))]
public class RagdollControllerEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Enable Ragdoll"))
		{
			(target as RagdollController).EnableRagdoll();
		}
	}
}
