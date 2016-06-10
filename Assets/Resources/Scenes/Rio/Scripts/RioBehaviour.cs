using UnityEngine;
using System.Collections;

public class RioBehaviour : MonoBehaviour {

	// Use this for initialization
	public GameObject[] garbage;
	void Start () {
		StartCoroutine("Runtime");
	}
	
	IEnumerator Runtime() {
		Vector3 pos;
		GameObject g;
		while(true) {
			for(int i =0; i < Random.Range(4, 8); i++) {

				g = Instantiate(garbage[Random.Range(0, garbage.Length)]);
				pos = new Vector3(-50f, g.transform.position.y, Random.Range(-5.0f, 10.0f));
				g.transform.position = pos;

				yield return new WaitForSeconds(Random.Range(1f,2f));
			}
			
			yield return new WaitForSeconds(5);
		}
	}
}
