using UnityEngine;
using System.Collections;

public class DiscreteDistanceEstimation : MonoBehaviour {

	public float maxRadius;
	public float[] radiiOfEstimationInterval;
	public Material maxCircelMaterial;
	public Material betweenCircelsMaterial;
	public Material selectionCircelMaterial;

	public GameObject selectionCircle;
	GameObject[] selectableCircles;

	int currentIndex;
	// Use this for initialization
	void Start () {
		selectableCircles = new GameObject[3];
		createCircleObject (maxRadius, 0.15f, "OuterRing", maxCircelMaterial, 0);
		for(int i = 0; i < radiiOfEstimationInterval.Length; i++)
		{
			selectableCircles[i] =  createCircleObject (radiiOfEstimationInterval[i], 0.05f, "ring", betweenCircelsMaterial, 1);
		}
		currentIndex = radiiOfEstimationInterval.Length - 1;

		LineRenderer lineRenderer = selectionCircle.GetComponent<LineRenderer> ();
		lineRenderer.SetVertexCount (66);
		lineRenderer.SetWidth (0.05f, 0.05f);
		lineRenderer.material = selectionCircelMaterial;
		selectionCircle.GetComponent<CircleDraw>().SetCurrentCircleRadius(radiiOfEstimationInterval[currentIndex]);
	}

	bool axisMoved = false;
	// Update is called once per frame
	void Update () {
		float input = Input.GetAxis ("Vertical");
		if (input > 0 && !axisMoved) {
			currentIndex = Mathf.Clamp ((--currentIndex), 0, radiiOfEstimationInterval.Length - 1);  
			axisMoved = true;
		} else if (input < 0 && !axisMoved) {
			currentIndex = Mathf.Clamp ((++currentIndex), 0, radiiOfEstimationInterval.Length - 1);   
			axisMoved = true;
		} else if(input == 0) {
			axisMoved = false;
		}
		float radius = selectableCircles[currentIndex].GetComponent<CircleDraw>().GetCurrentCircleRadius();
		selectionCircle.GetComponent<CircleDraw>().SetCurrentCircleRadius(radius);	
	}

	GameObject createCircleObject(float radius, float lineWidth, string name, Material m, float height)
	{
		GameObject g = new GameObject();
		g.transform.parent = this.gameObject.transform;
		Vector4 v = g.transform.position;
		v.y = height;
		g.transform.position = v;
		g.name = name;
		LineRenderer lineRenderer = g.AddComponent<LineRenderer> ();
		lineRenderer.SetVertexCount (66);
		lineRenderer.SetWidth (lineWidth, lineWidth);
		lineRenderer.material = m;
		g.AddComponent<CircleDraw>().SetCurrentCircleRadius(radius);
		return g;
	}

}
