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

	public bool battleOnGoing;
	private bool executedPlayerTurn;
	private bool executedEnemyTurn;
	private bool executedWin;
	private bool executedLose;

	public CanvasGroup endBattlePanel;
	public Text endBattlePrompt;
	public Button endBattleButton;

	public UIFader commandPrompt;
	private Text commandPromptText;

	public GameObject floatingTextPrefab;
	public AnimationClip floatingTextAnim;

	public Transform floatingTextPanel;
	void Awake(){
		commandPromptText = commandPrompt.GetComponentInChildren<Text>();
	}

	void Update () {

		if(!battleOnGoing)
			return;
		
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

	public void Begin() {
		currentState = BattleStates.START;
		battleOnGoing = true;
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
		if(damage > 0){
			setFloatingText("-" + damage.ToString());
		}
		else{
			setFloatingText("Hindi tumama");
		}
		return damage;
	}

	void Win(){
		LevelLoader levelLoader = FindObjectOfType<LevelLoader> ();
		ObjectiveMapper objectiveMapper = GetComponentInChildren<ObjectiveMapper> ();

		executedWin = true;
		levelLoader.launchPrevScene ();
		objectiveMapper.checkIfCurrent_misc ();
		//setEndBattlePanelMsg("Wagi!", "Tumuloy");
		Debug.Log("You Win");

	}

	void Lose(){
		LevelLoader levelLoader = FindObjectOfType<LevelLoader> ();
		GenericMinigameManger minigameManager = FindObjectOfType<GenericMinigameManger>();
		minigameManager.lose();
		executedLose = true;
		//levelLoader.launchScene ("(Minigame) Battle");
		//setEndBattlePanelMsg("Talo", "Ulitin");
		Debug.Log("You Lose");
	}

	void setEndBattlePanelMsg(string promptText, string buttonText){
		endBattlePanel.blocksRaycasts = true;
		endBattlePanel.alpha = 1;
		endBattlePanel.interactable = true;
		endBattlePrompt.text = promptText;
		endBattleButton.GetComponentInChildren<Text>().text = buttonText;
	}

	public void endBattleButtonClick(){
		LevelLoader levelLoader = FindObjectOfType<LevelLoader> ();
		ObjectiveMapper objectiveMapper = GetComponentInChildren<ObjectiveMapper> ();
		if(executedWin){
			levelLoader.launchPrevScene ();
			objectiveMapper.checkIfCurrent_misc ();
		}
		else if(executedLose){
			levelLoader.launchScene ("(Minigame) Battle");
		}
	}

	public void setCommandPrompt(string prompt){
		commandPrompt.canvasGroup.alpha = 1f;
		commandPromptText.text = prompt;
	}

	public void setFloatingText(string text){
		GameObject floatingText = (GameObject)Instantiate(floatingTextPrefab, Vector3.zero, Quaternion.identity);
		Text[] texts = floatingText.GetComponentsInChildren<Text>();
		floatingText.transform.SetParent(floatingTextPanel, true);
		floatingText.transform.localScale = new Vector3(1,1,1);

		if(currentState == BattleStates.PLAYERCHOICE){
			floatingText.transform.position = Camera.main.WorldToScreenPoint(FindObjectOfType<BattleSetup>().enemyAvatar.transform.position);
		}
		else if(currentState == BattleStates.ENEMYCHOICE){
			Vector3 tempPosition = new Vector3(player.transform.position.x + 3f, player.transform.position.y - 5f, player.transform.position.z);
			floatingText.transform.position = Camera.main.WorldToScreenPoint(tempPosition);
		}

		foreach(Text _text in texts){
			_text.text = text;
		}
		StartCoroutine(destroyFloatingText(floatingText));
	}
	IEnumerator destroyFloatingText(GameObject floatingText){
		yield return new WaitForSeconds(floatingTextAnim.length);
		Destroy(floatingText);
	}
}
