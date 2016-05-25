using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattlePlayer : MonoBehaviour {

	private BattleStateMachine battleStateMachine;
	private BattleEnemy enemy;
	public Stats_BattlePlayer stats;

	public bool inDefense;
	private int attackCharge = 0;

	public Button[] commandButtons;
	public Button strongAttackButton;

	//Temporary
	public Text currentHPTextBox;
	public Text baseHPTextBox;

	void Awake(){
		battleStateMachine = FindObjectOfType<BattleStateMachine>();
		baseHPTextBox.text = "/" + stats.baseHP;
		stats.currentHP = stats.baseHP;
		currentHPTextBox.text = "" + stats.currentHP;
		enemy = battleStateMachine.enemy;
	}

	public void StartTurn(){
		Debug.Log("Player Turn");
		inDefense = false;

		foreach(Button commandButton in commandButtons){
			if(commandButton != strongAttackButton)
				commandButton.interactable = true;
			else if(commandButton == strongAttackButton){
				if(attackCharge == 2){
					commandButton.interactable = true;
				}
				else if(attackCharge < 2){
					commandButton.interactable = false;
				}
			}
		}
	}

	public void Attack(){
		Debug.Log("Player attacked!");
		enemy.DecreaseHP(battleStateMachine.ComputeDamage(stats.accuracy, stats.criticalChance, stats.critAdditionPercent, stats.attack, enemy.inDefense, enemy.stats.defense));
		attackCharge++;
		if(attackCharge > 2)
			attackCharge = 2;
		EndTurn();
	}

	public void StrongAttack(){
		Debug.Log("Player used strong attack!");
		attackCharge = 0;
		enemy.DecreaseHP(battleStateMachine.ComputeDamage(stats.accuracy, stats.criticalChance, stats.critAdditionPercent, stats.attack * 2, enemy.inDefense, enemy.stats.defense));
		EndTurn();
	}

	public void Defend(){
		inDefense = true;
		EndTurn();
	}

	public void DecreaseHP(int damage){
		stats.currentHP -= damage;
		currentHPTextBox.text = "" + stats.currentHP;
		return;
	}

	public void EndTurn(){
		Debug.Log("Player turn has ended.");

		foreach(Button commandButton in commandButtons){
			commandButton.interactable = false;
		}

		if(enemy.stats.currentHP <= 0){
			enemy.stats.currentHP = 0;
			battleStateMachine.currentState = BattleStateMachine.BattleStates.WIN;
		}
			
		else
			battleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYCHOICE;
	}


}
