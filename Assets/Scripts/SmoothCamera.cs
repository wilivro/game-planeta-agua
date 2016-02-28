﻿using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour {

	// Use this for initialization
	private Camera cam;
	private GameObject player;
	void Start () {
		cam = GetComponent<Camera>();
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		cam.orthographicSize = ((Screen.height/100f) / 4f);

		transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.1f) + new Vector3(0, 0, -10);
	}
}