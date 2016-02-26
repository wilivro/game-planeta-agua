using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour {

	// Use this for initialization
	private Camera cam;
	void Start () {
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		cam.orthographicSize = ((Screen.height/100f) / 4f)*100;
	}
}
