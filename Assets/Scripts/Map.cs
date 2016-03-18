using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	// Use this for initialization
	public static bool created = false;
	void Awake() {
        if (!created) {
     		DontDestroyOnLoad(this.gameObject);
     		created = true;
     	} else {
         	Destroy(this.gameObject);
     	} 
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
