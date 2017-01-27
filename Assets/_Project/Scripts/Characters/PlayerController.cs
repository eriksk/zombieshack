using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public Animator Animator;
	
	void Start()
	{
		Animator.SetBool("alive", true);
		Animator.SetBool("moving", false);
		Animator.SetFloat("forward", 0f);
		Animator.SetFloat("side", 0f);
	}

	void Update () 
	{
		if(Animator == null) return;

		var leftStick = new Vector2(Input.GetAxis("LeftStickX"), -Input.GetAxis("LeftStickY"));
		var rightStick = new Vector2(Input.GetAxis("RightStickX"), -Input.GetAxis("RightStickY"));

		var movement = leftStick.magnitude > 0.1f ? leftStick : Vector2.zero;
		var aim = rightStick.magnitude > 0.1f ? rightStick : Vector2.zero; // TODO: current angle

		MoveAndAim(movement, aim);
	}

    private void MoveAndAim(Vector2 movement, Vector2 aim)
    {
		Animator.SetBool("moving", movement.magnitude > 0.1f);
		Animator.SetFloat("forward", movement.y);
		Animator.SetFloat("side", movement.x);

		float angle = Mathf.Atan2(-aim.y, aim.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
