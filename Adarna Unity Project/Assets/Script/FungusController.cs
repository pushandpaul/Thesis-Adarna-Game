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

		if(gameManager.currentCharacterName == "Olikornyo"){
			PlayerController player = FindObjectOfType<PlayerController>();
			character.nameText = player.item.getItem().name.Replace("(Nakasakay)", "");
		}
		else
			character.nameText = gameManager.currentCharacterName;
	}
}
