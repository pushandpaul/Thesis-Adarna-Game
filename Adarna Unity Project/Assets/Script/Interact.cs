using UnityEngine;
using System.Collections;
using Fungus;

public class Interact : MonoBehaviour {

	public bool isNPC;
	private bool waitForPress;

	public bool requirePress;
	public Flowchart flowchart;

	public PlayerController player;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(waitForPress && Input.GetKeyDown(KeyCode.E)){
			if(isNPC){
				Debug.Log ("Interacted with NPC " + gameObject.name);
				flowchart.SendFungusMessage(gameObject.name);

			}
				
			else{
				Debug.Log ("Interacted with Object");
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other){
		Debug.Log("Collided with " + gameObject.name);
		
		if(requirePress){
			waitForPress = true;
			return;
		}
		if(other.name == "Player"){
			Debug.Log ("Talk NPC.");
		}
	}
	
	void OnTriggerExit2D (Collider2D other){
		if(other.name == "Player"){
			waitForPress = false;
		}
	}
}
