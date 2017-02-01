using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
	public GameObject ZombiePrefab;

	public Vector2 Interval;

	private float _currentInterval;
	private float _current;

	void Start()
	{
		Reset();
	}

    private void Reset()
    {
		_current = 0f;
		_currentInterval = UnityEngine.Random.Range(Interval.x, Interval.y);
    }

    void Update () 
	{
		_current += Time.deltaTime;
		if(_current >= _currentInterval)
		{
			Instantiate(ZombiePrefab, transform.position, transform.rotation);
			Reset();
		}
	}
}
