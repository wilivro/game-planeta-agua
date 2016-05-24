using UnityEngine;
using System.Collections;

namespace Scene
{

	
	public class Language
	{
		public string journal;
		public string inventory;
		public string gold;

		string lang;

		public Language(string lang){
			SetLanguage(lang);
		}

		public void SetLanguage(string lang){
			string path = "Language/"+lang;
			TextAsset questFile = Resources.Load(path) as TextAsset;
	 		JsonUtility.FromJsonOverwrite(questFile.text, this);
		}
	}

}
