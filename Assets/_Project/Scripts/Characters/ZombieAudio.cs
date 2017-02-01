using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZombieAudio : MonoBehaviour 
{
	public List<AudioClip> Grunts;
	public AudioClip Death;
	public AudioClip Attack;
	public AudioClip Splatter;

	public Vector2 GruntInterval;

	private AudioSource _source;
	private float _wait;

	// TODO: add släpande ljud när dom går omkring Animator.GetBool("moving")

	void Start () 
	{
		_source = GetComponent<AudioSource>();
		_source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
		var health = GetComponent<Health>();

		health.OnDeath += (x, y) => 
		{
			_source.PlayOneShot(Death);
			_source.PlayOneShot(Splatter);
		};
	}
	
	void Update () 
	{
		_wait -= Time.deltaTime;
		if(_wait <= 0f)
		{
			_source.PlayOneShot(Grunts[UnityEngine.Random.Range(0, Grunts.Count)]);
			_wait = UnityEngine.Random.Range(GruntInterval.x, GruntInterval.y);
		}
	}
}
