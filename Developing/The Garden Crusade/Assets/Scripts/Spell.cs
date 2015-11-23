using UnityEngine;
using System.Collections;

public class Spell {

	private string name;
	public string Name {
		get { return name;}
	}

	private float castTime;
	public float CastTime {
		get { return castTime;}
	}

	private Color32 spellColor;
	public Color32 Spellcolor {
		get { return spellColor;}
	}

	public Spell(string name, float castTime, Color32 spellColor) {

		this.name = name;
		this.castTime = castTime;
		this.spellColor = spellColor; 
	}
}
  