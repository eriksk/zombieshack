using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour 
{
	public float Speed = 1f;
	public Vector3 Angles;

	void Update ()
	{
		transform.rotation = transform.rotation * Quaternion.Euler(Angles * Speed * Time.deltaTime);		
	}
}
