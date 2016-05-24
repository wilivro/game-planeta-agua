using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Cryptography;


using Window;
using Rpg.QuestSystem;
using Rpg.DialogueSystem;

namespace Rpg
{

	namespace QuestSystem{}

	public interface IInteractable
	{
		void OnInteractEnter(GameObject from);
		void OnInteractExit(GameObject from);
		void Interact(GameObject to);
		bool AutoInteract();
	}

	public interface IControlable
	{
		void Control();
	}

	public class Character
	{
		public string name;

		public Character(string _name){
			name = _name;
		}
	}

	public class Player : Character
	{
		public static string name;
		public LogData[] logDataArr;
		public static List<Quest> questLog;
		public static Inventory inventory;

		static readonly string PasswordHash = "P@@Sw0rd";
		static readonly string SaltKey = "S@LT&KEY";
		static readonly string VIKey = "@1B2c3D4e5F6g7H8";

		public static Dictionary <string, string> activities = new Dictionary<string, string>();

		private UnityAction<object[]> OnGiveQuest;
		private UnityAction<object[]> OncompleteQuest;
		private UnityAction<object[]> OnGiveItem;
		private UnityAction<object[]> OnGetItem;

		public Player(string _name) :  base(_name){

			name = _name;
			
			questLog = new List<Quest>();
			inventory = new Rpg.Inventory();

			OnGiveQuest = new UnityAction<object[]> (OnGiveQuestCallback);
			EventManager.AddListener("PlayerReciveQuest", OnGiveQuest);

			OnGiveItem = new UnityAction<object[]> (OnGiveItemCallback);
			EventManager.AddListener("PlayerReciveItem", OnGiveItem);

			OncompleteQuest = new UnityAction<object[]> (OncompleteQuestCallback);
			EventManager.AddListener("PlayerCompleteQuest", OncompleteQuest);

			OnGetItem = new UnityAction<object[]> (OnGetItemCallback);
			EventManager.AddListener("PlayerRemoveItem", OnGetItem);


		}

		void OnGiveQuestCallback(object[] param) {
			string[] give = (string[])param[0];

			for(int i = 0; i < give.Length; i++) {
				questLog.Add(new Quest(give[i]));
			}
		}

		void OnGiveItemCallback(object[] param) {
			string[] give = (string[])param[0];

			string[] itemMoney;
			for(int i = 0; i < give.Length; i++) {
				itemMoney = give[i].Split('|');
				if(itemMoney.Length == 1){
					inventory.Add(new Item(itemMoney[0]));
					continue;
				}
				if(itemMoney[1][0] == 'g') {
					int val = Convert.ToInt32(itemMoney[1].Remove(0,1));
					if(inventory.HasEnoughGold(val)){
						inventory.GetGold(val);
						inventory.Add(new Item(itemMoney[0]));
					}
					continue;
				}

			}
		}

		void OnGetItemCallback(object[] param) {
			string[] getItem = (string[])param[0];

			for(int i = 0; i < getItem.Length; i++) {
				inventory.Remove(new Item(getItem[i]));
			}
		}

		void OncompleteQuestCallback(object[] param) {
			int[] quests = (int[])param[0];

			if(quests == null) return;

			for(int i=0; i < quests.Length; i++) {
				Quest quest = questLog.Where(q => q.index == quests[i]).Single();
				if(quest != null) quest.status = Quest.QuestStatus.complete;
			} 

		}

		public void ParseLists(){
			
			int i = 0;
			logDataArr = new LogData[activities.Count];
			foreach(KeyValuePair<string, string> entry in activities)
			{
				logDataArr[i] = new LogData(entry.Key, entry.Value);
			    // do something with entry.Value or entry.Key
			    i++;
			}

			// logDataArr = logData.ToArray();
		}

		public static string Encrypt(string plainText)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
			var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
			
			byte[] cipherTextBytes;

			using (var memoryStream = new MemoryStream())
			{
				using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
					cryptoStream.FlushFinalBlock();
					cipherTextBytes = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return Convert.ToBase64String(cipherTextBytes);
		}

