using UnityEngine;
using System.Collections;

public class IrrigacaoBehaviourScript : MonoBehaviour {

	// Use this for initialization
	

	Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
		//gain = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
		
	}
	
	// Update is called once per frame
	void Update () {

		float ratio = (3 - IrrigacaoPaddle.values)/5f + 0.2f;

		Vector3 newSize = new Vector3(ratio, ratio, ratio);
		transform.localScale = Vector3.Lerp(transform.localScale, newSize, 0.8f);
		anim.SetFloat("velocity", ratio);
	}
}
