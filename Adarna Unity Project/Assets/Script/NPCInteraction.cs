using UnityEngine;
using System.Collections;
using Fungus;

public class NPCInteraction : MonoBehaviour {

	private bool waitForPress;
	public bool pressToInteract;

	public Flowchart flowchart;

	public PlayerController player;

	private float colliderOffsetX;
	private float colliderOffsetY;

	private float scaleX;
	private float scaleY;

	public bool facingRight;

	public bool allowFlip = true;
	private bool toFlip;

	private ObjectiveMapper objectiveMapper;
	public bool anObjective;
	//public bool isAchieved;

	public string message;

	public SpeechBubble bubble;

	void Awake(){
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
	}

	void Start () {
		if(bubble == null){
			bubble = this.GetComponentInChildren<SpeechBubble> ();
		}
		//bubble = this.GetComponentInChildren<SpeechBubble> ();
		//objectiveMapper = this.GetComponent<ObjectiveMapper>();
		if (objectiveMapper != null) {
			anObjective = true;

		} else {
			anObjective = false;
			message = this.name;
		}
		flowchart = FindObjectOfType<Flowchart>();

		player = FindObjectOfType<PlayerController>();
		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
		colliderOffsetX = this.GetComponent<CircleCollider2D>().offset.x;
		colliderOffsetY = this.GetComponent<CircleCollider2D>().offset.y;

	}
	
	// Update is called once per frame
	void Update () { 
		if (anObjective) {
			if (objectiveMapper.checkIfCurrent ()) {
				if (Camera.main.WorldToViewportPoint (bubble.transform.position).x < 0 ||
				   Camera.main.WorldToViewportPoint (bubble.transform.position).x > 1) {
					bubble.displayBubble (true);
				}
			}
		}

		if(allowFlip){
			if(facingRight != player.facingRight){
				GetComponent<CircleCollider2D>().offset = new Vector2(colliderOffsetX, colliderOffsetY);
				toFlip = false;
			}

			else if(facingRight == player.facingRight){
				GetComponent<CircleCollider2D>().offset = new Vector2(-colliderOffsetX, colliderOffsetY);
				toFlip = true;
			}
		}
		else
			toFlip = false;
		if(!DialogueController.inDialogue){
			if(pressToInteract && waitForPress && Input.GetKeyDown(KeyCode.E)){
				if(toFlip){
					transform.localScale = new Vector3(-scaleX, scaleY, 1f);
					//facingPlayer == true;
				}

				else
					transform.localScale = new Vector3(scaleX, scaleY, 1f);
				Debug.Log ("Interacted with NPC " + gameObject.name);
				checkIfCurrent();
			}
		}
	}

	/*public void flipCharacter(bool _facingRight) {
		if(facingRight != _facingRight){
			Debug.Log ("FACING RIGHT != _FACING RIGHT");
			GetComponent<CircleCollider2D>().offset = new Vector2(colliderOffsetX, colliderOffsetY);
			toFlip = true;
		} else if(facingRight == _facingRight){
			Debug.Log ("FACING RIGHT == _FACING RIGHT");
			GetComponent<CircleCollider2D>().offset = new Vector2(-colliderOffsetX, colliderOffsetY);
			toFlip = false;
		}

		if(!facingRight){
			transform.localScale = new Vector3(-scaleX, scaleY, 1f);
			//facingPlayer == true;
		} else
			transform.localScale = new Vector3(scaleX, scaleY, 1f);
	}*/

	void startDialogue(string toSend){
		bubble.displayBubble (false);
		flowchart.SendFungusMessage(toSend);
	}

	public void endDialogue(){
		transform.localScale = new Vector3(scaleX, scaleY, 1f);
	}

	private void checkIfCurrent(){
		if(anObjective){
			objectiveMapper.checkIfCurrent_npc();
		}
		Debug.Log(message);
		startDialogue(message);
	}
	void OnTriggerEnter2D (Collider2D other){
		Debug.Log("Collided with " + gameObject.name);
		if(other.tag == "Player"){
			if(pressToInteract){
				Debug.Log ("Interact");
				waitForPress = true;
			}
			else if(!pressToInteract && !DialogueController.inDialogue)
				checkIfCurrent();
		}
	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player"){
			if(pressToInteract){
				waitForPress = false;
			}
		}
	}
}
