using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Rpg;


public class RioControllerScript : MonoBehaviour {

	public static bool gameOver = true;

	// Use this for initialization
	void Start () {

	}

	public void StartGame () {
		gameOver = false;
		Destroy(transform.Find("Help").gameObject);
	}

	public void EndGame () {
		string msg = "";

		if (RioGarbageBehaviour.quantidadeObjetos < RioGarbageBehaviour.quantidadeObjetos) {
			msg = "Ruim";
		} else if (RioGarbageBehaviour.quantidadeObjetos == RioGarbageBehaviour.quantidadeObjetos) {
			msg = "Bom";
		} else if (RioGarbageBehaviour.quantidadeObjetos > RioGarbageBehaviour.quantidadeObjetos) {
			msg = "Ótimo";
		}

		GameObject end = Resources.Load("Scenes/Irrigacao/Prefabs/End") as GameObject;

		end = Instantiate(end);
		end.transform.SetParent(transform, false);
		end.transform.Find("Text").GetComponent<Text>().text = msg;
		end.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {ExitGame();});
	}

	void ExitGame () {
		Log.Register("q03s03", "Rio");
		EventManager.Trigger("QuestHelperItemCheck", new object[1]{"q03s03"});
		Application.LoadLevel(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
