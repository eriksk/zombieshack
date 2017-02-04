using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
	public static GameState Instance { get; private set; }

	public Text ScoreText;
	public int Score = 0;
	public int MaxDeadBodies = 5;

	private List<GameObject> _deadZombies;

    void Start () 
	{
		if(Instance != null)
		{
			Destroy(Instance);
		}
		Instance = this;
		Reset();
	}

    private void Reset()
    {
		Score = 0;
		_deadZombies = new List<GameObject>();
    }

	void Update()
	{
		while(_deadZombies.Count > MaxDeadBodies)
		{
			var zombie = _deadZombies[0];
			_deadZombies.RemoveAt(0);
			Destroy(zombie);
		}
	}

	public void OnZombieKilled(GameObject zombie)
	{
		_deadZombies.Add(zombie);
		Score++;
		UpdateScoreText();
	}

    private void UpdateScoreText()
    {
		ScoreText.text = Score.ToString();
    }
}
