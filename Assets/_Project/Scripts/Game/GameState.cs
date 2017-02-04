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

	void Start () 
	{
		Instance = this;
		Score = 0;
	}

	public void OnZombieKilled()
	{
		Score++;
		UpdateScoreText();
	}

    private void UpdateScoreText()
    {
		ScoreText.text = Score.ToString();
    }
}
