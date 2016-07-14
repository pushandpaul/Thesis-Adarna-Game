using UnityEngine;
using System.Collections;

public class InteractPrompt : MonoBehaviour {

	public SpriteRenderer myRenderer;
	public Sprite E;
	public Sprite W;
	public Sprite S;

	public enum keyToInteract{
		E,
		W,
		S,
	}

	public Vector2 offset;
	// Use this for initialization
	void Awake () {
		myRenderer = this.GetComponent<SpriteRenderer>();
	}

	public void show(keyToInteract key, bool show, Transform referencePoint){
		myRenderer.enabled = show;
		if(show){
			transform.position = new Vector3(referencePoint.position.x + offset.x, referencePoint.position.y + offset.y, referencePoint.position.z);
			switch(key){
			case(keyToInteract.E): myRenderer.sprite = E; 
				break;
			case(keyToInteract.W): myRenderer.sprite = W; 
				break;
			case(keyToInteract.S): myRenderer.sprite = S; 
				break;
			}
		}
	}
}
