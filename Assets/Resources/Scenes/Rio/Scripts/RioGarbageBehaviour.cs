using UnityEngine;
using System.Collections;

public class RioGarbageBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, 0.1f);
		if(transform.position.x > 50) Destroy(gameObject);
	}
}
