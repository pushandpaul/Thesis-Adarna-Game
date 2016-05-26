using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleStateMachine : MonoBehaviour {

	public enum BattleStates{
		START,
		PLAYERCHOICE,
		ENEMYCHOICE,
		LOSE,
		WIN
	}

	public BattleStates currentState;
	public BattleEnemy enemy;
	public BattlePlayer player;

	private bool executedPlayerTurn;
	private bool executedEnemyTurn;
	private bool executedWin;
	private bool executedLose;

	void Start () {
		currentState = BattleStates.START;
	}
	

	void Update () {
		switch(currentState){

		case (BattleStates.START):
			Debug.Log("Battle Started");
			currentState = BattleStates.PLAYERCHOICE;
			break;
		case (BattleStates.PLAYERCHOICE):
			if(!executedPlayerTurn){
				player.StartTurn();
				executedPlayerTurn = true;
				executedEnemyTurn = false;
			}
			break;
		case (BattleStates.ENEMYCHOICE):
			if(!executedEnemyTurn){
				enemy.StartTurn();
				executedEnemyTurn = true;
				executedPlayerTurn = false;
			}
			break;
		case (BattleStates.LOSE):
			if(!executedLose)
				Lose();
			break;
		case (BattleStates.WIN):
			if(!executedWin)
				Win();
			break;
		}
	}

	public int ComputeDamage(int accuracy, int criticalChance, float critAdditionPercent, float attack, bool targetInDefense, float targetDefense){
		float hitRandomPoint = Random.value * 100;
		float criticalRandomPoint = Random.value * 100;

		int damage = 0;
		float damageFloat = 0;
		string currentTurn = "";
		string oppositeTurn = "";

		if(currentState == BattleStates.PLAYERCHOICE){
			currentTurn = "Player";
			oppositeTurn = "Enemy";
		}
		else if(currentState ==  BattleStates.ENEMYCHOICE){
			currentTurn = "Enemy";
			oppositeTurn = "Player";
		}

		if(hitRandomPoint < accuracy){
			Debug.Log(currentTurn + " hit landed!");
			if(criticalRandomPoint < criticalChance){
				Debug.Log("It was a critical hit!");
				damageFloat = attack + (attack * (0.01f * critAdditionPercent));
			}
			else {
				damageFloat = attack;
			}
			if(targetInDefense){
				Debug.Log(oppositeTurn + " defended. Original damage is: " + damageFloat);
				damageFloat -= damageFloat * (0.01f * targetDefense);
				Debug.Log("Reduced to: " + damageFloat);
			}
				
		}
		else
			Debug.Log(currentTurn + " attack Missed.");

		damage = Mathf.RoundToInt(damageFloat);

		return damage;
	}

	void Win(){
		executedWin = true;
		Debug.Log("You Win");
	}

	void Lose(){
		executedLose = true;
		Debug.Log("You Lose");
	}
}
