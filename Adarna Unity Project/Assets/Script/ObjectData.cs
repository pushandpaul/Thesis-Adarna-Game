using UnityEngine;
using System.Collections;

public class ObjectData : MonoBehaviour {

	//public Vector3 coordinates;
	public string Name;
	public bool destroyed;
	void Awake () {
		this.Name = this.name;
	}

	public void enableSprite(bool boolean) {
		SpriteRenderer sr = this.GetComponent<SpriteRenderer>(); 
		sr.enabled = boolean;
	}
}
