using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoScene : MonoBehaviour 
{
	public string TargetScene;

	public void SwitchScene() 
	{
		SceneManager.LoadScene(TargetScene);
	}
}
