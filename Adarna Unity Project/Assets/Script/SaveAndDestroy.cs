using UnityEngine;
using System.Collections;

public class SaveAndDestroy : MonoBehaviour {

	// Use this for initialization
	private GameManager gameManager;

	void Start () {
		gameManager = FindObjectOfType<GameManager>();
	}
	public void saveAndDestroy(GameObject myGameObject){
		ObjectData objectData = this.GetComponent<ObjectData>();
		if(objectData != null){
			objectData.destroyed = true;
			gameManager.searchData(objectData, 's');
		}
		Destroy(myGameObject);
	}
}
