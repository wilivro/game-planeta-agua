using UnityEngine;
using System.Collections;

public class PlayerSortInLayer : MonoBehaviour {
	GameObject player;
	public int orderInLayer;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		player.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
	}
}
