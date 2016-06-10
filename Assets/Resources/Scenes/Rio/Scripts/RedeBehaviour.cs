using UnityEngine;
using System.Collections;

public class RedeBehaviour : MonoBehaviour {

	// Use this for initialization
	public float[] bound;
	public float diff;
	void Start() {
		diff = Mathf.Abs(bound[1] - bound[0]);
	}
	
	// Update is called once per frame
	void Update () {
		print(RioPaddleBehaviour.pos);
		transform.position = new Vector3(transform.position.x, transform.position.y, bound[0] + diff*RioPaddleBehaviour.pos);
	}
}
