using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogWindow : MonoBehaviour
{

	GameObject win;
	GameObject winLong;
	GameObject instatiation;
	Sprite[] face;
	Sprite[] facePlayer;
	Canvas joyCanvas;
	public string name;

	public void Config(string _name, string _facePath) {
		win = Resources.Load("Prefabs/UI/DialogWindow") as GameObject;
		winLong = Resources.Load("Prefabs/UI/DialogWindowLong") as GameObject;
		face = Resources.LoadAll<Sprite>(_facePath);
		facePlayer = Resources.LoadAll<Sprite>("Characters/Leo/face");
		joyCanvas = GameObject.Find("Joystick").GetComponent<Canvas>();
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

		joyCanvas.transform.Find("MobileJoystick").GetComponent<Image>().enabled = false;

		instatiation = GameObject.Instantiate(s.isLong ? winLong : win) as GameObject;

		string formatted = formatText(s.text);
		Transform ctx = instatiation.transform.Find("ContainerText");
		Transform ctxName = ctx.Find("NamePanel");

		object[] parms = new object[3]{formatted, ctx.transform.Find("Text").GetComponent<UnityEngine.UI.Text>(), c};
		StartCoroutine(Write(parms));

		ctxName.transform.Find("Name").GetComponent<Text>().text = s.isPlayer ? "Leo" : name;

		if(s.isLong) return false;

		ctx.transform.Find("Face").GetComponent<Image>().sprite = s.isPlayer ? facePlayer[s.expression] : face[s.expression];

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
		button.Find("Text").GetComponent<Text>().text = s.choices[0].text;
		rtOriginal = button.GetComponent (typeof (RectTransform)) as RectTransform;

		Button btn = button.GetComponent<Button>();

		btn.onClick.AddListener(delegate { OnChoice(s.choices[0], c); });

		Transform newButton;
		for(int i = 1; i < s.choices.Count; i++) {
			newButton = Instantiate(button);

			newButton.SetParent(ctxChoices);

			newButton.Find("Text").GetComponent<Text>().text = s.choices[i].text;
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

			newButton.gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnChoice(arg, c); });

		}

		return true;
	}

	public IEnumerator Write(object[] parms) {

		string text = (string)parms[0];
		Text ctx = (Text)parms[1];
		Character c = (Character)parms[2];


		for(int i = 0; i < text.Length; i++){
			if(!ctx) yield return null;
			yield return new WaitForSeconds(0.05f);
			ctx.text += text[i];
		}

		c.myBehaviour.canInteract = true;
	}

	public void Destroy() {
		joyCanvas.transform.Find("MobileJoystick").GetComponent<Image>().enabled = true;
		GameObject.Destroy(instatiation);
	}

	public void OnChoice(Choice ch, Character c) {
		int score = PlayerPrefs.GetInt("Score");
		PlayerPrefs.SetInt("Score", score+ch.addScore);
		c.actualSpeech = ch.gotoSpeech;
		Destroy();
		Show(c);
	}

}
