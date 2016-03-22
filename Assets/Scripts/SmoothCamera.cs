using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour {

	// Use this for initialization
	private Camera cam;
	private GameObject player;

	public static bool isFading;
	
	void Awake() {
 		cam = GetComponent<Camera>();
		player = GameObject.Find("Player");
		
		Time.timeScale = 1;
		isFading = false;
    }
	
	// Update is called once per frame
	void Update () {
		cam.orthographicSize = ((Screen.height/Screen.dpi) / 1.5f);

		transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.4f) + new Vector3(0, 0, -10);
	}
}
