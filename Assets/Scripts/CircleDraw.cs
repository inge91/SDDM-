using UnityEngine;
using System.Collections;

public class CircleDraw : MonoBehaviour {


	public float maxRadius = 5;
	public bool scaleWithInput = false;

	private float currentRadius;
	private bool drawCircle;

	// Use this for initialization
	void Start () {
		drawCircle = true;
		currentRadius = maxRadius;
		DrawCircle(currentRadius);
	}
	

	void Update()
	{
		if (scaleWithInput && drawCircle) {

			float input = Input.GetAxis ("Vertical");
			if (input > 0)
			{
				currentRadius +=  input * 2f * Time.deltaTime;
			}
			else if (input < 0)
			{
				currentRadius += input * 2f * Time.deltaTime;
			}
		
			currentRadius = Mathf.Clamp(currentRadius, 0, maxRadius);
			DrawCircle(currentRadius);
		}
	}


	void DrawCircle(float radius)
	{
		float step_size = (2 * Mathf.PI) / 64.0f;
		int i = 0;
		for(float currentStep  = 0; currentStep < 2 * Mathf.PI; currentStep += step_size)
		{
			float x = radius * Mathf.Cos(currentStep) + this.transform.position.x;
			float z = radius * Mathf.Sin(currentStep) + this.transform.position.z;

			Vector3 pos = new Vector3(x, this.transform.position.y, z);
			this.GetComponent<LineRenderer>().SetPosition(i, pos);
			i++;
		}

		float finalx = radius * Mathf.Cos(0) + this.transform.position.x;
		float finalz = radius * Mathf.Sin(0) + this.transform.position.z;
		Vector3 finalpos = new Vector3(finalx, this.transform.position.y, finalz);

		this.GetComponent<LineRenderer>().SetPosition(i, finalpos);
	}

	public float GetCurrentCircleRadius()
	{
        return maxRadius;
	}

	public void SetCurrentCircleRadius(float radius)
	{
		maxRadius = radius;
		DrawCircle(maxRadius);
	}
	
	public void DrawCircle(bool _drawCircle)
	{
		drawCircle = _drawCircle;
		this.GetComponent<LineRenderer> ().enabled = drawCircle;
	}
}
