using UnityEngine;
using System.Collections;

public class BattleEnemy : MonoBehaviour {
	
	private BattleStateMachine battleStateMachine;
	public Stats_BattleEnemy stats;
	private BattlePlayer player;

	[System.NonSerialized]
	public Animator anim;

	public bool inDefense;

	void Start(){
		battleStateMachine = FindObjectOfType<BattleStateMachine>();

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

		else if(chosenCommand.move == Stats_BattleEnemy.Moves.StrongAttack){
			StrongAttack ();
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

	void StrongAttack(){
		Debug.Log ("Enemy used strong attack!");
		StartCoroutine(StartAttackAnim(Stats_BattleEnemy.Moves.StrongAttack, getRandomAnimClip(Stats_BattleEnemy.Moves.StrongAttack)));
	}

	void Defend(){
		inDefense = true;
		Debug.Log("Enemy defended!");
		EndTurn();
	}

	void Heal(){
		Debug.Log("Enemy healed!");
		StartCoroutine(StartSpecialMoveAnim(Stats_BattleEnemy.Moves.Heal, getRandomAnimClip(Stats_BattleEnemy.Moves.Heal)));
	}

	public void IncreaseHP(int addHP){
		stats.currentHP += addHP;
		if(stats.currentHP > stats.baseHP){
			battleStateMachine.setFloatingText("+" + addHP.ToString());
			stats.currentHP = stats.baseHP;
		}
		return;
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
		battleStateMachine.commandPrompt.canvasGroup.alpha = 0f;
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
			battleStateMachine.setCommandPrompt("Umatake ang " + stats.name + "!");
			attackPower = stats.attack;
			animationName = moveAnimation.name;
		}

		else  if(attackMove == Stats_BattleEnemy.Moves.StrongAttack){
			battleStateMachine.setCommandPrompt("Ginamit ng " + stats.name + "ang Malakas na Pag-atake!");
			attackPower = stats.attack * 2;
			animationName = moveAnimation.name;
		}

		anim.Play(animationName);
		yield return new WaitForSeconds(moveAnimation.length);
		player.DecreaseHP(battleStateMachine.ComputeDamage(stats.accuracy, stats.criticalChance, stats.critAdditionPercent, attackPower, player.inDefense, player.stats.defense));
		EndTurn();
	}

	IEnumerator StartSpecialMoveAnim(Stats_BattleEnemy.Moves move, AnimationClip moveAnimation){
		string animationName = "";
		bool increaseHP = false;

		yield return new WaitForSeconds(1);

		if(move == Stats_BattleEnemy.Moves.Heal){
			battleStateMachine.setCommandPrompt("Nagapgaling ang " + stats.name);
			animationName = moveAnimation.name;
			increaseHP = true;
		}

		anim.Play (animationName);
		yield return new WaitForSeconds(moveAnimation.length);

		if(increaseHP){
			IncreaseHP((int)stats.special);
		}

		EndTurn ();
	}

}
