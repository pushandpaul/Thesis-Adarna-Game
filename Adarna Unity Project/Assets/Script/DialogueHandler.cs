using UnityEngine;
using System.Collections;

public class DialogueHandler : MonoBehaviour {

	public PlayerController player;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void enablePlayerMovement(){
		player.canMove = true;
	}

	void disablePlayerMovement(){
		player.canMove = false;
	}
}
