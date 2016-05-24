using UnityEngine;
using System.Collections;

public class IrrigacaoBehaviourScript : MonoBehaviour {

	// Use this for initialization
	public WheelBehaviour[] valve;
	public Vector3 gain;

	Animator anim;
	float force;

	void Start () {
		anim = GetComponent<Animator>();
		//gain = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
		
	}
	
	// Update is called once per frame
	void Update () {
		force = (gain.x) * (valve[0].angle)/360 + (gain.y) * (valve[1].angle)/360 + (gain.z) * (valve[2].angle)/360;
		anim.SetFloat("velocity", force/2.0f + 0.2f);
	}
}
