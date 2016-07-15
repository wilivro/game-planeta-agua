using UnityEngine;
using System.Collections;

public class RioGarbageBehaviour : MonoBehaviour {

	public static int quantidadeAcertos;
	public static int quantidadeObjetos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, 0.1f);
		if (transform.position.x > 15) {
			Destroy(gameObject);

			if (RioControllerScript.gameOver == false) {
				quantidadeObjetos++;	
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (RioControllerScript.gameOver == false) {
			quantidadeAcertos++;	
		}

		Destroy(gameObject);
	}
}
