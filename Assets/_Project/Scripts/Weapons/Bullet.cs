using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	public Vector2 Speed;
	public float Range = 1f;

	private Vector3 _startPosition;

	void Start ()
	{
		_startPosition = transform.position;
		var rigidBody = GetComponent<Rigidbody>();
		rigidBody.velocity = transform.forward * UnityEngine.Random.Range(Speed.x, Speed.y);
		// Auto destroy after "LifeTime" secs
		StartCoroutine(AutoDestroy());
	}

    private IEnumerator AutoDestroy()
    {
		while(Vector3.Distance(transform.position, _startPosition) < Range)
			yield return new WaitForEndOfFrame();

		Destroy(GetComponent<MeshRenderer>());
		Destroy(GetComponent<MeshFilter>());
		Destroy(GetComponent<Collider>());
		GetComponent<Rigidbody>().drag = 10f;
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
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
