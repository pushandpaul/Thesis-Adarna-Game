using UnityEngine;
using System.Collections;
using Fungus;

public class NPCInteraction : MonoBehaviour {

	private bool waitForPress;

	public Flowchart flowchart;

	public PlayerController player;

	private float colliderOffsetX;
	private float colliderOffsetY;

	private float scaleX;
	private float scaleY;

	private bool toFlip;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
		colliderOffsetX = GetComponent<CircleCollider2D>().offset.x;
		colliderOffsetY = GetComponent<CircleCollider2D>().offset.y;
	}
	
	// Update is called once per frame
	void Update () { 
		if(player.transform.localScale.x > 0){
			GetComponent<CircleCollider2D>().offset = new Vector2(colliderOffsetX, colliderOffsetY);
			toFlip = false;
		}
			
		else if(player.transform.localScale.x < 0){
			GetComponent<CircleCollider2D>().offset = new Vector2(-colliderOffsetX, colliderOffsetY);
			toFlip = true;
		}

		if(waitForPress && Input.GetKeyDown(KeyCode.E)){
			if(toFlip)
				transform.localScale = new Vector3(-scaleX, scaleY, 1f);
			else
				transform.localScale = new Vector3(scaleX, scaleY, 1f);
			Debug.Log ("Interacted with NPC " + gameObject.name);
			startDialogue();
		}
	}

	void startDialogue(){
		flowchart.SendFungusMessage(gameObject.name);
	}

	public void endDialogue(){
		transform.localScale = new Vector3(scaleX, scaleY, 1f);
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
