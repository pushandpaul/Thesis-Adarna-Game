using UnityEngine;
using System.Collections;

public class ObjectCollect : MonoBehaviour {
	private bool waitForPress;
	private ObjectData objectData;
	private GameManager gameManager;
	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		objectData = this.GetComponent<ObjectData>();
	}

	void Update () {
		if(waitForPress && Input.GetKeyDown(KeyCode.E)){
			Debug.Log("Collected: " + gameObject.name);
			objectData.destroyed = true;
			gameManager.searchData(this.objectData,'s'); //saves object data to game manager, before the object is destroyed
			Destroy(this.gameObject);
		}
			
		else
			return;
	}
	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player")
			waitForPress = true;
	}
	void OnTriggerExit2D (Collider2D other){
		if(other.tag == "Player")
			waitForPress = false;
	}
}
