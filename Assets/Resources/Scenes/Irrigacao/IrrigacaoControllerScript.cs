using UnityEngine;
using System.Collections;

public class IrrigacaoControllerScript : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}

	public void StartGame () {
		Destroy(transform.Find("Help").gameObject);
	}
}
