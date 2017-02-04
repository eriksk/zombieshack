using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieController : MonoBehaviour 
{
	public Vector2 Speed = new Vector2();

	[Range(0.01f, 0.5f)]
	public float RotationSpeed = 0.1f;

	public GameObject BloodPrefab;

	private GameObject _target;
	private Animator _animator;
	private float _speed;

	private int _attackType = 0;
	public ZombieState _state = ZombieState.MovingToWaypoint;

	public bool CanAttack { get { return _attackCooldown <= 0f; } }
	private float _attackCooldown;

	void Start () 
	{
		_speed = UnityEngine.Random.Range(Speed.x, Speed.y);
		_target = PlayerAccessSingleton.Player;
		_animator = GetComponent<Animator>();

		var health = GetComponent<Health>();
		health.OnDamaged += OnDamaged;
		health.OnDeath += OnDeath;
	}


    internal void ResetAttack()
    {
		_attackCooldown = UnityEngine.Random.Range(1f, 5f);
    }

    private IEnumerable<Transform> GetWaypoints()
    {
		var root = GameObject.Find("Waypoints").transform;
		for(var i = 0; i < root.childCount; i++)
			yield return root.GetChild(i);
    }

    private void OnDamaged(int damage, Vector3 position, Vector3 velocity)
	{
		Instantiate(BloodPrefab, position, Quaternion.LookRotation(velocity, Vector3.up));
	}

	private void OnDeath(Vector3 position, Vector3 velocity)
	{
		GameState.Instance.OnZombieKilled(gameObject);
		Instantiate(BloodPrefab, position, Quaternion.LookRotation(velocity, Vector3.up));
		// Enable ragdoll, disable this controller and destroy object after some time
		GetComponent<RagdollController>().EnableRagdoll(position, velocity);
		this.enabled = false;
	}

	void Update () 
	{
		if(_target == null)
		{
			_animator.SetBool("moving", false);
			return; // Perhaps walk around aimlessly instead, all zombie-like
		}

		_attackCooldown -= Time.deltaTime;

		if(_state == ZombieState.MovingToWaypoint)
		{
			var nearestWaypoint = Waypoints.GetNearestWaypoint(transform);
			var directionToWaypoint = (nearestWaypoint - transform.position).normalized;
			MoveInDirection(directionToWaypoint);
			_animator.SetBool("moving", true);
			_animator.SetFloat("move_speed", _speed);

			if(Vector3.Distance(nearestWaypoint, transform.position) < 0.3f)
				_state = ZombieState.MovingToPlayer;
		}
		else if(_state == ZombieState.MovingToPlayer)
		{
			// Euler direction to the player/target
			var directionToPlayer = (_target.transform.position - transform.position).normalized;

			MoveInDirection(directionToPlayer);
			
			var distanceToPlayer = Vector3.Distance(_target.transform.position, transform.position);

			if(distanceToPlayer < 0.5f)
			{
				_animator.SetBool(_attackType == 0 ? "attacking" : "biting", true);
				_animator.SetBool("moving", false);
				_animator.SetFloat("move_speed", _speed);
			}
			else
			{
				_attackType = UnityEngine.Random.Range(0, 2);
				_animator.SetBool("attacking", false);
				_animator.SetBool("biting", false);
				_animator.SetBool("moving", true);
				_animator.SetFloat("move_speed", _speed);
			}
		}
	}

    private void MoveInDirection(Vector3 direction)
    {
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), RotationSpeed);
    }
}

public enum ZombieState
{
	MovingToWaypoint,
	MovingToPlayer
}

public class Waypoints
{
	public static Transform[] All 
	{
		get
		{
			if(_waypoints != null) return _waypoints;
			return (_waypoints = GetWaypoints().ToArray());
		}
	}
	private static Transform[] _waypoints;

    private static IEnumerable<Transform> GetWaypoints()
    {
		var root = GameObject.Find("Waypoints").transform;
		for(var i = 0; i < root.childCount; i++)
			yield return root.GetChild(i);
    }


    public static Vector3 GetNearestWaypoint(Transform transform)
    {
		var distance = float.MaxValue;
		Transform nearestWaypoint = null;

		foreach(var wp in All)
		{
			var dst = Vector3.Distance(transform.position, wp.position);
			if(dst < distance)
			{
				distance = dst;
				nearestWaypoint = wp;
			}
		}

		if(nearestWaypoint == null) return Vector3.zero;

		return nearestWaypoint.position;
    }
}