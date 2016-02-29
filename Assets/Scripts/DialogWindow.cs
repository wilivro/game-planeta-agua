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

	private string formatText(string input) {
		string formatted = input.Replace("{name}", name);
		formatted = formatted.Replace("\t", "");
		formatted = formatted.Trim();

		return formatted;
	}

	public bool Show(Character c) {
		Speech s = c.GetDialog();

		instatiation = GameObject.Instantiate(win, new Vector3(67,11,0), Quaternion.identity) as GameObject;

		string formatted = formatText(s.text);
		Transform ctx = instatiation.transform.Find("ContainerText");
		Transform ctxName = ctx.Find("NamePanel");
		ctx.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = formatted;

		ctxName.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = s.isPlayer ? "Leo" : name;
		ctx.transform.Find("Face").GetComponent<UnityEngine.UI.Image>().sprite = s.isPlayer ? facePlayer[s.expression] : face[s.expression];

		Transform ctxChoices = ctx.transform.Find("Choices");

		if(s.choices.Count == 0){
			GameObject.Destroy(ctxChoices.gameObject);

			return false;
		}

		RectTransform rtCtx = ctxChoices.GetComponent (typeof (RectTransform)) as RectTransform;
		rtCtx.sizeDelta = new Vector2(rtCtx.sizeDelta.x, s.choices.Count*30 +20);

		RectTransform rtOriginal;
		RectTransform rt;

		Transform button = ctxChoices.Find("Button");
		button.Find("Text").GetComponent<UnityEngine.UI.Text>().text = s.choices[0].text;
		rtOriginal = button.GetComponent (typeof (RectTransform)) as RectTransform;

		UnityEngine.UI.Button btn = button.GetComponent<UnityEngine.UI.Button>();

		btn.onClick.AddListener(delegate { OnChoice(s.choices[0], c); });

		Transform newButton;
		for(int i = 1; i < s.choices.Count; i++) {
			newButton = Instantiate(button);

			newButton.SetParent(ctxChoices);

			newButton.Find("Text").GetComponent<UnityEngine.UI.Text>().text = s.choices[i].text;
			rt = newButton.GetComponent (typeof (RectTransform)) as RectTransform;

			rt.sizeDelta = new Vector2(246, 30);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, ((-30*i) - 10)*(-1), 30);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 10, 0);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10, -20);

			rt.localScale = new Vector3(1,1,0);
			rt.anchorMin = new Vector2(0,1);
			rt.anchorMax = new Vector2(1,1);
			rt.pivot = new Vector2(0,1);

			Choice arg = s.choices[i];

			newButton.gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnChoice(arg, c); });

		}

		return true;
	}

	public void Destroy() {
		GameObject.Destroy(instatiation);
	}

	public void OnChoice(Choice ch, Character c) {
		print ("oi");
		int score = PlayerPrefs.GetInt("Score");
		PlayerPrefs.SetInt("Score", score+ch.addScore);
		c.actualSpeech = ch.gotoSpeech;
		Destroy();
		Show(c);
	}

}
