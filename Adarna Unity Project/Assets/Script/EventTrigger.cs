using UnityEngine;
using System.Collections;
using Fungus;

public class EventTrigger : MonoBehaviour {

	public Flowchart flowchart; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other){
		flowchart.SendFungusMessage(gameObject.name);
		Destroy(this.gameObject);
	}
}
