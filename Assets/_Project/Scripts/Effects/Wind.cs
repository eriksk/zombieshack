using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour 
{
	public static float GlobalDirection = 0f;

	public static Vector3 Direction
	{
		get{ return new Vector3(Mathf.Cos(GlobalDirection), 0f, Mathf.Sin(GlobalDirection)); }
	}

	public static float GetMagnitude(Vector3 position)
	{
		float scale = 1f;
		return Mathf.PerlinNoise(position.x * scale, position.z * scale) / scale;
	}

	public float DirectionChangeSpeed = 0.1f;
	public float CurrentDirection;

	private float _directionChange;

	void Start () 
	{
		GlobalDirection = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
	}
	
	void Update () 
	{
		GlobalDirection += _directionChange * Time.deltaTime;
		WrapDirection();
		_directionChange += DirectionChangeSpeed * 0.001f *  Time.deltaTime;
		CurrentDirection = GlobalDirection;
	}

    private void WrapDirection()
    {
		var dir = GlobalDirection * Mathf.Rad2Deg;
		while(dir > 360f)
			dir -= 360f;
		while(dir < 0f)
			dir += 360f;
		GlobalDirection = dir * Mathf.Deg2Rad;
    }
}
