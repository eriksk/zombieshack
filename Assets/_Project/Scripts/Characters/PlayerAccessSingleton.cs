using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccessSingleton
{
	private static GameObject _player;
	
	public static GameObject Player
	{
		get 
		{
			if(_player == null)
				_player = GameObject.Find("Player");
			return _player; 
		}
	}
}
