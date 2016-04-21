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

		Switch(newPlayer);
	}

	public void instantSwitch(Transform newPlayer){
		//changes player in an instant and destroys the previous one - used for persistence of the "new player"
		playerHolder = FindObjectOfType<PlayerController>();
		GameObject currentPlayer = GameObject.FindGameObjectWithTag("Character Controlling");

		Destroy(currentPlayer);

		Switch(newPlayer);
	}

	void Switch(Transform newPlayer){
		Vector3 backupHolderScale = playerHolder.transform.localScale;
		GameManager gameManager = FindObjectOfType<GameManager>();

		playerHolder.anim.SetFloat("Speed", 0);
		playerHolder.anim.SetBool("Ground", true);

		playerHolder.anim = newPlayer.GetComponent<Animator>();
		playerHolder.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

		newPlayer.parent = playerHolder.transform;
		newPlayer.localPosition = Vector3.zero;
		newPlayer.tag = "Character Controlling";

		playerHolder.transform.localScale = backupHolderScale;

		gameManager.currentCharacterName = newPlayer.name;
	}
}
