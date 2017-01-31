using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIKController : MonoBehaviour 
{
	public List<IKSolver> Solvers;

	private Animator _animator;
	 
	void Start()
	{
		_animator = GetComponent<Animator>();
	}

	public void SetWeightsOnAllSolvers(float weight)
	{
		if(Solvers == null) return;

		foreach(var solver in Solvers)
		{
			if(solver == null) continue;
			solver.Weight = Mathf.Lerp(solver.Weight, weight, 0.2f);
		}
	}

	void OnAnimatorIK()
	{
		if(Solvers == null) return;

		foreach(var solver in Solvers)
		{
			if(solver == null) continue;

			_animator.SetIKPositionWeight(solver.Goal, solver.Weight);
			_animator.SetIKRotationWeight(solver.Goal, solver.Weight);  
			_animator.SetIKPosition(solver.Goal, solver.Effector.position);
			_animator.SetIKRotation(solver.Goal, solver.Effector.rotation);
		}
	}
}

[Serializable]
public class IKSolver
{
	public string Name;
	public Transform Effector;
	[Range(0f, 1f)]
	public float Weight = 1f;
	public AvatarIKGoal Goal;
}
