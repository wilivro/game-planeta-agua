using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogWindow : MonoBehaviour
{

	GameObject win;
	GameObject instatiation;
	Sprite[] face;
	string name;

	public DialogWindow(string _name, string _facePath) {
		win = Resources.Load("Prefabs/UI/DialogWindow") as GameObject;
		face = Resources.LoadAll<Sprite>(_facePath);
		name = _name;
		print(face);
	}

	public void Show(Speech s) {
		print(s.isPlayer);
		instatiation = GameObject.Instantiate(win, new Vector3(67,11,0), Quaternion.identity) as GameObject;
		instatiation.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = s.speech;
		instatiation.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = s.isPlayer ? name : "Leo";
		instatiation.transform.Find("Face").GetComponent<UnityEngine.UI.Image>().sprite = face[0];

		instatiation.transform.position = new Vector3(67,11,0);
	}

	public void Destroy() {
		GameObject.Destroy(instatiation);
	}

}
