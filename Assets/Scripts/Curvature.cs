// Author: Fatima Nadeem

using UnityEngine;

public class Curvature : MonoBehaviour
/*
	This class uses the custom curve shader to give a curvature to the 
	environment, which can be controlled in the XY axes.
*/
{
	[Range(-1f, 1f)]
	public float curveStrengthX = 0f;
	[Range(-1f, 1f)]
	public float curveStrengthY = 0f;

	int curveStrengthXID;
	int curveStrengthYID;

	private void Start()
    {
        curveStrengthXID = Shader.PropertyToID("_CurveStrengthX");
		curveStrengthYID = Shader.PropertyToID("_CurveStrengthY");
	}

	void Update()
	{
		Shader.SetGlobalFloat(curveStrengthXID, curveStrengthX);
		Shader.SetGlobalFloat(curveStrengthYID, curveStrengthY);
	}
}
