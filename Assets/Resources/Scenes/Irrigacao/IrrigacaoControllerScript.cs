using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rpg;

public class IrrigacaoControllerScript : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}

	public void StartGame () {
		Destroy(transform.Find("Help").gameObject);
	}

	public void ExitGame () {
		Log.Register("q03s03", "Irrigação");
		EventManager.Trigger("QuestHelperItemCheck", new object[1]{"q03s03"});
		Application.LoadLevel(0);
	}

	public void EndGame () {
		int acertos = IrrigacaoPaddle.values;
		string msg = "";

		switch (acertos) {
		case 0:
			msg = "Péssimo";
			break;

		case 1:
			msg = "Razoável";
			break;

		case 2:
			msg = "Bom";
			break;

		case 3:
			msg = "Exelente";
			break;
		}

		GameObject end = Resources.Load("Scenes/Irrigacao/Prefabs/End") as GameObject;

		end = Instantiate(end);
		end.transform.SetParent(transform, false);
		end.transform.Find("Text").GetComponent<Text>().text = msg;
		end.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {ExitGame();});
	}
}
