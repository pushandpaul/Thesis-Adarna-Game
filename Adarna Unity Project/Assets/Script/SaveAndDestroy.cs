using UnityEngine;
using System.Collections;

public class SaveAndDestroy : MonoBehaviour {

	// Use this for initialization
	private GameManager gameManager;

	void Awake () {
		gameManager = FindObjectOfType<GameManager>();
	}
	public void saveAndDestroy(GameObject myGameObject){
		ObjectData objectData = myGameObject.GetComponent<ObjectData>();
		if(objectData != null){
			objectData.destroyed = true;
			gameManager.searchObjectData(objectData, 's');
		}
		Destroy(myGameObject);
	}

	public void saveNowDestroyLater(GameObject myGameObject){
		ObjectData objectData = myGameObject.GetComponent<ObjectData>();
		if(objectData != null){
			objectData.destroyed = true;
			gameManager.searchObjectData(objectData, 's');
		}
	}
}
