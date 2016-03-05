using UnityEngine;
using System.Collections;

public class DeadEndHandler : MonoBehaviour {

	public PlayerController player;
	public string animContainer;
	private Transform playerChild;

	void Start(){
		player = FindObjectOfType<PlayerController>();
		playerChild = player.transform.FindChild(animContainer);
	}
	void OnCollisionEnter2D (Collision2D other){
		if(other.gameObject.tag == "Player"){
			playerChild.GetComponent<Animator>().Play("Idle"); 
			playerChild.GetComponent<Animator>().enabled = false;

		}
			
	}
	void OnCollisionExit2D (Collision2D other){
		if(other.gameObject.tag == "Player")
			playerChild.GetComponent<Animator>().enabled = true;
	}
}
