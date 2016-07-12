using UnityEngine;
using System.Collections;
using Fungus;

public class FungusController : MonoBehaviour {

	// Use this for initialization
	public void ChangeCharacter(Character character, string characterName){
		character.nameText = characterName;
	}

	public void SetPlayerAsCharacter(Character character){
		GameManager gameManager = FindObjectOfType<GameManager> ();
		character.nameText = gameManager.currentCharacterName;
	}
}
