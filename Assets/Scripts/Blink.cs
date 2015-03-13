/// <summary>
/// Vignette and blinking / restricted eye size effects
/// StartOpenEyeAnimationSequence needs to be called when player wakes up.
/// StartUnconsciousAnimationSequence needs to be called when player gets hit in the head.
/// </summary>

using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {


	public enum eyeSide
	{
		ES_RIGHT,
		ES_LEFT
	};
	public eyeSide eyePos;
	public float maxClosePercentage = 0.5f;
	public Material mat;
	private float currentClosePercentage;

	// Use this for initialization
	void Start () {
		mat.SetFloat("_CurrentClosePercentage", 0.5f);
		StartOpenEyeAnimationSequence ();
	}

	public void StartOpenEyeAnimationSequence()
	{
		StartCoroutine("OpenEyeAnimationSequence", maxClosePercentage);
	}

	public void StartUnconsciousAnimationSequence()
	{
		StartCoroutine("UnconsciousAnimationSequence", maxClosePercentage);
		StartCoroutine(CloseEyeAnimation(maxClosePercentage, 1.0f, 0.2f));
	}

	float filterPercentage;
	IEnumerator UnconsciousAnimationSequence()
	{
		float startTime = Time.time;
		float duration = 0.5f;
		filterPercentage = 0;
		while (filterPercentage < 1.0f) {
			float t =  (Time.time - startTime) / duration;
			filterPercentage = Mathf.SmoothStep(0.0f, 1.0f, t);
			yield return null;
		}
	}
	
	IEnumerator OpenEyeAnimationSequence(float maxClose)
	{
		StartCoroutine (OpenEyeAnimation(1.0f, maxClose * 1.75f, 2.0f));
		yield return new WaitForSeconds (1f);

		StartCoroutine (CloseEyeAnimation(maxClose * 1.75f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation(1.0f, maxClose * 1.75f, 0.1f));
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (CloseEyeAnimation(maxClose * 1.75f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation(1.0f, maxClose * 1.75f, 0.1f));
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (CloseEyeAnimation(maxClose * 1.75f, 1.0f, 3.0f));
		yield return new WaitForSeconds (2.5f);
		StartCoroutine (OpenEyeAnimation(1.0f, maxClose * 1.5f, 0.5f));
		yield return new WaitForSeconds (0.5f);

		StartCoroutine (CloseEyeAnimation(maxClose * 1.5f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation(1.0f, maxClose * 1.5f, 0.1f));
		yield return new WaitForSeconds (0.1f);

		StartCoroutine (CloseEyeAnimation(maxClose * 1.5f, 1.0f, 0.2f));
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (OpenEyeAnimation(1.0f, maxClose, 0.15f));
		yield return new WaitForSeconds (2f);
	}

	float transition;
	IEnumerator OpenEyeAnimation(float startPercentageClosed, float targetClosePercentage, float duration)
	{
		float startTime = Time.time;
		float stepSize = 0;
		transition = startPercentageClosed;
		while (transition > targetClosePercentage) {
			float t =  (Time.time - startTime) / duration;
			transition = Mathf.SmoothStep(transition, targetClosePercentage, t);
			yield return null;
		}
	}

	IEnumerator CloseEyeAnimation(float startPercentageClosed, float targetClosePercentage, float duration)
	{
		float startTime = Time.time;
		float stepSize = 0;
		transition = startPercentageClosed;
		while (targetClosePercentage > transition) 
		{
			float t =  (Time.time - startTime) / duration;
			transition = Mathf.SmoothStep(transition, targetClosePercentage, t);

			yield return null;
		}
	}

	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		
		mat.SetFloat ("_FilterPercentage", filterPercentage);
		mat.SetFloat ("_CurrentClosePercentage", transition);
		mat.SetInt ("_IsLeft", (int)eyePos);
		Graphics.Blit (source, destination, mat);
	}
}
