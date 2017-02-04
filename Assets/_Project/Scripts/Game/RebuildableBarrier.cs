using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class RebuildableBarrier : MonoBehaviour 
{
	public GameObject RebuildEffectPrefab;
	public GameObject TeardownEffectPrefab;
	public GameObject DamagedEffectPrefab;
	public float TeardownDistance = 2f;
	public float TearDownDuration = 1f;

	public Transform ExclamationMark;

	public AudioClip[] DamagedClips;
	public AudioClip TearDownClip;

	private float _currentTeardown;
	private Vector3 _startPosition;
	private Health _health;
	private AudioSource _audio;

	public BarrierState _state = BarrierState.Up;

	void Start () 
	{
		ExclamationMark.gameObject.SetActive(false);
		_audio = GetComponent<AudioSource>();
		_state = BarrierState.Up;
		_startPosition = transform.position;
		_health = GetComponent<Health>();
		_health.OnDamaged += OnDamaged;
		_health.OnDeath += OnDeath;
	}

	private void TearDown()
	{
		_audio.PlayOneShot(TearDownClip);
		ExclamationMark.gameObject.SetActive(true);
		_state = BarrierState.GoingUpOrDown;
		StartCoroutine(Move(true));
		if(TeardownEffectPrefab != null)
			Instantiate(TeardownEffectPrefab, transform.position, transform.rotation);
	}

	private void Rebuild()
	{
		ExclamationMark.gameObject.SetActive(false);
		_state = BarrierState.GoingUpOrDown;
		_health.Reset();
		StartCoroutine(Move(false));
		if(RebuildEffectPrefab != null)
			Instantiate(RebuildEffectPrefab, transform.position, transform.rotation);
	}

    private IEnumerator Move(bool down)
    {
		_currentTeardown = 0f;
		var currentPosition = transform.position;
		var targetPosition = down ? 
			_startPosition + (Vector3.down * TeardownDistance) : 
			_startPosition;
		while(_currentTeardown <= TearDownDuration)
		{
			_currentTeardown += Time.deltaTime;
			var progress = _currentTeardown / TearDownDuration;
			transform.position = Vector3.Lerp(currentPosition, targetPosition, progress);
			yield return new WaitForEndOfFrame();
		}
		transform.position = targetPosition;

		_state = down ? BarrierState.Down : BarrierState.Up;
    }

    private void OnDeath(Vector3 position, Vector3 velocity)
    {
		TearDown();
    }

    private void OnDamaged(int damage, Vector3 position, Vector3 velocity)
    {
		_audio.PlayOneShot(DamagedClips[UnityEngine.Random.Range(0, DamagedClips.Length)]);
		if(DamagedEffectPrefab != null)
			Instantiate(DamagedEffectPrefab, transform.position, transform.rotation);
    }

	private void OnTriggerStay(Collider collider)
	{
		TryDestroyBarrierIfZombie(collider);
		TryRebuildingBarrier(collider);
	}

	private void OnTriggerEnter(Collider collider)
	{
		TryDestroyBarrierIfZombie(collider);
		TryRebuildingBarrier(collider);
	}

    private void TryDestroyBarrierIfZombie(Collider collider)
    {
		if(_health.Dead) return;
		if(_state == BarrierState.GoingUpOrDown) return;

		var zombie = collider.gameObject.GetComponent<ZombieController>();
		if(zombie == null) return;

		if(zombie.CanAttack)
		{
			zombie.ResetAttack();
			_health.Deal(1, collider.transform.position, Vector3.zero);
		}
    }

    private void TryRebuildingBarrier(Collider collider)
    {
		if(_health.Alive) return;
		if(_state == BarrierState.GoingUpOrDown) return;

		var root = collider.transform.root.gameObject;
		if(root.name != "Player") return;

		Rebuild();
    }
}

public enum BarrierState
{
	Up,
	Down,
	GoingUpOrDown
}
