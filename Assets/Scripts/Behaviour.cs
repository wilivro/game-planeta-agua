﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Behaviour")]
public class Behaviour {

	[XmlElement("characterName")]
	public string charName;

	[XmlElement("interacatle")]
	public bool canInteract;

	[XmlElement("quest")]
	public int quest;

	[XmlElement("subQuest")]
	public int subQuest;

	[XmlElement("dismissible")]
	public bool dismissible;

	[XmlArray("dialogs")]
	[XmlArrayItem("dialog")]
	public List<Dialog> dialogs;

}

public class Dialog
{
	[XmlElement("speech")]
	public List<Speech> speechs;
}

public class Speech
{
	[XmlElement("text")]
	public string text;

	[XmlAttribute("player")]
	public bool isPlayer;

	[XmlAttribute("expression")]
	public int expression;

	[XmlAttribute("long")]
	public bool isLong;

	[XmlElement("choice")]
	public List<Choice> choices;

}

public class Choice
{
	[XmlTextAttribute()]
	public string text;

	[XmlAttribute("correct")]
	public bool correct;

	[XmlAttribute("addScore")]
	public int addScore;

	[XmlAttribute("goto")]
	public int gotoSpeech;
}