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

		currentPlayer.transform.parent = characterHolder;
		currentPlayer.transform.localPosition = Vector3.zero;
		currentPlayer.tag = "Playable Character";
		currentPlayer.transform.SetAsFirstSibling();
		holderTransform.position = new Vector3(newPlayer.position.x, holderTransform.position.y, holderTransform.position.z);

		toInitialize = false;
		Switch(newPlayer, currentPlayer, this.switchType);
	}

	public void actualSwitch(Transform newPlayer, Transform holderTransfer){//specifies holder in which the previous player will be placed
		playerHolder = FindObjectOfType<PlayerController>();
		Transform holderTransform = playerHolder.transform;

		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		switchType = "actual";

		holderTransfer.position = holderTransform.position;
		currentPlayer.transform.parent = holderTransfer;
		currentPlayer.transform.localPosition = Vector3.zero;
		currentPlayer.tag = "Playable Character";
		currentPlayer.transform.SetAsFirstSibling();
		holderTransform.position = new Vector3(newPlayer.position.x, holderTransform.position.y, holderTransform.position.z);

		toInitialize = false;
		Switch(newPlayer, currentPlayer, this.switchType);
	}

	public void instantSwitch(Transform newPlayer){
		//changes player in an instant and destroys the previous one - used for persistence of the "new player"
		playerHolder = FindObjectOfType<PlayerController>();
		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		switchType = "instant";

		Destroy(currentPlayer);

		toInitialize = true;
		routine = 'a';
		Switch(newPlayer, currentPlayer, this.switchType);
	}

	void Switch(Transform newPlayer, GameObject currentPlayer, string switchType){
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
				if(!toInitialize)
					backupHolderScale = new Vector3(Mathf.Abs(backupHolderScale.x), backupHolderScale.y, backupHolderScale.z);
				Debug.Log("New player scale is greater than 0");
			}

			else if(newPlayer.parent.localScale.x < 0){
				playerHolder.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
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
	}
}
