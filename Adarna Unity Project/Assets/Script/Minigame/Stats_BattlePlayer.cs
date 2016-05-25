using UnityEngine;
using System.Collections;

[System.Serializable]

public class Stats_BattlePlayer{
	public int currentHP;
	public int baseHP;

	public float attack;
	public float defense;

	[Tooltip("Chance that the player will hit the enemy (in %).")]
	public int accuracy;

	[Tooltip("Chance that the player will land a critical hit (in %).")]
	public int criticalChance;
	[Tooltip("Percentage added if the hit is critical (in %).")]
	public float critAdditionPercent;
}
