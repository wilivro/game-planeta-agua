using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rpg;

public class LoucaController : MonoBehaviour {

	// Use this for initialization
	public GameObject[] obj;
	RectTransform water;

	int qtdObjToWash = 10;
	float waterRate = 1;

	bool end;
	bool win;

	void Start () {
		obj = new GameObject[2];
		obj[0] = Resources.Load("Scenes/LavarLouca/Prefabs/prato") as GameObject;
		obj[1] = Resources.Load("Scenes/LavarLouca/Prefabs/frigideira") as GameObject;

		water = transform.Find("Water").gameObject.GetComponent<RectTransform>();
		
		StartCoroutine("Walter");

		win = false;
		end = false;
	}

	IEnumerator Walter() {
		float initialHeight = water.sizeDelta.y;
		while(true) {
			yield return new WaitForSeconds(0.1f);

			if(waterRate <= 0) {
				end = true;
				win = false;
				break;
			}

			water.sizeDelta = new Vector2(32, Mathf.Lerp(water.sizeDelta.y, initialHeight*waterRate, 10*Time.deltaTime));
		}

		yield return null;
		
	}
	
	IEnumerator InvokeObj() {
		float jitter;
		GameObject n;
		while(!end) {
			jitter =  Random.Range(0.2f, 1.2f);
			yield return new WaitForSeconds(jitter);
			n = Instantiate(obj[Random.Range(0,2)]);
			n.transform.SetParent(transform, false);
			n.transform.SetSiblingIndex(2);
		}

		EndGame();
	}

	public void Wash(bool correct) {
		if(correct) {
			waterRate -= 0.05f;
			qtdObjToWash--;
		} else {
			waterRate -= 0.1f;
		}

		if(waterRate <= 0) waterRate = 0;
		if(qtdObjToWash == 0) {
			end = true;
			if(waterRate >= 0) win = true;
			else win = false;
		}
	}

	public void StartGame() {
		GameObject panel = transform.Find("Help").gameObject;
		Destroy(panel);
		StartCoroutine("InvokeObj");
	}

	public void ExitGame() {
		Log.Register("q02s01", "louça lavada");
		EventManager.Trigger("QuestHelperItemCheck", new object[1]{"q02s01"});
		Application.LoadLevel(0);
	}

	public void EndGame() {

		string msg = "Parabens";

		if(!win) msg = "Se fudeu";

		GameObject end = Resources.Load("Scenes/LavarLouca/Prefabs/End") as GameObject;

		end = Instantiate(end);
		end.transform.SetParent(transform, false);
		end.transform.Find("Text").GetComponent<Text>().text = msg;
		end.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {ExitGame();});
	}
}
