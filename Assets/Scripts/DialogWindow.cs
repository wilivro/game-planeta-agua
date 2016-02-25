using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogWindow : MonoBehaviour
{

	GameObject win;
	GameObject instatiation;
	Sprite[] face;
	Sprite[] facePlayer;
	string name;

	public DialogWindow(string _name, string _facePath) {
		win = Resources.Load("Prefabs/UI/DialogWindow") as GameObject;
		face = Resources.LoadAll<Sprite>(_facePath);
		facePlayer = Resources.LoadAll<Sprite>("Characters/Leo/face");
		name = _name;
	}

	public void Show(Speech s) {
		instatiation = GameObject.Instantiate(win, new Vector3(67,11,0), Quaternion.identity) as GameObject;

		string formatted = s.text.Replace("{name}", name);
		instatiation.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = formatted;


		instatiation.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = s.isPlayer ? "Leo" : name;
		instatiation.transform.Find("Face").GetComponent<UnityEngine.UI.Image>().sprite = s.isPlayer ? facePlayer[s.expression] : face[s.expression];

		instatiation.transform.position = new Vector3(67,11,0);
	}

	public void Destroy() {
		GameObject.Destroy(instatiation);
	}

}
