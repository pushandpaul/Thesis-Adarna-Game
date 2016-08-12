using UnityEngine;
using System.Collections;

public class Underwater_CollectSingsing : MonoBehaviour {

	public bool collected;
	private bool waitForPress;
	private ObjectiveMapper objectiveMapper;
	private InteractPrompt interactionPrompt;

	// Use this for initialization

	void Awake(){
		objectiveMapper = GetComponent<ObjectiveMapper>();
		interactionPrompt = FindObjectOfType<InteractPrompt>();
	}

	void Update(){
		if(waitForPress && Input.GetKeyDown(KeyCode.E)){
			if(interactionPrompt != null)
				interactionPrompt.show(InteractPrompt.keyToInteract.E, false, transform);
			collected = true;
			Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player"){
			waitForPress = true;
			if(interactionPrompt != null)
				interactionPrompt.show(InteractPrompt.keyToInteract.E, true, transform);
		}

	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player"){
			waitForPress = false;
			if(interactionPrompt != null)
				interactionPrompt.show(InteractPrompt.keyToInteract.E, false, transform);
		}
	}
}
