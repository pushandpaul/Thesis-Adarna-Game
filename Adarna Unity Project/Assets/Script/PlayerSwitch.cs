using UnityEngine;
using System.Collections;

public class PlayerSwitch : MonoBehaviour {
	public Transform targetCharacter;
	private PlayerController playerHolder;
	public Transform characterContainer;

	public void actualSwitch(Transform newPlayer){ 
		//camera follows another player, previous player stays in the scene - used for instances of actual switching players
		playerHolder = FindObjectOfType<PlayerController>();
		Transform holderTransform = playerHolder.transform;
		Transform characterHolder = (Transform)Instantiate(characterContainer, holderTransform.position, holderTransform.rotation);

		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		currentPlayer.transform.parent = characterHolder;
		currentPlayer.transform.localPosition = Vector3.zero;
		holderTransform.position = new Vector3(newPlayer.position.x, holderTransform.position.y, holderTransform.position.z);

		Switch(newPlayer, currentPlayer);
	}

	public void actualSwitch(Transform newPlayer, Transform holderTransfer){//specifies holder in which the previous player will be placed
		playerHolder = FindObjectOfType<PlayerController>();
		Transform holderTransform = playerHolder.transform;

		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		holderTransfer.position = holderTransform.position;
		currentPlayer.transform.parent = holderTransfer;
		currentPlayer.transform.localPosition = Vector3.zero;
		holderTransform.position = new Vector3(newPlayer.position.x, holderTransform.position.y, holderTransform.position.z);

		Switch(newPlayer, currentPlayer);
	}

	public void instantSwitch(Transform newPlayer){
		//changes player in an instant and destroys the previous one - used for persistence of the "new player"
		playerHolder = FindObjectOfType<PlayerController>();
		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		Destroy(currentPlayer);

		Switch(newPlayer, currentPlayer);
	}

	void Switch(Transform newPlayer, GameObject currentPlayer){
		Vector3 backupHolderScale = playerHolder.transform.localScale;
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

		if(newPlayer.parent != null){
			if(newPlayer.parent.localScale.x > 0){
				playerHolder.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				backupHolderScale = new Vector3(Mathf.Abs(backupHolderScale.x), backupHolderScale.y, backupHolderScale.z);
				Debug.Log("New player scale is greater than 0");
			}

			else if(newPlayer.parent.localScale.x < 0){
				playerHolder.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
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

		gameManager.currentCharacterName = newPlayer.name;
	}
}
