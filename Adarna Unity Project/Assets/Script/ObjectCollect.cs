using UnityEngine;
using System.Collections;

public class ObjectCollect : MonoBehaviour {
	private bool waitForPress;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(waitForPress && Input.GetKeyDown(KeyCode.E)){
			Debug.Log("Collected: " + gameObject.name);
			//gameObject.SetActive = false;
			Destroy(gameObject);
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
