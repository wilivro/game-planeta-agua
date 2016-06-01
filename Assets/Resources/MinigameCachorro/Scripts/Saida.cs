using UnityEngine;
using System.Collections;

public class Saida : WarpBehaviour {
	public Vector3 warpLocation = new Vector3(15.24f, -10.85f, -10.53f);

	public void Start () {
		base.Start();
	}

	public override bool Teleporter() {
		print("Voltar para cidade");
		GameObject.Find("Player").transform.position = warpLocation;
		Application.LoadLevel(2);
		return true;
	}

	public override void NotReady() {
		print("Mostrar video");
	}
}
