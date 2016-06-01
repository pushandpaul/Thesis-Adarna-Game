using UnityEngine;
using System.Collections;

[System.Serializable]

public class Stats_BattleEnemy{
	public string name;

	public int currentHP;
	public int baseHP;

	public float attack;
	public float defense;

	[Tooltip("Special power, used in magic or spells (if exists).")]
	public float special;

	[Tooltip("Chance that the enemy will hit the player (in %).")]
	public int accuracy; 

	[Tooltip("Chance that the enemy will land a critical hit (in %).")]
	public int criticalChance;
	[Tooltip("Percentage added if the hit is critical (in %).")]
	public float critAdditionPercent;

	public enum Moves{
		Attack,
		StrongAttack,
		Defend,
		Heal,
	}

	public enum Size{
		Small,
		Medium,
		Large,
		ExtraLarge,
	}

	[System.Serializable]
	public class CommandAndChance{
		public Moves move;
		public int chance;
		public AnimationClip[] animationClips;

	}

	public Size size;
	public CommandAndChance[] commandsAndChance;

}
