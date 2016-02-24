using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Behaviour")]
public class Behaviour {

	[XmlElement("characterName")]
	public string charName;

	[XmlElement("canInteract")]
	public bool canInteract;

	[XmlArray("dialogs")]
	[XmlArrayItem("dialog")]
	public List<Dialog> dialogs;

}

public class Dialog
{
	[XmlElement("dialog")]
	[XmlArrayItem("speech")]
	public List<Speech> speechs;
}

public class Speech
{
	////[XmlElement("speech")]
	public string speech;
	[XmlElementAttribute("player")]
	public bool isPlayer;

}