using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour 
{
	public float FadeDuration = 1f;
	public bool StartFadingIn = true;

	private float _current;

	public void Start()
	{
		if(StartFadingIn)
			FadeIn();
		else
			FadeOut();
	}

	public void FadeIn()
	{
		StartCoroutine(Fade(true));
	}

	public void FadeOut()
	{
		StartCoroutine(Fade(false));
	}

    private IEnumerator Fade(bool fadeIn)
    {
		_current = 0f;
		SetAlpha(fadeIn ? 1f : 0f);

		while(_current <= FadeDuration)
		{
			var progress = _current / FadeDuration;
			SetAlpha(
				fadeIn ? 
				Mathf.Lerp(1f, 0f, progress) :
				Mathf.Lerp(0f, 1f, progress)
			);
			yield return new WaitForEndOfFrame();
			_current += Time.deltaTime;
		}

		SetAlpha(fadeIn ? 0f : 1f);
    }

    private void SetAlpha(float alpha)
    {
		var image = GetComponent<UnityEngine.UI.Image>();
		var col = image.color;
		col.a = alpha;
		image.color = col;
    }
}
