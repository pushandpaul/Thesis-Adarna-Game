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

	private bool toFlip;

	private ObjectiveMapper objectiveMapper;
	public bool anObjective;
	//public bool isAchieved;

	public string message;
	void Start () {
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
		if(objectiveMapper != null)
			anObjective = true;
		else{
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
		if(facingRight != player.facingRight){
			GetComponent<CircleCollider2D>().offset = new Vector2(colliderOffsetX, colliderOffsetY);
			toFlip = false;
		}
			
		else if(facingRight == player.facingRight){
			GetComponent<CircleCollider2D>().offset = new Vector2(-colliderOffsetX, colliderOffsetY);
			toFlip = true;
		}
			

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

	void startDialogue(string toSend){
		flowchart.SendFungusMessage(toSend);
		//objectiveMapper.textBox.disableTextBox();
	}

	public void endDialogue(){
		transform.localScale = new Vector3(scaleX, scaleY, 1f);
		//objectiveMapper.textBox.enableTextBox();
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
			else
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
