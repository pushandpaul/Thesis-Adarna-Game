using UnityEngine;
using System.Collections;

public class PlayerSwitch : MonoBehaviour {
	public Transform targetCharacter;
	private PlayerController playerHolder;
	public Transform characterContainer;

	private string switchType;
	/* 
	 * switchType - "instant" allows the initialization of item held saved, and animation
	 * switchType - "actual" does not allow the initialization of item held saved and animation due to the fact that
	 * 				it may vary in runtime.
	*/
	public char routine = 'a';
	/*
	* routine 'a' - sets the position of the new player to the player holder r
	* routine 'b' - sets the position of the player holder to the new player
	*/
	private bool toInitialize = false;

	public void actualSwitch(Transform newPlayer){ 
		
		//camera follows another player, previous player stays in the scene - used for instances of actual switching players
		playerHolder = FindObjectOfType<PlayerController>();
		Transform holderTransform = playerHolder.transform;
		Transform characterHolder = (Transform)Instantiate(characterContainer, holderTransform.position, holderTransform.rotation);

		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		switchType = "actual";
		characterHolder.localScale = playerHolder.transform.localScale;
		currentPlayer.transform.parent = characterHolder;
		currentPlayer.transform.localPosition = Vector3.zero;
		currentPlayer.tag = "Playable Character";
		currentPlayer.transform.SetAsFirstSibling();
		holderTransform.position = new Vector3(newPlayer.position.x, holderTransform.position.y, holderTransform.position.z);

		toInitialize = false;
		Switch(newPlayer, currentPlayer);
	}

	public void actualSwitch(Transform newPlayer, Transform holderTransfer){//specifies holder in which the previous player will be placed
		playerHolder = FindObjectOfType<PlayerController>();
		Transform holderTransform = playerHolder.transform;

		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		switchType = "actual";

		holderTransfer.position = holderTransform.position;
		holderTransfer.localScale = playerHolder.transform.localScale;
		currentPlayer.transform.parent = holderTransfer;
		currentPlayer.transform.localPosition = Vector3.zero;
		currentPlayer.tag = "Playable Character";
		currentPlayer.transform.SetAsFirstSibling();
		holderTransform.position = new Vector3(newPlayer.position.x, holderTransform.position.y, holderTransform.position.z);

		toInitialize = false;
		Debug.Log ("New Player: " + newPlayer.name);
		Debug.Log ("Current Player: " + currentPlayer.name);
		Switch(newPlayer, currentPlayer);
	}

	public void instantSwitch(Transform newPlayer){
		//changes player in an instant and destroys the previous one - used for persistence of the "new player"
		playerHolder = FindObjectOfType<PlayerController>();
		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");
		Transform currentPlayerDump = GameObject.FindGameObjectWithTag("Default Player Dump").transform;
		Vector3 dumpScale = currentPlayerDump.localScale;

		switchType = "instant";

		if(currentPlayerDump != null){
			currentPlayerDump.localScale = playerHolder.transform.localScale;
			currentPlayer.transform.parent = currentPlayerDump;
			currentPlayer.transform.localPosition = Vector3.zero;
			currentPlayer.transform.SetAsFirstSibling();
			if(dumpScale.x > 0)
				currentPlayerDump.localScale = new Vector3(Mathf.Abs(currentPlayerDump.localScale.x), currentPlayerDump.localScale.y, currentPlayerDump.localScale.z);
			else if(dumpScale.x < 0)
				currentPlayerDump.localScale = new Vector3(-Mathf.Abs(currentPlayerDump.localScale.x), currentPlayerDump.localScale.y, currentPlayerDump.localScale.z);
			currentPlayer.tag = "Playable Character";
		}
		else	
			Destroy(currentPlayer);

		toInitialize = true;
		routine = 'a';
		Switch(newPlayer, currentPlayer);
	}

	void Switch(Transform newPlayer, GameObject currentPlayer){
		Vector3 backupHolderScale = playerHolder.transform.localScale;
		Debug.Log(backupHolderScale);
		GameManager gameManager = FindObjectOfType<GameManager>();
		SpriteRenderer[] Current = currentPlayer.GetComponentsInChildren<SpriteRenderer>(true);
		SpriteRenderer[] New = newPlayer.GetComponentsInChildren<SpriteRenderer>(true);

		foreach(SpriteRenderer _current in Current){
			_current.sortingLayerName = "NPC";
		}

		foreach(SpriteRenderer _new in New){
			_new.sortingLayerName = "Player";
		}

		playerHolder.anim.SetFloat("Speed", 0);
		playerHolder.anim.SetBool("Ground", true);

		playerHolder.anim = newPlayer.GetComponent<Animator>();
		playerHolder.item = newPlayer.GetComponentInChildren<ItemToGive>(true);

		if(newPlayer.parent != null){
			if(newPlayer.parent.localScale.x > 0){
				playerHolder.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				newPlayer.parent.localScale = new Vector3(0.5f, 0.5f, 1f);
				if(!toInitialize)
					backupHolderScale = new Vector3(Mathf.Abs(backupHolderScale.x), backupHolderScale.y, backupHolderScale.z);
				Debug.Log("New player scale is greater than 0");
			}

			else if(newPlayer.parent.localScale.x < 0){
				playerHolder.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
				newPlayer.parent.localScale = new Vector3(-0.5f, 0.5f, 1f);
				if(!toInitialize)
					backupHolderScale = new Vector3(-(Mathf.Abs(backupHolderScale.x)), backupHolderScale.y, backupHolderScale.z);
				Debug.Log("New player scale is less than 0");
			}
		}

		else{
			playerHolder.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		}

		newPlayer.parent = playerHolder.transform;
		newPlayer.localPosition = Vector3.zero;
		newPlayer.tag = "Character Controlling";

		playerHolder.transform.localScale = backupHolderScale;
		if(switchType == "instant")
			playerHolder.initState();
		gameManager.currentCharacterName = newPlayer.name;
		gameManager.initPlayerIdleStateHash = playerHolder.anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
	}
}
