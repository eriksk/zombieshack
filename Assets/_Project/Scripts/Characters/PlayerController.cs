using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public Animator Animator;
	public Transform Muzzle;
	public GameObject BulletPrefab;
	[Range(0.1f, 45f)]
	public float Spread = 0f;
	[Range(0.1f, 3f)]
	public float ReloadTime = 0.5f;
	public GameObject MuzzleFlashPrefab;

	private float _currentReload = 0f;
	private Rigidbody _rigidBody;
	private PlayerIKController _ik;
	
	void Start()
	{
		Animator.SetBool("alive", true);
		Animator.SetBool("moving", false);
		Animator.SetFloat("forward", 0f);
		Animator.SetFloat("side", 0f);
		_currentReload = float.MaxValue;
		_rigidBody = GetComponent<Rigidbody>();
		_ik = GetComponent<PlayerIKController>();
	}

	void Update () 
	{
		if(Animator == null) return;

		var leftStick = new Vector2(Input.GetAxis("LeftStickX"), -Input.GetAxis("LeftStickY"));
		var rightStick = new Vector2(Input.GetAxis("RightStickX"), -Input.GetAxis("RightStickY"));

		var movement = leftStick.magnitude > 0.1f ? leftStick : Vector2.zero;
		var aim = rightStick.magnitude > 0.1f ? rightStick.normalized : Vector2.zero; // TODO: current angle

		MoveAndAim(movement, aim);


		if(Input.GetAxis("RightTrigger") > 0.3f && _currentReload >= ReloadTime)
		{
			Fire();
			_currentReload = 0f;
		}

		_currentReload += Time.deltaTime;
		_rigidBody.angularVelocity = Vector3.zero;
	}

	private Vector2 TransformStickFromCameraView(Vector2 stick)
	{
		return TransformDirection(stick, Camera.main.transform);
	}

	private Vector2 TransformDirection(Vector2 stick, Transform transform)
	{
		var dir = transform.TransformDirection(new Vector3(stick.x, 0f, stick.y));
		dir.y = 0f;
		return new Vector2(dir.x, dir.z);
	}


    private void Fire()
    {
		for (int i = 0; i < 8; i++)
		{
			var rotationOffset = new Vector3(
				UnityEngine.Random.Range(-Spread, Spread),
				UnityEngine.Random.Range(-Spread, Spread),
				UnityEngine.Random.Range(-Spread, Spread)
				);

			var fireAngle = Muzzle.rotation.eulerAngles;
			fireAngle.z = 0;
			fireAngle.x = 0f;
			var bullet = (GameObject)Instantiate(BulletPrefab, Muzzle.position, Quaternion.Euler(fireAngle));	
			bullet.transform.Rotate(rotationOffset);
		}
		Instantiate(MuzzleFlashPrefab, Muzzle.position, Muzzle.rotation);
    }

    private void MoveAndAim(Vector2 movement, Vector2 aim)
    {
		// aim = TransformStickFromCameraView(aim);

		if(aim.magnitude > 0.1f)
		{
			float angle = Mathf.Atan2(-aim.y, aim.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), 0.3f);
			_ik.SetWeightsOnAllSolvers(1f);
		}
		else
		{
			_ik.SetWeightsOnAllSolvers(0f);
		}

		var transformedMovement = movement; // TransformStickFromCameraView(movement);
		// transformedMovement = TransformDirection(transformedMovement, transform);

		var m = transform.TransformDirection(new Vector3(movement.x, 0f, -movement.y));
		m.y = 0f;

		Animator.SetBool("moving", movement.magnitude > 0.1f);
		Animator.SetFloat("forward", m.x);
		Animator.SetFloat("side", m.z);

    }
}
