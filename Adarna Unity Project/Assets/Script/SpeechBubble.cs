using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {

	// Use this for initialization
	void Start () {
		hideBubble ();
	}

	public void hideBubble() {
		GetComponent<Renderer>().enabled = false;
	}

	public void showBubble() {
		GetComponent<Renderer>().enabled = true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
