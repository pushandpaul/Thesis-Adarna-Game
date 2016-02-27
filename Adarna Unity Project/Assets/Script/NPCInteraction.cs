using UnityEngine;
using System.Collections;
using Fungus;

public class NPCInteraction : MonoBehaviour {

	private bool waitForPress;

	public Flowchart flowchart;

	public PlayerController player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {

		if(waitForPress && Input.GetKeyDown(KeyCode.E)){
			Debug.Log ("Interacted with NPC " + gameObject.name);
			flowchart.SendFungusMessage(gameObject.name);
		}
	}

	void OnTriggerEnter2D (Collider2D other){
		Debug.Log("Collided with " + gameObject.name);
		if(other.tag == "Player"){
			Debug.Log ("Interact");
			waitForPress = true;
		}
	}
	
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player")
			waitForPress = false;
	}
}
