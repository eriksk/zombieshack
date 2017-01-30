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
	public AnimationCurve ShadowIntensityRange;
	public AnimationCurve SaturationIntensityRange;

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
			ColorCorrectionCurves.saturation = SaturationIntensityRange.Evaluate(0f);
		FlashLight.enabled = true;

		var duration = UnityEngine.Random.Range(FlashDuration.x, FlashDuration.y);
		var current = 0f;

		while(current < duration)
		{
			yield return new WaitForEndOfFrame();
			current += Time.deltaTime;
			var progress = current / duration;
			FlashLight.intensity = IntensityRange.Evaluate(progress);
			FlashLight.shadowStrength = Mathf.Clamp01(ShadowIntensityRange.Evaluate(progress));
			if(ColorCorrectionCurves != null)
				ColorCorrectionCurves.saturation = SaturationIntensityRange.Evaluate(progress);
		}

		FlashLight.enabled = false;
		if(ColorCorrectionCurves != null)
			ColorCorrectionCurves.saturation = SaturationIntensityRange.Evaluate(1f);
    }

    private void Reset()
	{
		_current = 0f;
		_interval = UnityEngine.Random.Range(WaitInterval.x, WaitInterval.y);
	}
}
