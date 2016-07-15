using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IrrigacaoPaddle : MonoBehaviour {

	// Use this for initialization
	public static int values;
	public WheelBehaviour[] valve;
	public Vector3 gain;
	public float threshold;

	RectTransform rt;

	int[] positions;

	void Start () {
		rt = GetComponent<RectTransform>();
		positions = new int[4];
		positions[0] = 13;
		positions[1] = 118;
		positions[2] = 228;
		positions[3] = 333;
	}

	// Update is called once per frame
	void Update () {
		values = Convert.ToInt32(Mathf.Abs(valve[0].angle - gain.x) <= threshold) +
				Convert.ToInt32(Mathf.Abs(valve[1].angle - gain.y) <= threshold) +
				Convert.ToInt32(Mathf.Abs(valve[2].angle - gain.z) <= threshold);

		rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, positions[values], 32);
	}
}
