using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestHelper : MonoBehaviour {

	// Use this for initialization
	Transform root;
	void Start () {
		root = transform;
	}

	public void UpdateQuestHelper(){
		string questLog = PlayerPrefs.GetString("QuestLog");

		string[] questLoadArr = questLog.Split('|');
		Transform mock = null;

		foreach (Transform t in root) {
		    if(t.gameObject.tag == "Mock") {
		    	mock = t;
		    	continue;
		    };
		    Destroy(t.gameObject);
	 	}

	 	int total = 0;

		for(int itr = 0; itr < questLoadArr.Length -1; itr++) {
			bool contains = Player.inventory.content.Contains(new Item(questLoadArr[itr]));

			if(contains) total++;

			Transform ist = Instantiate(mock);
			ist.SetParent(root);
			ist.gameObject.tag = "Untagged";
			ist.gameObject.active = true;

			ist.GetComponent<Toggle>().isOn = contains;

			RectTransform rt = ist.GetComponent<RectTransform>();

			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
			rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, ((20f*itr)), 0f);
			rt.sizeDelta = new Vector2(335f, 20f);

			rt.localScale = new Vector3(1,1,1);
			rt.anchorMin = new Vector2(1f,1f);
			rt.anchorMax = new Vector2(1f,1f);
			rt.pivot = new Vector2(1f,1f);

			ist.Find("Label").GetComponent<Text>().text = questLoadArr[itr];

		}

		if(questLoadArr.Length > 1 && total == questLoadArr.Length -1) {
			int subQuest = PlayerPrefs.GetInt("SubQuest");
			PlayerPrefs.SetInt("SubQuest", subQuest+1);
			PlayerPrefs.SetString("QuestLog", "");
			Player.hasQuestLog = false;
			PlayerPrefs.Save();
		}

	}
	
	// Update is called once per frame
	void Update () {
		UpdateQuestHelper();
	}
}