		public static string Decrypt(string encryptedText)
		{
			byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
			byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

			var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
			var memoryStream = new MemoryStream(cipherTextBytes);
			var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
		}


		public bool Save(){

			ParseLists();

			string dir = "SaveData";

			if(!Directory.Exists(dir)) {
	           Directory.CreateDirectory(dir);
	        }

			string path = null;
			path = "SaveData/save_"+name+".json";
			#if UNITY_ANDROID
				path = Application.persistentDataPath+"/save_" + name +".json";
			#endif
			#if UNITY_EDITOR
				path = "Assets/Resources/Player/SaveData/save_"+name+".json";
			#endif

			string str = JsonUtility.ToJson(this);

			//str = Encrypt(str);

			using (FileStream fs = new FileStream(path, FileMode.Create)){
				using (StreamWriter writer = new StreamWriter(fs)){
			    	writer.Write(str);
			 	}
			}

			#if UNITY_EDITOR
				UnityEditor.AssetDatabase.Refresh ();
			#endif

			return File.Exists(path);
		}

		public bool Load() {
			// PlayerLoadObject plo = new PlayerLoadObject();
			Log.autoSave = false;
			questLog = new List<Quest>();
			inventory = new Rpg.Inventory();

			string path = null;
			path = "SaveData/save_"+name+".json";
			#if UNITY_ANDROID
				path = Application.persistentDataPath+"/save_" + name +".json";
			#endif
			#if UNITY_EDITOR
				path = "Player/SaveData/save_"+name;
				TextAsset questFile = Resources.Load(path) as TextAsset;
				string json = questFile.text;
				//json = Decrypt(json);
				JsonUtility.FromJsonOverwrite(json, this);
			#endif

			string questName;
			Quest.QuestStatus questStatus;

			string itemName;
			int itemQtd;

			string questPattern = @"^q[0-9]+$";
			string itemPattern = @"^i[0-9]+$";
			Regex qrgx = new Regex(questPattern, RegexOptions.IgnoreCase);
			Regex irgx = new Regex(itemPattern, RegexOptions.IgnoreCase);

			for(int i=0; i < logDataArr.Length; i++) {
				if(qrgx.IsMatch(logDataArr[i].key)) { //is a Quest Entry

					questName = logDataArr[i].key;
					questStatus = (Quest.QuestStatus)Enum.Parse(typeof(Quest.QuestStatus), logDataArr[i].message);
					Quest[] qst = questLog.Where(q => q.id == questName).ToArray();
					if(qst.Length > 0) {
						if(qst[0].status != questStatus){
							if (questStatus == Quest.QuestStatus.archived) {
								qst[0].Archive();
							} else { 
								qst[0].status = questStatus;
							}
						}
					} else {
						Quest q = new Quest(questName,questStatus);
						questLog.Add(q);
						if (questStatus == Quest.QuestStatus.archived) {
							q.Archive();
						}
					}
					continue;
				}
				if(irgx.IsMatch(logDataArr[i].key)) { // is a Item Entry

					itemName = logDataArr[i].key;
					itemQtd = System.Convert.ToInt32(logDataArr[i].message);

					if(itemQtd > 0 ) inventory.Add(new Item(itemName),itemQtd);
					continue;
				}

				Log.Register(logDataArr[i].key, logDataArr[i].message);
			}
			Log.autoSave = true;
			GameController.player.Save();
			logDataArr = null;

			return true;

			//questLog.Add(new Quest(1));

		}
	}


	public class Npc : Character
	{
		public string name;
		public Speech[] dummyDialogue;
		public int[] questList;
		public Dialogue[]  dialogue;
		public DialogueControl dialogueControl;

		public Npc(string _name) : base(_name) {
			string path = "Npcs/source/"+_name;

			TextAsset questFile = Resources.Load(path) as TextAsset;
			JsonUtility.FromJsonOverwrite(questFile.text, this);

			dialogueControl = new DialogueControl(dialogue, dummyDialogue, name);
		}
	}

	public class Warp
	{
		public string[] requirements;
		public string message;

		public Span span;

		public Warp() {
			
		}

