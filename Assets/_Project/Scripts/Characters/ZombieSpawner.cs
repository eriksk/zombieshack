using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
	public List<GameObject> ZombiePrefabs;

	public Vector2 Interval;
	public Transform Container;

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
			var zombie = (GameObject)Instantiate(ZombiePrefabs[UnityEngine.Random.Range(0, ZombiePrefabs.Count)], transform.position, transform.rotation);
			if(Container != null)
				zombie.transform.parent = Container;

			Reset();
		}
	}
}
