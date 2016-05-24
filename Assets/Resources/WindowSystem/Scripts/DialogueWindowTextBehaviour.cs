using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DialogueWindowTextBehaviour : MonoBehaviour {

	// Use this for initialization
	Text text;
	Button bt;

	bool endAnim;
	bool endWrite;
	float writeSpeed;

	void Awake () {
		text = transform.Find("Text").GetComponent<Text>();

		#if !UNITY_STANDALONE
		bt = transform.Find("Next").GetComponent<Button>();
		bt.onClick.AddListener(delegate {NextButton();});
		#endif

		endAnim = false;
		endWrite = false;

		writeSpeed = 0.1f;
	}

	public void Write (string _text) {
		StartCoroutine("WriteText", _text);
	}

	public void AnimationEnd () {
		endAnim = true;
	}

	IEnumerator WriteText(string _text) {
		
		text.text = "";
		while(!endAnim)
			yield return null;

		endWrite = false;
		writeSpeed = 0.1f;

		for(int i=0; i< _text.Length; i++) {
			text.text += _text[i];
			yield return new WaitForSeconds(writeSpeed);
		}

		endWrite = true;

		EventManager.Trigger("DialogueWriteEnd");

		yield return null;
	}

	void NextButton() {
		if(endWrite) {
			Transform chs = transform.parent.Find("Choices");

			if(chs != null) return;

			EventManager.Trigger("DialoguePageComplete");
			writeSpeed = 0.1f;
			return;
		}

		writeSpeed = (writeSpeed == 0.1f) ? 0.01f : 0.1f;
	}

	#if UNITY_STANDALONE
	void Update() {
		if(CrossPlatformInputManager.GetButtonDown("Submit")){
			NextButton();
		}
	}
	#endif
}
