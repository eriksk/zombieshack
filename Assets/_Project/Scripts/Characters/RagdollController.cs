using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour 
{
	public List<GameObject> RagdollParts;
	public Collider CharacterCollider;
	public Animator Animator;

	void Start()
	{
		// Disable all colliders for the ragdoll
		foreach(var part in RagdollParts)
		{
			var collider = part.GetComponent<Collider>();
			
			// if(collider != null)
			// 	collider.enabled = false;
		}
	}

	public void EnableRagdoll()
	{
		EnableRagdoll(transform.position, Vector3.zero);
	}

	public void EnableRagdoll(Vector3 position, Vector3 velocity)
	{
		// Disable character animator and collider
		CharacterCollider.enabled = false;
		Animator.enabled = false;
		// Destroy rigidbody since it cannot be disabled
		Destroy(CharacterCollider.gameObject.GetComponent<Rigidbody>());

		// Enable all colliders for the ragdoll
		foreach(var part in RagdollParts)
		{
			var collider = part.GetComponent<Collider>();
			var rigidBody = part.GetComponent<Rigidbody>();

			if(collider != null)
				collider.enabled = true;
			
			// Clear out any existing forces
			if(rigidBody != null)
			{
				rigidBody.velocity = Vector3.zero;
				rigidBody.angularVelocity = Vector3.zero;
				// Bump it up a bit
				rigidBody.AddForceAtPosition(velocity * UnityEngine.Random.Range(0.1f, 0.8f), position, ForceMode.Impulse);
			}
		}
	}
}
