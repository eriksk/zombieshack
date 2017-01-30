using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour 
{
	public Vector2 Speed;

	void Start ()
	{
		var rigidBody = GetComponent<Rigidbody>();
		rigidBody.velocity = transform.forward * UnityEngine.Random.Range(Speed.x, Speed.y);
		// Auto destroy after 2 secs
		Destroy(gameObject, 2f);
	}

	public void OnCollisionEnter(Collision collision)
	{
		var health = collision.gameObject.GetComponent<Health>();

		if(health != null)
		{
			var rigidBody = GetComponent<Rigidbody>();
			health.Deal(1, collision.contacts[0].point, rigidBody.velocity);
		}

		// Debug.Log("bullet " + gameObject.name + " hit " + collision.gameObject.name);
		Destroy(gameObject);
	}	
}
