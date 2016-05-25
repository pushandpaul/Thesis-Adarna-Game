using UnityEngine;
using System.Collections;

[System.Serializable]

public class Stats_BattleEnemy{
	public string name;

	public int currentHP;
	public int baseHP;

	public float attack;
	public float defense;

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

	[System.Serializable]
	public class CommandAndChance{
		public Moves move;
		public int chance;
		public AnimationClip[] animationClips;

	}

	public CommandAndChance[] commandsAndChance;
}
