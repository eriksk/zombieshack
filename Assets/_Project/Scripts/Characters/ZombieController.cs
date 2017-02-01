using System.Collections;
using System.Collections.Generic;
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

	void Start () 
	{
		_speed = Random.Range(Speed.x, Speed.y);
		_target = PlayerAccessSingleton.Player;
		_animator = GetComponent<Animator>();

		var health = GetComponent<Health>();
		health.OnDamaged += OnDamaged;
		health.OnDeath += OnDeath;
	}

	private void OnDamaged(int damage, Vector3 position, Vector3 velocity)
	{
		Instantiate(BloodPrefab, position, Quaternion.LookRotation(velocity, Vector3.up));
	}

	private void OnDeath(Vector3 position, Vector3 velocity)
	{
		Instantiate(BloodPrefab, position, Quaternion.LookRotation(velocity, Vector3.up));
		// Enable ragdoll, disable this controller and destroy object after some time
		GetComponent<RagdollController>().EnableRagdoll(position, velocity);
		this.enabled = false;
		// Destroy(gameObject, 6f);
	}
	
	void Update () 
	{
		if(_target == null)
		{
			_animator.SetBool("moving", false);
			return; // Perhaps walk around aimlessly instead, all zombie-like
		}

		// Euler direction to the player/target
		var directionToPlayer = (_target.transform.position - transform.position).normalized;

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer, Vector3.up), RotationSpeed);

		_animator.SetBool("moving", true);
		_animator.SetFloat("move_speed", _speed);
	}
}
