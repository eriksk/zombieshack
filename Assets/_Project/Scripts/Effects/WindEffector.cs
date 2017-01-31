using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffector : MonoBehaviour 
{
	[Range(0f, 1f)]
	public float Magnitude = 1f;
	
	void FixedUpdate () 
	{
		var direction = Wind.Direction;
		var magnitude = Wind.GetMagnitude(transform.position) * Magnitude;
		GetComponent<Rigidbody>().AddForce(direction * magnitude);
	}
}