		public Warp(string id) {
			DatabaseWarp dbw = ((DatabaseWarp)GameController.database.Find(id));
			string path = dbw.GetPath();

			TextAsset questFile = Resources.Load(path) as TextAsset;
			JsonUtility.FromJsonOverwrite(questFile.text, this);

			Transform gc = UnityEngine.GameObject.Find("GameController").transform;
			span = new Span(gc);

		}

		public bool Ready() {
			return Log.HasKey(requirements);
		}
	}

	[Serializable]
	public class LogData
	{
		public string key;
		public string message;

		public LogData(string _key, string _value) {
			key = _key;
			message = _value;
		}
	}

	public class Log
	{
		
		public static bool autoSave = true;
		public static void Register(string _key, string _value) {
			try {
				EventManager.Trigger("QuestHelperItemCheck", new object[1] {_key});
				Player.activities.Add(_key, _value);
			} catch {
				Player.activities[_key] = _value;
			}
			
			if(autoSave) {
				GameController.player.Save();
				Debug.Log("Logging "+_key+" , "+_value);
			}
		}

		public static bool HasKey(string _key) {
			string _value = null;
			if(Player.activities.TryGetValue(_key, out _value)) {
				return true;
			}

			return false;
		}

		public static string GetValue(string _key) {
			string _value = null;
			if(Player.activities.TryGetValue(_key, out _value)) {
				return _value;
			}

			return null;
		}

		public static bool HasKey(string[] arr) {
			foreach(string s in arr) {
				if(!HasKey(s)) return false;
			}
			return true;
		}

		public static void Register(LogData[] registerLog){
			foreach(LogData l in registerLog) {
				Register(l.key, l.message);
			}
		}

	}

	[Serializable]
	public class DatabaseDictionary : IDatabaseItem
	{
		public string key;
		public string label;

		public string GetKey() {
			return key;
		}

		public string GetLabel() {
			return label;
		}
	}

	[Serializable]
	public class DatabaseItem : IDatabaseItem
	{
		public string key;
		public string path;
		public string filename;

		public string GetKey() {
			return key;
		}

		public string GetFullPath(){
			return path+filename;
		}

		public string GetPath(){
			return path;
		}
	}

	[Serializable]
	public class DatabaseQuest : DatabaseItem
	{
	}

	[Serializable]
	public class DatabaseImage : IDatabaseItem
	{
		public string key;
		public string source;
		public int index;

		public string GetKey() {
			return key;
		}

		public string GetSource(){
			return source;
		}

		public int GetIndex(){
			return index;
		}
	}

	[Serializable]
	public class DatabaseWarp : IDatabaseItem
	{
		public string key;
		public string path;

		public string GetKey() {
			return key;
		}

		public string GetPath() {
			return path;
		}
	}

	public interface IDatabaseItem
	{		
		string GetKey();
	}

	public class DatabaseJson
	{
		public DatabaseDictionary[] data;
		public DatabaseQuest[] quests;
		public DatabaseImage[] images;
		public DatabaseItem[] items;
		public DatabaseWarp[] warps;

		public DatabaseJson(string _path) {
			TextAsset file = Resources.Load(_path) as TextAsset;
			JsonUtility.FromJsonOverwrite(file.text, this);
		}
	}

	public class Database
	{
		public DatabaseJson json;

		public List<IDatabaseItem> i;

		public Database(string _path) {

			json = new DatabaseJson(_path);

			i = new List<IDatabaseItem>();

			FieldInfo[] fields = json.GetType().GetFields();

			foreach (FieldInfo fieldInfo in fields)
			{
			    IDatabaseItem[] property = (IDatabaseItem[])(typeof(DatabaseJson).GetField(fieldInfo.Name).GetValue(json));
			    List<IDatabaseItem> list = property.ToList();

			    foreach(IDatabaseItem it in list) {
			    	i.Add(it);
			    }
			}
			
		}

		public IDatabaseItem Find(string _key) {
			try {
				return i.Where(d => d.GetKey() == _key).Single();
			} catch {
				return null;
			}
		}
		
	}
	
}


