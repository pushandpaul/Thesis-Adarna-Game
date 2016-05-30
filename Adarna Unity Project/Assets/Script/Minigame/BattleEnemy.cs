using UnityEngine;
using System.Collections;

public class BattleEnemy : MonoBehaviour {
	
	private BattleStateMachine battleStateMachine;
	public Stats_BattleEnemy stats;
	private BattlePlayer player;

	private Animator anim;

	public bool inDefense;

	void Start(){
		battleStateMachine = FindObjectOfType<BattleStateMachine>();
		anim = GetComponentInChildren<Animator>();

		if(battleStateMachine != null){
			stats.currentHP = stats.baseHP;
			player = battleStateMachine.player;
		}
	}

	public void StartTurn(){
		Debug.Log("Enemy Turn");
		inDefense = false;
		ExcecuteAI();
	}

	void ExcecuteAI(){
		Stats_BattleEnemy.CommandAndChance chosenCommand = new Stats_BattleEnemy.CommandAndChance();
		float movesTotalChance = 0;
		float randomPoint = 0;

		foreach(Stats_BattleEnemy.CommandAndChance command in stats.commandsAndChance){
			movesTotalChance += command.chance;
		}

		randomPoint = Random.value * movesTotalChance;
		//Debug.Log("Enemy move total chance is: " + movesTotalChance);
		//Debug.Log("Random enemy value is: " + randomPoint);

		foreach(Stats_BattleEnemy.CommandAndChance command in stats.commandsAndChance){
			if(randomPoint < command.chance){
				chosenCommand = command;
				break;
			}
			else
				randomPoint -= command.chance;
		}

		if(chosenCommand.move == Stats_BattleEnemy.Moves.Attack){
			Attack();
		}
		else if(chosenCommand.move == Stats_BattleEnemy.Moves.Defend){
			Defend();
		}
		else if(chosenCommand.move == Stats_BattleEnemy.Moves.Heal){
			Heal();
		}
	}

	void Attack(){
		Debug.Log("Enemy attacked!");
		StartCoroutine(StartAttackAnim(Stats_BattleEnemy.Moves.Attack, getRandomAnimClip(Stats_BattleEnemy.Moves.Attack)));
	}

	void Defend(){
		inDefense = true;
		Debug.Log("Enemy defended!");
		EndTurn();
	}

	void Heal(){
		Debug.Log("Enemy healed!");
		EndTurn();
	}

	public void DecreaseHP(int damage){
		stats.currentHP -= damage;
		if(stats.currentHP < 0){
			stats.currentHP = 0;
		}
		return;
	}

	void EndTurn(){
		Debug.Log("Enemy turn has ended.");
		if(player.stats.currentHP == 0){
			battleStateMachine.currentState = BattleStateMachine.BattleStates.LOSE;
		}
			
		else
			battleStateMachine.currentState = BattleStateMachine.BattleStates.PLAYERCHOICE;
	}

	AnimationClip getRandomAnimClip(Stats_BattleEnemy.Moves move){
		AnimationClip randomizedMoveClip = new AnimationClip();
		int randomizedAnimIndex = 0;

		foreach(Stats_BattleEnemy.CommandAndChance commandAndChance in stats.commandsAndChance){
			if(move == commandAndChance.move){
				randomizedAnimIndex = Random.Range(0, commandAndChance.animationClips.Length);
				randomizedMoveClip = commandAndChance.animationClips[randomizedAnimIndex];
				break;
			}
		}

		return randomizedMoveClip;
	}

	IEnumerator StartAttackAnim(Stats_BattleEnemy.Moves attackMove, AnimationClip moveAnimation){
		string animationName = "";
		float attackPower = 0f;
		int computedDamage = 0;

		yield return new WaitForSeconds(1);

		if(attackMove == Stats_BattleEnemy.Moves.Attack){
			attackPower = stats.attack;
			animationName = moveAnimation.name;
		}

		anim.Play(animationName);
		yield return new WaitForSeconds(moveAnimation.length);
		player.DecreaseHP(battleStateMachine.ComputeDamage(stats.accuracy, stats.criticalChance, stats.critAdditionPercent, attackPower, player.inDefense, player.stats.defense));
		EndTurn();
	}

}
