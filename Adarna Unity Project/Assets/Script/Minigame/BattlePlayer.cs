using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattlePlayer : MonoBehaviour {

	private BattleStateMachine battleStateMachine;
	private CameraController camera;
	private BattleEnemy enemy;
	public Stats_BattlePlayer stats;

	public bool inDefense;
	private int attackCharge = 0;

	//public Button[] commandButtons;
	public Button attackButton;
	public Button defendButton;
	public Button strongAttackButton;

	public Image portraitUI;
	public Sprite lowHPPortrait;
	public Sprite medHPPortrait;
	public Sprite highHPPortrait;

	//Temporary
	public Text currentHPTextBox;
	public Text baseHPTextBox;

	private Animator anim;

	public enum MoveSet{
		Attack,
		StrongAttack,
		Defend,
	}

	public enum CombatStance{
		Sword,
		Brawl,
	}

	[System.Serializable]
	public class StanceMoveSet{
		public CombatStance stance;
		public AnimationClip idleAnim;
		public Move attack;
		public Move defend;
		public Move strongAttack;
	}

	[System.Serializable]
	public class Move{
		public AnimationClip animation;
		public Sprite icon;
		public Sprite iconHighligted;
		public Sprite iconPressed;
		public Sprite iconDisable;
	}

	public StanceMoveSet[] stanceMoveSets;
	public static CombatStance currCombatStance;
	private StanceMoveSet currStanceMoveSet;

	public CanvasGroup commandButtonGroup;

	void Awake(){
		battleStateMachine = FindObjectOfType<BattleStateMachine>();
		anim = GetComponentInChildren<Animator>();
		camera = FindObjectOfType<CameraController>();
		//currCombatStance = CombatStance.Sword;
		foreach(StanceMoveSet stanceMoveSet in stanceMoveSets){
			if(currCombatStance == stanceMoveSet.stance){
				currStanceMoveSet = stanceMoveSet;
				initButtonIcons(attackButton, currStanceMoveSet.attack);
				initButtonIcons(defendButton, currStanceMoveSet.defend);
				initButtonIcons(strongAttackButton, currStanceMoveSet.strongAttack);
				break;
			}
		}
		//currStanceMoveSet = stanceMoveSets[0];
	}

	void Start(){
		if(battleStateMachine != null){
			anim.Play(currStanceMoveSet.idleAnim.name);
			baseHPTextBox.text = "/" + stats.baseHP;
			stats.currentHP = stats.baseHP;
			currentHPTextBox.text = "" + stats.currentHP;
			enemy = battleStateMachine.enemy;
			setButtonCanvas(true);

			attackButton.interactable = false;
			defendButton.interactable = false;
			strongAttackButton.interactable = false;

			/*foreach(Button commandButton in commandButtons){
				commandButton.interactable = false;
			}*/
		}
	}

	public void StartTurn(){
		Debug.Log("-------------------------------------------------");
		Debug.Log("Player Turn");

		inDefense = false;
		anim.SetBool("Defend", inDefense);

		camera.setPlayerAsFollowing();
		camera.Zoom(5.0f, 0.8f);
		setButtonCanvas(true);

		attackButton.interactable = true;
		defendButton.interactable = true;

		if(attackCharge == 2){
			strongAttackButton.interactable = true;
		}
		else if(attackCharge < 2){
			strongAttackButton.interactable = false;
		}
		/*
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
		}*/
	}

	public void Attack(){
		Debug.Log("Player attacked!");
		battleStateMachine.setCommandPrompt("Umatake si Don Juan!");
		commandPicked();
		StartCoroutine(StartAttackAnim(MoveSet.Attack));
		/*enemy.DecreaseHP(battleStateMachine.ComputeDamage(stats.accuracy, stats.criticalChance, stats.critAdditionPercent, stats.attack, enemy.inDefense, enemy.stats.defense));
		attackCharge++;
		if(attackCharge > 2)
			attackCharge = 2;*/
		//EndTurn();
	}

	public void StrongAttack(){
		Debug.Log("Player used strong attack!");
		battleStateMachine.setCommandPrompt("Ginamit ni Don Juan ang Malakas na Pag-atake!");
		commandPicked();
		StartCoroutine(StartAttackAnim(MoveSet.StrongAttack));
		/*attackCharge = 0;
		enemy.DecreaseHP(battleStateMachine.ComputeDamage(stats.accuracy, stats.criticalChance, stats.critAdditionPercent, stats.attack * 2, enemy.inDefense, enemy.stats.defense));
		EndTurn();*/
	}

	public void Defend(){
		inDefense = true;
		battleStateMachine.setCommandPrompt("Dumepensa si Don Juan.");
		commandPicked();
		StartCoroutine(StartAnimVarControlled(MoveSet.Defend));
		//EndTurn();
	}

	public void DecreaseHP(int damage){
		stats.currentHP -= damage;
		if(stats.currentHP < 0){
			stats.currentHP = 0;
		}

		if(stats.currentHP < .20 * stats.baseHP){
			portraitUI.sprite = lowHPPortrait;
		}
		else if(stats.currentHP < .70 * stats.baseHP){
			portraitUI.sprite = medHPPortrait;
		}
		else
			portraitUI.sprite = highHPPortrait;
		currentHPTextBox.text = "" + stats.currentHP;

		return;
	}

	void commandPicked(){
		setButtonCanvas(false);
		//commandButtonGroup.alpha = 0f;
		camera.revertFollowing();
		camera.Zoom(camera.initialCamSize, 0.8f);
	}

	public void EndTurn(){
		Debug.Log("Player turn has ended.");
		battleStateMachine.commandPrompt.canvasGroup.alpha = 0f;
		if(enemy.stats.currentHP == 0){
			battleStateMachine.currentState = BattleStateMachine.BattleStates.WIN;
		}
			
		else
			battleStateMachine.currentState = BattleStateMachine.BattleStates.ENEMYCHOICE;
	}

	void setButtonCanvas(bool enable){
		if(enable){
			commandButtonGroup.alpha = 1f;
		}
		else
			commandButtonGroup.alpha = 0f;
		
		commandButtonGroup.interactable = enable;
	}

	void initButtonIcons(Button button, Move move){

		SpriteState mySpriteState = new SpriteState();

		button.image.sprite = move.icon;
		mySpriteState.highlightedSprite = move.iconHighligted;
		mySpriteState.pressedSprite = move.iconPressed;
		mySpriteState.disabledSprite = move.iconDisable;

		button.spriteState = mySpriteState;
	}

	IEnumerator StartAttackAnim(MoveSet attackMove){
		string animationName = "";
		AnimationClip myMoveAnim = new AnimationClip();
		float attackPower = 0f;
		int computedDamage = 0;

		if(attackMove == MoveSet.Attack){
			animationName = currStanceMoveSet.attack.animation.name;
			myMoveAnim = currStanceMoveSet.attack.animation;
			attackPower = stats.attack;
		}
		else if(attackMove == MoveSet.StrongAttack){
			animationName = currStanceMoveSet.strongAttack.animation.name;
			myMoveAnim = currStanceMoveSet.strongAttack.animation;
			attackPower = stats.attack * 2;
			attackCharge = 0;
		}
			
		anim.Play(animationName);

		yield return new WaitForSeconds(myMoveAnim.length);

		computedDamage = battleStateMachine.ComputeDamage(stats.accuracy, stats.criticalChance, stats.critAdditionPercent, attackPower, enemy.inDefense, enemy.stats.defense);

		if(computedDamage > 0){
			enemy.DecreaseHP(computedDamage);
			if(attackMove == MoveSet.Attack){
				attackCharge++;
				if(attackCharge > 2)
					attackCharge = 2;
			}

		}
		EndTurn();
	}
	IEnumerator StartAnimVarControlled(MoveSet move){
		bool varIsBool = false;
		bool boolTrigger = false;
		string variableName = "";
		AnimationClip myMoveAnim = new AnimationClip();

		if(move == MoveSet.Defend){
			varIsBool = true;
			boolTrigger = inDefense;
			variableName = "Defend";
			myMoveAnim = currStanceMoveSet.defend.animation;
		}

		if(varIsBool)
			anim.SetBool(variableName, boolTrigger);
		
		yield return new WaitForSeconds(myMoveAnim.length);

		EndTurn();
	}
}
