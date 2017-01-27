using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class Thunder : MonoBehaviour 
{
	public Light FlashLight;
	public ColorCorrectionCurves ColorCorrectionCurves;
	public Vector2 FlashDuration;
	public Vector2 WaitInterval;
	public AnimationCurve IntensityRange;

	private float _current, _interval;

	void Start()
	{
		Reset();
	}

	void Update () 
	{
		_current += Time.deltaTime;
		if(_current >= _interval)
		{
			StartCoroutine(Flash());
			Reset();
		}
	}

    private IEnumerator Flash()
    {
		if(ColorCorrectionCurves != null)
			ColorCorrectionCurves.saturation = 0.2f;
		FlashLight.enabled = true;

		var duration = UnityEngine.Random.Range(FlashDuration.x, FlashDuration.y);
		var current = 0f;

		while(current < duration)
		{
			yield return new WaitForEndOfFrame();
			current += Time.deltaTime;
			var progress = current / duration;
			FlashLight.intensity = IntensityRange.Evaluate(progress);
			if(ColorCorrectionCurves != null)
				ColorCorrectionCurves.saturation = Mathf.Lerp(0.2f, 1.34f, progress);
		}

		FlashLight.enabled = false;
		if(ColorCorrectionCurves != null)
			ColorCorrectionCurves.saturation = 1.34f;
    }

    private void Reset()
	{
		_current = 0f;
		_interval = UnityEngine.Random.Range(WaitInterval.x, WaitInterval.y);
	}
}
