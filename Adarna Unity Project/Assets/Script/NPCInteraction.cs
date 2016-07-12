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

	public bool onlyTrigger = false;
	public Transform toTransform;

	public bool facingRight;
	public bool standardFacing;

	public bool allowFlip = true;
	private bool toFlip;
	private Vector3 backupScale;

	private ObjectiveMapper objectiveMapper;
	public bool anObjective;

	public string message;
	public string originalMessage;

	public SpeechBubble bubble;

	void Awake(){
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
		if(onlyTrigger){
			toTransform = transform.parent;
		}
		else
			toTransform = transform;;
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

		}

		if(message == "")
			message = this.name;

		originalMessage = message;
		foreach(Flowchart _flowchart in FindObjectsOfType<Flowchart>()){
			if(_flowchart.tag != "Global Flowchart"){
				flowchart = _flowchart;
			}
		}

		//flowchart = FindObjectOfType<Flowchart>();

		player = FindObjectOfType<PlayerController>();
		colliderOffsetX = this.GetComponent<CircleCollider2D>().offset.x;
		colliderOffsetY = this.GetComponent<CircleCollider2D>().offset.y;

		if(anObjective){
			StartCoroutine(displaySpeechBubble());
		}
	}
	
	// Update is called once per frame
	void Update () { 

		/*if (anObjective) {
			if (objectiveMapper.checkIfCurrent ()) {
				if (Camera.main.WorldToViewportPoint (bubble.transform.position).x < 0 ||
				   Camera.main.WorldToViewportPoint (bubble.transform.position).x > 1) {
					bubble.displayBubble (true);
				}
			}
		}*/

		if(standardFacing){
			if(toTransform.localScale.x > 0)
				facingRight = true;
			else if(toTransform.localScale.x < 0)
				facingRight = false;
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
		if(pressToInteract && waitForPress && Input.GetKeyDown(KeyCode.E)){
			backupScale = toTransform.localScale;
			if(toFlip){
				toTransform.localScale = new Vector3(-backupScale.x, backupScale.y, backupScale.z);
					//facingPlayer == true;
			}

			else
				toTransform.localScale = new Vector3(backupScale.x, backupScale.y, backupScale.z);
			Debug.Log ("Interacted with NPC " + gameObject.name);
			checkIfCurrent();
		}
	}

	void startDialogue(string toSend){
		StartCoroutine(startingDialogue(toSend));
	}

	public void endDialogue(){
		toTransform.localScale = backupScale;
	}

	void checkIfCurrent(){
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
			else if(!pressToInteract)
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

	IEnumerator displaySpeechBubble(){
		bool temp = true;
		bool displayed = false;
		while(temp){
			if(objectiveMapper.checkIfCurrent() && !DialogueController.inDialogue){
				if(!displayed){
					Debug.Log ("Bubble is displayed.");
					bubble.displayBubble (true);
					displayed = true;
				}
			}
			else{
				if(displayed){
					Debug.Log ("Bubble is not displayed.");
					bubble.displayBubble (false);
					displayed = false;
				}
			}
			yield return null;
		}
	}

	IEnumerator startingDialogue(string toSend){
		MoveCharacter moveCharacter = FindObjectOfType<MoveCharacter>();
		PlayerController player = FindObjectOfType<PlayerController>();
		Animator playerAnim = player.GetComponentInChildren<Animator>();

		CircleCollider2D myCollider = this.GetComponent<CircleCollider2D>();
		float myColliderRadius = myCollider.radius;
		float targetDistance = 0f;

		if(!onlyTrigger){
			myColliderRadius *= Mathf.Abs(transform.localScale.x);
		}
		else
			myColliderRadius *= Mathf.Abs(transform.parent.localScale.x);

		targetDistance = Mathf.Abs(myCollider.offset.x) + myColliderRadius + (player.GetComponent<BoxCollider2D>().size.x * Mathf.Abs(player.transform.localScale.x)/2); 
		if(player.transform.localScale.x > 0){
			targetDistance *= -1;
		}

		Vector3 targetPosition = new Vector3(this.transform.position.x + targetDistance, player.transform.position.y, player.transform.position.z);

		Debug.Log(targetPosition);

		player.canMove = false;
		player.canJump = false;
		bubble.displayBubble (false);

		//May use in movecharacter in the future
		while(player.transform.position != targetPosition){
			player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, Time.deltaTime * 4f);
			playerAnim.SetFloat("Speed", Vector3.Distance(player.transform.position, targetPosition));
			yield return null;
		}


		flowchart.SendFungusMessage (toSend);
		if(!DialogueController.inDialogue){
			player.canMove = true;
			player.canJump = true;
		}
	}
}
