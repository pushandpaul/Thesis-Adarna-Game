using UnityEngine;
using System.Collections;

public class ObjectCollect : MonoBehaviour {
	private bool waitForPress;
	private ObjectData objectData;
	private GameManager gameManager;
	private ObjectiveMapper objectiveMapper;
	private PlayerController playerController;

	public bool includedInMassCollect = false;

	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		objectData = this.GetComponent<ObjectData>();
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
		playerController = FindObjectOfType<PlayerController>();
	}

	void Update () {

		if(!includedInMassCollect)
			return;
		if(waitForPress && Input.GetKeyDown(KeyCode.E)){
			FindObjectOfType<ItemCollectionManager>().Collect(gameObject);
		}
	}
	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player")
			waitForPress = true;
	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player")
			waitForPress = false;
	}

	public void carryItem() {
		Debug.Log ("Collected: " + gameObject.name);
		if(objectData != null){
			objectData.destroyed = true;
			gameManager.searchObjectData (this.objectData, 's'); //saves object data to game manager, before the object is destroyed
		}
		Destroy (this.gameObject);
		//playerController.setPlayerSate ("Carry Item (Idle)");
	}

}
