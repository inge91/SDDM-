/// <summary>
/// Vignette and blinking / restricted eye size effects
/// StartOpenEyeAnimationSequence needs to be called when player wakes up.
/// StartUnconsciousAnimationSequence needs to be called when player gets hit in the head.
/// </summary>

using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {
	public float maxClosePercentage = 0.5f;
	public Material mat;
	private float currentClosePercentage;
	
	// Use this for initialization
	void Start () {
		mat.SetFloat("_CurrentClosePercentage", 0.5f);
		//StartOpenEyeAnimationSequence ();
	}

	public void StartOpenEyeAnimationSequence()
	{
		StartCoroutine("OpenEyeAnimationSequence", maxClosePercentage);
	}

	public void StartUnconsciousAnimationSequence()
	{
		StartCoroutine("UnconsciousAnimationSequence", maxClosePercentage);
		StartCoroutine(CloseEyeAnimation2(maxClosePercentage, 1.0f, 0.2f));
	}

	IEnumerator UnconsciousAnimationSequence()
	{
		float startTime = Time.time;
		float duration = 0.5f;
		float filterPercentage = 0;
		while (filterPercentage < 1.0f) {
			float t =  (Time.time - startTime) / duration;
			filterPercentage = Mathf.SmoothStep(0.0f, 1.0f, t);
			mat.SetFloat ("_FilterPercentage", filterPercentage);
			yield return null;
		}
	}
	
	IEnumerator OpenEyeAnimationSequence(float maxClose)
	{
		StartCoroutine (OpenEyeAnimation2(1.0f, maxClose * 1.75f, 2.0f));
		yield return new WaitForSeconds (1f);

		StartCoroutine (CloseEyeAnimation2(maxClose * 1.75f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation2(1.0f, maxClose * 1.75f, 0.1f));
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (CloseEyeAnimation2(maxClose * 1.75f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation2(1.0f, maxClose * 1.75f, 0.1f));
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (CloseEyeAnimation2(maxClose * 1.75f, 1.0f, 3.0f));
		yield return new WaitForSeconds (2.5f);
		StartCoroutine (OpenEyeAnimation2(1.0f, maxClose * 1.5f, 0.5f));
		yield return new WaitForSeconds (0.5f);

		StartCoroutine (CloseEyeAnimation2(maxClose * 1.5f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation2(1.0f, maxClose * 1.5f, 0.1f));
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (CloseEyeAnimation2(maxClose * 1.5f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation2(1.0f, maxClose, 0.15f));
		yield return new WaitForSeconds (2f);
	}

	IEnumerator OpenEyeAnimation2(float startPercentageClosed, float targetClosePercentage, float duration)
	{
		float startTime = Time.time;
		float stepSize = 0;

		while (startPercentageClosed > targetClosePercentage) {
			float t =  (Time.time - startTime) / duration;
			startPercentageClosed = Mathf.SmoothStep(startPercentageClosed, targetClosePercentage, t);
			mat.SetFloat ("_CurrentClosePercentage", startPercentageClosed);
			yield return null;
		}
	}

	IEnumerator CloseEyeAnimation2(float startPercentageClosed, float targetClosePercentage, float duration)
	{
		float startTime = Time.time;
		float stepSize = 0;
		
		while (targetClosePercentage > startPercentageClosed) 
		{
			float t =  (Time.time - startTime) / duration;
			startPercentageClosed = Mathf.SmoothStep(startPercentageClosed, targetClosePercentage, t);
			mat.SetFloat ("_CurrentClosePercentage", startPercentageClosed);
			yield return null;
		}
	}

	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit (source, destination, mat);
	}
}
