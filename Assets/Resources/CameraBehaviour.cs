using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	private Camera cam;
	private GameObject player;

	public static bool created = false;

	void Awake() {
        if (!created) {
         	// this is the first instance - make it persist
     		DontDestroyOnLoad(this.gameObject);
     		created = true;
     		cam = GetComponent<Camera>();
			player = GameObject.Find("Player");

     	} else {
         	// this must be a duplicate from a scene reload - DESTROY!
         	Destroy(this.gameObject);
     	} 
    }
	
	// Update is called once per frame
	void Update () {
		cam.orthographicSize = ((Screen.height/Screen.dpi) / 2f);

		transform.position = Vector3.Lerp(transform.position, player.transform.position, 1f) + new Vector3(0, 0, -10);
	}
}
