using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	[Range(1, 30)]
	public int Initial = 1;

	private int _health;

	public event Death OnDeath;
	public event Damaged OnDamaged;

	public bool Alive { get { return _health > 0; } }
	public bool Dead { get { return !Alive; } }

	void Start()
	{
		Reset();
	}

    public void Reset()
    {
		_health = Initial;
    }

	public void Deal(int damage, Vector3 position, Vector3 velocity)
	{
		if(Dead) return;

		_health -= damage;
		if(_health <= 0)
		{
			_health = 0;
			InvokeOnDeath(position, velocity);
		}
		else
		{
			InvokeOnDamaged(damage, position, velocity);
		}
	}

    private void InvokeOnDeath(Vector3 position, Vector3 velocity)
    {
		if(OnDeath != null)
			OnDeath(position, velocity);
    }

    private void InvokeOnDamaged(int damage, Vector3 position, Vector3 velocity)
    {
		if(OnDamaged != null)
			OnDamaged(damage, position, velocity);
    }
}

public delegate void Death(Vector3 position, Vector3 velocity);
public delegate void Damaged(int damage, Vector3 position, Vector3 velocity);
