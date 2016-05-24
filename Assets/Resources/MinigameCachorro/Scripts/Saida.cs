using UnityEngine;
using System.Collections;

public class Saida : WarpBehaviour {
	public void Start () {
		base.Start();
	}

	public override bool Teleporter() {
		print("Voltar para cidade");
		return true;
	}

	public override void NotReady() {
		print("Mostrar video");
	}
}
