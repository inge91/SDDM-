/// <summary>
/// Author Inge Becht
/// Calculates a Discrete Gaussian blur kernel and sends it to the active material.
/// User can specify Kernel size, step size(this determines which points on the texture get sampled,) 
/// and the derivation of the Gaussian kernel.
/// </summary>
using UnityEngine;
using System.Collections;

public class GaussianBlur : MonoBehaviour {

	public float sigma = 1.0f;
	public int kernelSize = 3;
	public float stepSize = 0.001f;
	float normalization;
	public Material mat;

	void Start () {
		Matrix4x4 m = new Matrix4x4 (); 
		int index = (kernelSize - 1) / 2; 
		float total = 0;
		float aTerm = 1 / (sigma * Mathf.Sqrt(Mathf.PI * 2));
		for (int i = - index; i <= index; i ++) {
			total += aTerm * Mathf.Exp(- (Mathf.Pow(i, 2) / (2 * sigma)));
		}

		Vector3 v = new Vector3(0,0,0);
		normalization = 1 / total;
		float[,] r = new float[4,4];

		for (int i = 0; i <= index; i++) {
			r[i / 4, i % 4] = aTerm * Mathf.Exp(-(Mathf.Pow(i, 2) / (2 * sigma))) * normalization;
		}

		m.m00 = r [0, 0];
		m.m10 = r [1, 0];
		m.m20 = r [2, 0];
		m.m30 = r [3, 0];
		m.m01 = r [0, 1];
		m.m11 = r [1, 1];
		m.m21 = r [2, 1];
		m.m31 = r [3, 1];
		m.m02 = r [0, 2];
		m.m12 = r [1, 2];
		m.m22 = r [2, 2];
		m.m32 = r [3, 2];
		m.m03 = r [0, 3];
		m.m13 = r [1, 3];
		m.m23 = r [2, 3];
		m.m33 = r [3, 3];

		mat.SetInt ("_StartIndex", index);
		mat.SetFloat ("_StepSize", stepSize);
		mat.SetMatrix ("_GaussianKernel", m);
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit (source, destination, mat);
	}
}
